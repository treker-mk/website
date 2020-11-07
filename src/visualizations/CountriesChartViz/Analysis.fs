﻿module CountriesChartViz.Analysis

open Data.OurWorldInData
open Types
open System

type MetricToDisplay =
    | NewCasesPer1M
    | ActiveCasesPer1M
    | TotalDeathsPer1M
    | DeathsPerCases

type CountryDataDayEntry = {
    Date: DateTime
    Value: float
}

type CountryData = {
    IsoCode : CountryIsoCode
    Entries: CountryDataDayEntry[]
}

type CountriesData = Map<CountryIsoCode, CountryData>

let groupEntriesByCountries
    (metricToDisplay: MetricToDisplay) (entries: DataPoint list)
    : CountriesData =

    let transformFromRawOwid (entryRaw: DataPoint)
            : CountryDataDayEntry option =
        match metricToDisplay with
        | NewCasesPer1M ->
            match entryRaw.NewCasesPerMillion with
            | Some value -> Some { Date = entryRaw.Date; Value = value / 10. }
            | None -> None
        | ActiveCasesPer1M ->
            match entryRaw.NewCasesPerMillion with
            | Some value -> Some { Date = entryRaw.Date; Value = value / 10. }
            | None -> None
        | TotalDeathsPer1M ->
            match entryRaw.TotalDeathsPerMillion with
            | Some value -> Some { Date = entryRaw.Date; Value = value / 10. }
            | None -> None
        | DeathsPerCases ->
            match entryRaw.TotalDeaths, entryRaw.TotalCases with
            | Some totalDeaths, Some totalCases ->
                if totalCases > 0 then
                    let value = (float totalDeaths) * 100.0 / (float totalCases)
                    Some { Date = entryRaw.Date; Value = value }
                else
                    None
            | _ -> None

    let groupedRaw =
        entries |> Seq.groupBy (fun entry -> entry.CountryCode)

    groupedRaw
    |> Seq.map (fun (isoCode, countryEntriesRaw) ->
        let countryEntries =
            countryEntriesRaw
            |> Seq.choose transformFromRawOwid
            |> Seq.toArray

        (isoCode, { IsoCode = isoCode; Entries = countryEntries } )
    ) |> Map.ofSeq

let calculateActiveCases (countryEntries: CountryDataDayEntry[]) =
    let entriesCount = countryEntries.Length

    let mutable runningActiveCases = 0.
    Array.init entriesCount (fun i ->
        let countryEntry = countryEntries.[i]

        runningActiveCases <- runningActiveCases + countryEntry.Value
        if i >= 14 then
            runningActiveCases <-
                runningActiveCases - countryEntries.[i - 14].Value

        { countryEntry with Value = runningActiveCases }
    )

let calculateMovingAverages
    daysOfMovingAverage
    (countryEntries: CountryDataDayEntry[]) =

    let entriesCount = countryEntries.Length
    let cutOff = daysOfMovingAverage / 2
    let averagesSetLength = entriesCount - cutOff * 2

    let averages: CountryDataDayEntry[] = Array.zeroCreate averagesSetLength

    let daysOfMovingAverageFloat = float daysOfMovingAverage
    let mutable currentSum = 0.

    let movingAverageFunc index =
        let entry = countryEntries.[index]

        currentSum <- currentSum + entry.Value

        match index with
        | index when index >= daysOfMovingAverage - 1 ->
            let date = countryEntries.[index - cutOff].Date
            let average = currentSum / daysOfMovingAverageFloat

            averages.[index - (daysOfMovingAverage - 1)] <- {
                Date = date; Value = average }

            let entryToRemove =
                countryEntries.[index - (daysOfMovingAverage - 1)]
            currentSum <- currentSum - entryToRemove.Value

        | _ -> ignore()

    for i in 0 .. entriesCount-1 do
        movingAverageFunc i

    averages

type OwidDataState =
    | NotLoaded
    | PreviousAndLoadingNew of OurWorldInDataRemoteData
    | Current of OurWorldInDataRemoteData

let SloveniaPopulationInM =
    (Utils.Dictionaries.regions.["mk"].Population
    |> Utils.optionToInt
    |> float)
    / 1000000.

let buildFromSloveniaDomesticData (statsData: StatsData) (date: DateTime)
        : DataPoint option =
    let domesticDataForDate =
        statsData
        |> List.tryFind(fun dataForDate -> dataForDate.Date = date)

    let extractMetricIfPresent (metricValue: int option)
            : (int option * float option) =
        match metricValue with
        | Some value ->
            (Some value, (float value) / SloveniaPopulationInM |> Some)
        | None -> (None, None)

    match domesticDataForDate with
    | Some domesticDataForDate ->
        let newCases, newCasesPerM =
            extractMetricIfPresent domesticDataForDate.Cases.ConfirmedToday
        let totalCases, totalCasesPerM =
            extractMetricIfPresent domesticDataForDate.Cases.ConfirmedToDate
        let totalDeaths, totalDeathsPerM =
            extractMetricIfPresent
                domesticDataForDate.StatePerTreatment.DeceasedToDate

        {
            CountryCode = "MKD"; Date = date
            NewCases = newCases
            NewCasesPerMillion = newCasesPerM
            TotalCases = totalCases
            TotalCasesPerMillion = totalCasesPerM
            TotalDeaths = totalDeaths
            TotalDeathsPerMillion = totalDeathsPerM
        } |> Some
    | None -> None

let updateWithSloveniaDomesticData
        (statsData: StatsData) (countryData: DataPoint): DataPoint option =
    match countryData.CountryCode with
    | "MKD" ->
        countryData.Date |> buildFromSloveniaDomesticData statsData
    | _ -> Some countryData

let aggregateOurWorldInData
    daysOfMovingAverage
    (metricToDisplay: MetricToDisplay)
    (owidDataState: OwidDataState)
    (statsData: StatsData)
    : CountriesData option =

    let doAggregate (owidData: OurWorldInDataRemoteData): CountriesData option =
        match owidData with
        | Success dataPoints ->
            let dataPointsWithLocalSloveniaData =
                dataPoints
                |> List.choose (updateWithSloveniaDomesticData statsData)

            let groupedByCountries: CountriesData =
                dataPointsWithLocalSloveniaData
                |> (groupEntriesByCountries metricToDisplay)

            let averagedAndFilteredByCountries: CountriesData  =
                groupedByCountries
                |> Map.map (fun _ (countryData: CountryData) ->
                    let postProcessedEntries =
                        match metricToDisplay with
                        | NewCasesPer1M ->
                            countryData.Entries
                            |> calculateMovingAverages daysOfMovingAverage
                        | ActiveCasesPer1M ->
                            countryData.Entries |> calculateActiveCases
                        | TotalDeathsPer1M ->
                            countryData.Entries
                            |> calculateMovingAverages daysOfMovingAverage
                        | DeathsPerCases ->
                            countryData.Entries
                            |> calculateMovingAverages daysOfMovingAverage

                    let minValueFilter =
                        match metricToDisplay with
                        | NewCasesPer1M -> 0.1
                        | ActiveCasesPer1M -> 0.1
                        | TotalDeathsPer1M -> 0.1
                        | DeathsPerCases -> 0.001

                    let trimmedEntries =
                        postProcessedEntries
                        |> Array.skipWhile
                               (fun entry -> entry.Value < minValueFilter)

                    { countryData with Entries = trimmedEntries })

            Some averagedAndFilteredByCountries
        | _ -> None

    match owidDataState with
    | PreviousAndLoadingNew owidData -> doAggregate owidData
    | Current owidData -> doAggregate owidData
    | NotLoaded -> None
