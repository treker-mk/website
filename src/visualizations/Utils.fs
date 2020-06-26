[<RequireQualifiedAccess>]
module Utils

open Feliz

open Types
open System

/// <summary>
/// Converts Some 0 value to None.
/// </summary>
let zeroToNone value =
    match value with
    | Some 0 -> None
    | _ -> value

let roundTo1Decimal (value: float) = System.Math.Round(value, 1)
let roundTo3Decimals (value: float) = System.Math.Round(value, 3)

let formatTo1DecimalWithTrailingZero (value: float) =
    let formatted =
        sprintf "%.1f" value
    // A hack to replace decimal point with decimal comma.
    formatted.Replace('.', ',')

let formatTo3DecimalWithTrailingZero (value: float) =
    let formatted =
        sprintf "%.3f" value
    // A hack to replace decimal point with decimal comma.
    formatted.Replace('.', ',')

let calculateDoublingTime (v1 : {| Day : int ; PositiveTests : int |}) (v2 : {| Day : int ; PositiveTests : int |}) =
    let v1,  v2,  dt = float v1.PositiveTests,  float v2.PositiveTests,  float (v2.Day - v1.Day)
    if v1 = v2 then None
    else
        let value = log10 2.0 / log10 ((v2 / v1) ** (1.0 / dt))
        if value < 0.0 then None
        else Some value

let findDoublingTime (values : {| Date : System.DateTime ; Value : int option |} list) =
    let reversedValues =
        values
        |> List.choose (fun dp ->
            match dp.Value with
            | None -> None
            | Some value -> Some {| Date = dp.Date ; Value = value |}
        )
        |> List.rev

    match reversedValues with
    | head :: tail ->
        match tail |> List.tryFind (fun dp ->
            float head.Value / 2. >= float dp.Value) with
        | None -> None
        | Some halfValue -> (head.Date - halfValue.Date).TotalDays |> Some
    | _ -> None

let renderScaleSelector scaleType dispatch =
    let renderSelector (scaleType : ScaleType) (currentScaleType : ScaleType) (label : string) =
        let defaultProps =
            [ prop.text label
              prop.className [
                  true,  "chart-display-property-selector__item"
                  scaleType = currentScaleType,  "selected" ] ]
        if scaleType = currentScaleType
        then Html.div defaultProps
        else Html.div ((prop.onClick (fun _ -> dispatch scaleType)) :: defaultProps)

    let yLabel = I18N.t "charts.common.yAxis"
    let linearLabel = I18N.t "charts.common.linear"
    let logLabel = I18N.t "charts.common.log"
    Html.div [
        prop.className "chart-display-property-selector"
        prop.children [
            Html.text yLabel
            renderSelector Linear scaleType linearLabel
            renderSelector Logarithmic scaleType logLabel
        ]
    ]

let renderChartTopControls (children: ReactElement seq) =
    Html.div [
        prop.className "chart-display-properties"
        prop.children children
    ]

let renderChartTopControlRight (topControl: ReactElement) =
    Html.div [
        prop.className "chart-display-properties"
        prop.style [ style.justifyContent.flexEnd ]
        prop.children [ topControl ]
    ]

let renderLoading =
    let loadingLabel = I18N.t "charts.common.loading"
    Html.div [
        prop.className "loader"
        prop.text loadingLabel 
    ]

let renderErrorLoading (error : string) =
    Html.text error

let monthNameOfdate (date : System.DateTime) =
    match date.Month with
    | 1 -> I18N.t "month.0"
    | 2 -> I18N.t "month.1"
    | 3 -> I18N.t "month.2"
    | 4 -> I18N.t "month.3"
    | 5 -> I18N.t "month.4"
    | 6 -> I18N.t "month.5"
    | 7 -> I18N.t "month.6"
    | 8 -> I18N.t "month.7"
    | 9 -> I18N.t "month.8"
    | 10 -> I18N.t "month.9"
    | 11 -> I18N.t "month.10"
    | 12 -> I18N.t "month.11"
    | _ -> failwith "Invalid month"

let transliterateCSZ (str : string) =
    str
        .Replace("Č",  "C")
        .Replace("Š",  "S")
        .Replace("Ž",  "Z")
        .Replace("č",  "c")
        .Replace("š",  "s")
        .Replace("ž",  "z")

module Dictionaries =

    type Region = {
        Key : string
        Name : string
        Population : int option }

