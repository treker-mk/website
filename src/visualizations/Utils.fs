[<RequireQualifiedAccess>]
module Utils

open Fable.Core
open Feliz

open Types
open System

let memoize f =
    let cache = new System.Collections.Generic.Dictionary<_,_>()
    (fun x ->
        match cache.TryGetValue x with
        | true, value ->
            value
        | false, _ ->
            let value = f x
            cache.Add(x, value)
            value)

// Converts Some 0 value to None
let zeroToNone value =
    match value with
    | Some 0 -> None
    | _ -> value

let optionToInt (value: int option) =
    match value with
    | Some x -> x
    | None -> 0

[<Emit("(x => isNaN(x) ? null : x)(+$0)")>]
let nativeParseInt (input : string) : int option = jsNative

[<Emit("(x => isNaN(x) ? null : x)(+$0)")>]
let nativeParseFloat (input : string) : float option = jsNative

let parseDate (str : string) =
    try
        DateTime.Parse(str) |> Ok
    with _ ->
        sprintf "Invalid date representation: %s" str |> Error

let roundDecimals (decimals:int) (value: float) = Math.Round(value, decimals)

let roundToInt = roundDecimals 0

let roundTo1Decimal = roundDecimals 1

let roundTo3Decimals = roundDecimals 3

let formatToInt (value: float) =
    let formatted = sprintf "%.0f" value
    // A hack to replace decimal point with decimal comma.
    formatted.Replace('.', ',')

let formatTo1DecimalWithTrailingZero (value: float) =
    let formatted = sprintf "%.1f" value
    // A hack to replace decimal point with decimal comma.
    formatted.Replace('.', ',')

let formatTo3DecimalWithTrailingZero (value: float) =
    let formatted = sprintf "%.3f" value
    // A hack to replace decimal point with decimal comma.
    formatted.Replace('.', ',')

let percentageValuesWith1DecimalTrailingZeroLabelFormatter (value: float) =
    let formatted = sprintf "%.1f" value
    formatted.Replace('.', ',') + "%"

let percentageValuesLabelFormatter (value: float) =
    // A hack to replace decimal point with decimal comma.
    ((abs value).ToString() + "%").Replace('.', ',')

let calculateDoublingTime (v1 : {| Day : int ; PositiveTests : int |}) (v2 : {| Day : int ; PositiveTests : int |}) =
    let v1,  v2,  dt = float v1.PositiveTests,  float v2.PositiveTests,  float (v2.Day - v1.Day)
    if v1 = v2 then None
    else
        let value = log10 2.0 / log10 ((v2 / v1) ** (1.0 / dt))
        if value < 0.0 then None
        else Some value

let findDoublingTime (values : {| Date : DateTime ; Value : int option |} list) =
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

let classes (classTuples: seq<bool * string>) =
    classTuples
    |> Seq.filter (fun (visible, _) -> visible)
    |> Seq.map (fun (_, className) -> className)
    |> Seq.toList
    |> prop.className

let renderScaleSelector scaleType dispatch =
    let renderSelector (scaleType : ScaleType) (currentScaleType : ScaleType) (label : string) =
        let defaultProps =
            [ prop.text label
              classes [
                  (true, "chart-display-property-selector__item")
                  (scaleType = currentScaleType, "selected") ] ]
        if scaleType = currentScaleType
        then Html.div defaultProps
        else Html.div ((prop.onClick (fun _ -> dispatch scaleType)) :: defaultProps)

    let yLabel = I18N.t "charts.common.yAxis"
    let linearLabel = I18N.t "charts.common.linear"
    let logLabel = I18N.t "charts.common.log"
    Html.div [
        prop.className "chart-display-property-selector"
        prop.children [
            Html.div [
                prop.text yLabel
                prop.className "chart-display-property-selector__item"
            ]
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

let renderMaybeVisible (visible: bool) (children: ReactElement seq) =
    Html.div [
        prop.className (match visible with
                        | true -> ""
                        | false -> "invisible" )
        prop.children children
    ]

let monthNameOfDate (date : DateTime) =
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

let mixColors
    (minColorR, minColorG, minColorB)
    (maxColorR, maxColorG, maxColorB)
    mixRatio =

    let colorR =
        ((maxColorR - minColorR) |> float)
        * mixRatio + (float minColorR)
        |> round |> int
    let colorG =
        ((maxColorG - minColorG) |> float)
        * mixRatio + (float minColorG)
        |> round |> int
    let colorB =
        ((maxColorB - minColorB) |> float)
        * mixRatio + (float minColorB)
        |> round |> int

    "#" + colorR.ToString("X2")
        + colorG.ToString("X2")
        + colorB.ToString("X2")

module Dictionaries =

    type Region = {
        Key : string
        Name : string
        Population : int option }

(* SLO-spec - replaced for MK *)
    let excludedRegions = Set.ofList ["mk"]

    let regions =
        [ "mk", Some 2076255
          "va", Some 151492
          "is", Some 182785
          "jz", Some 219180
          "ji", Some 172824
          "pe", Some 226837
          "po", Some 322872
          "si", Some 166992
          "sk", Some 633273 ]
        |> List.map (fun (key, population) -> key,  { Key = key ; Name = key ; Population = population }) 
        |> Map.ofList

    type Municipality = {
        Key : string
        Name : string
        Code : string
        Population : int
    }

(* SLO-spec - replaced for MK *)
    let municipalities =
        [ "berovo", 10890, "Berovo"
          "bitola", 93095, "Bitola"
          "valandovo", 10508, "Valandovo"
          "veles", 59638, "Veles"
          "vinica", 14475, "Vinitsa"
          "gevgelija", 32005, "Gevgelija"
          "gostivar", 84654, "Gostivar"
          "debar", 19132, "Debar"
          "delcevo", 20024, "Delchevo"
          "demir_hisar", 7260, "Demir Hisar"
          "kavadarci", 39529, "Kavadartsi"
          "kicevo", 43891, "Kichevo"
          "kocani", 39159, "Kochani"
          "kratovo", 7545, "Kratovo"
          "kriva_palanka", 21524, "Kriva Palanka"
          "krusevo", 8385, "Krushevo"
          "kumanovo", 123913, "Kumanovo"
          "makedonski_brod", 5889, "Makedonski Brod"
          "negotino", 21971, "Negotino"
          "ohrid", 55147, "Ohrid"
          "pehcevo", 3983, "Pehchevo"
          "prilep", 87318, "Prilep"
          "probistip", 13417, "Probishtip"
          "radovis", 26847, "Radovish"
          "resen", 14373, "Resen"
          "sveti_nikole", 17584, "Sveti Nikole"
          "skopje", 607007, "Skopje"
          "struga", 53339, "Struga"
          "strumica", 79027, "Strumitsa"
          "tetovo", 166898, "Tetovo"
          "stip", 48286, "Shtip" ]    
        |> List.map (fun (key, population, code) -> key,  { Key = key ; Name = key ; Population = population ; Code = code })
        |> Map.ofList

    let skopjeMunicipalities =
        [ "chair", 64773, "Chair"
          "aerodrom", 72009, "Aerodrom"
          "butel", 36154, "Butel"
          "gazi_baba", 72617, "Gazi Baba"
          "centar", 45412, "Centar"
          "karposh", 59666, "Karposh"
          "saraj", 35408, "Saraj"
          "kisela_voda", 57236, "Kisela Voda"
          "gjorche_petrov", 41634, "Gjorche Petrov"
          "studenichani", 17246, "Studenichani"
          "shuto_orizari", 22017, "Shuto Orizari"
          "arachinovo", 11597, "Arachinovo"
          "ilinden", 15894, "Ilinden"
          "petrovets", 8255, "Petrovets"
          "chucher-sandevo", 8493, "Chucher - Sandevo"
          "sopishte", 5656, "Sopishte"
          "zelenikovo", 4077, "Zelenikovo" ]    
        |> List.map (fun (key, population, code) -> key,  { Municipality.Key = key ; Municipality.Name = key ; Municipality.Population = population ; Municipality.Code = code })
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