(* SLO-spec - replaced for MK *)
    let excludedRegions = Set.ofList ["mk"]

    let regions =
        [ "mk", Some 2069009
          "va", Some 153205
          "is", Some 186795
          "jz", Some 220064
          "ji", Some 173633
          "pe", Some 231490
          "po", Some 319586
          "si", Some 166589
          "sk", Some 617647 ]
        |> List.map (fun (key, population) -> key,  { Key = key ; Name = key ; Population = population }) 
        |> Map.ofList

    type Municipality = {
        Key : string
        Name : string
        Population : int
        Code : string }

(* SLO-spec - replaced for MK *)
    let municipalities =
        [ "berovo", 12987, "Berovo"
          "bitola", 66256, "Bitola"
          "valandovo", 11858, "Valandovo"
          "veles", 66256, "Veles"
          "vinica", 19481, "Vinitsa"
          "gevgelija", 34364, "Gevgelija"
          "gostivar", 119792, "Gostivar"
          "debar", 27759, "Debar"
          "delcevo", 23810, "Delchevo"
          "demir_hisar", 8403, "Demir Hisar"
          "kavadarci", 42969, "Kavadartsi"
          "kicevo", 57143, "Kichevo"
          "kocani", 47988, "Kochani"
          "kratovo", 9569, "Kratovo"
          "kriva_palanka", 23904, "Kriva Palanka"
          "krusevo", 9524, "Krushevo"
          "kumanovo", 142685, "Kumanovo"
          "makedonski_brod", 11194, "Makedonski Brod"
          "negotino", 23474, "Negotino"
          "ohrid", 56109, "Ohrid"
          "pehcevo", 4995, "Pehchevo"
          "prilep", 95314, "Prilep"
          "probistip", 15294, "Probishtip"
          "radovis", 32520, "Radovish"
          "resen", 16353, "Resen"
          "sveti_nikole", 20507, "Sveti Nikole"
          "skopje", 617647, "Skopje"
          "struga", 67860, "Struga"
          "strumica", 94891, "Strumitsa"
          "tetovo", 199794, "Tetovo"
          "stip", 52672, "Shtip" ]    
        |> List.map (fun (key, population, code) -> key,  { Key = key ; Name = key ; Population = population ; Code = code })
        |> Map.ofList

module AgePopulationStats =
    type AgeGroupId = string

    type AgeGroupPopulationStats = {
        Key: AgeGroupId
        Male: int
        Female: int
    }

    let agePopulationStats =
        [
            "0-4", 53183, 50328
            "5-14", 106600, 100566
            "15-24", 100391, 93739
            "25-34", 133471, 122333
            "35-44", 162436, 146922
            "45-54", 153735, 146868
            "55-64", 147957, 147089
            "65-74", 101173, 113253
            "75-84", 54460, 81981
            "85+", 13635, 36760
        ]
        |> List.map (fun (ageGroupId,  male,  female) ->
            ageGroupId, { Key = ageGroupId;  Male = male;  Female = female })
        |> Map.ofList

    let parseAgeGroupId (ageGroupId: AgeGroupId): AgeGroupKey =
        if ageGroupId.Contains('-') then
            let i = ageGroupId.IndexOf('-')
            let fromAge = Int32.Parse(ageGroupId.Substring(0, i))
            let toAge = Int32.Parse(ageGroupId.Substring(i+1))
            { AgeFrom = Some fromAge; AgeTo =  Some toAge }
        else if ageGroupId.Contains('+') then
            let i = ageGroupId.IndexOf('+')
            let fromAge = Int32.Parse(ageGroupId.Substring(0, i-1))
            { AgeFrom = Some fromAge; AgeTo = None }
        else
            sprintf "Invalid age group ID: %s" ageGroupId
            |> ArgumentException |> raise

    let toAgeGroupId (groupKey: AgeGroupKey): AgeGroupId =
        match groupKey.AgeFrom, groupKey.AgeTo with
        | Some fromValue, Some toValue -> sprintf "%d-%d" fromValue toValue
        | Some fromValue, None -> sprintf "%d+" fromValue
        | _ -> sprintf "Invalid age group key (%A)" groupKey
                |> ArgumentException |> raise

    let populationStatsForAgeGroup (groupKey: AgeGroupKey)
        : AgeGroupPopulationStats =
        let ageGroupId = toAgeGroupId groupKey

        if agePopulationStats.ContainsKey ageGroupId then
            agePopulationStats.[ageGroupId]
        else
            sprintf "Age group '%s' does not exist." ageGroupId
            |> ArgumentException |> raise
