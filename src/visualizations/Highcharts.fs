module Highcharts

open System
open Fable.Core
open Fable.React
open Browser

open Types

[<Import("renderChart", from="./_highcharts")>]
let chart: obj -> ReactElement = jsNative

[<Import("renderChartFromWindow", from="./_highcharts")>]
let chartFromWindow: obj -> ReactElement = jsNative

[<Import("renderMap", from="./_highcharts")>]
let map: obj -> ReactElement = jsNative

[<Import("sparklineChart", from="./_highcharts")>]
let sparklineChart (documentElementId : string, options : obj) : unit = jsNative

[<AutoOpen>]
module Helpers =
    // Plain-Old-Javascript-Object (i.e. box)
    let inline pojo o = JsInterop.toPlainJsObj o

    // plain old javascript object
    [<Emit """Array.prototype.slice.call($0)""">]
    let poja (a: 'T[]) : obj = jsNative

    type JsTimestamp = float

    [<Emit("$0.getTime()")>]
    let jsTime (x: DateTime): JsTimestamp = jsNative

    let jsNoon : JsTimestamp = 43200000.0
    let jsTime12h = jsTime >> ( + ) jsNoon
    [<Emit("(new Date($0.getFullYear(), $0.getMonth(), $0.getDate())).getTime()")>]
    let jsTimeMidnight (x: DateTime): JsTimestamp = jsNative

    /// Given two dates it calculates the middle point between the midnight for the first date and end of day for the second date
    let jsDatesMiddle (a: DateTime) (b: DateTime): JsTimestamp = ( + ) (0.5 * jsTimeMidnight a) (0.5 * jsTimeMidnight b) + 43200000.0

type DashStyle =
    | Solid
    | ShortDash
    | ShortDot
    | ShortDashDot
    | ShortDashDotDot
    | Dot
    | Dash
    | LongDash
    | DashDot
    | LongDashDot
    | LongDashDotDot
  with
    static member toString = function
        | Solid -> "Solid"
        | ShortDash -> "ShortDash"
        | ShortDot -> "ShortDot"
        | ShortDashDot -> "ShortDashDot"
        | ShortDashDotDot -> "ShortDashDotDot"
        | Dot -> "Dot"
        | Dash -> "Dash"
        | LongDash -> "LongDash"
        | DashDot -> "DashDot"
        | LongDashDot -> "LongDashDot"
        | LongDashDotDot -> "LongDashDotDot"


let shadedWeekendPlotBands =
    let saturday = DateTime(2020,02,22)
    let nWeeks = (DateTime.Today-saturday).TotalDays / 7.0 |> int
    let oneDay = 86400000.0
    let origin = jsTime saturday // - oneDay / 2.0
    [|
        for i in 0..nWeeks+2 do
            //yield {| value=origin + oneDay * 7.0 * float i; label=None; color=Some "rgba(0,0,0,0.05)"; width=Some 5 |}
            //yield {| value=origin + oneDay * 7.0 * float (i+1); label=None; color=Some "rgba(0,0,0,0.05)"; width=Some 5 |}
            yield
                {|
                    ``from`` = origin + oneDay * 7.0 * float i
                    ``to`` = origin + oneDay * 7.0 * float i + oneDay * 2.0
                    color = "rgb(0,0,0,0.025)"
                    label = None
                |}
    |]

// if set to true:
// - MunicipalitiesChart will showDoublesInXday
let showDoublingTimeFeatures =
    true

// if set to true:
// - SpreadChart will show exponential growth pages
let showExpGrowthFeatures =
    true

let addContainmentMeasuresFlags
    (startDate: JsTimestamp)
    (endDate: JsTimestamp option) =
    let events = [|
    // day, mo, color,    i18n
        28,  2, "#FFFFFF", "gatheringsMass"
        16,  3, "#FFe6e6", "bordersClosure"
        17,  3, "#FFFFFF", "debarQuarantine"
        19, 3, "#FFe6e6", "gatherings5"
        21, 3, "#FFFFFF", "21hLockdown"
        22, 4, "#FFe6e6", "masksOn"
        12, 5, "#FFe6e6", "lessMeasures"
        27, 5, "#ebfaeb", "kafanasOpen"
        23, 6, "#e6f0ff", "bordersOopen"
        9, 9, "#ebfaeb", "kindergartensOpen"   
        1, 10, "#ebfaeb", "ucilistaOpen"
        4, 11, "#FFe6e6", "kafanas21h"
        18, 12, "#FFe6e6", "kafanas18h"
    |]
    {|
        ``type`` = "flags"
        shape = "flag"
        showInLegend = false
        color = "#444"
        data =
            events |> Array.choose (fun (d,m,color,i18n) ->
                let ts = DateTime(2020,m,d) |> jsTime
                let showMeasure =
                    match startDate, endDate with
                    | startDate, None -> ts >= startDate
                    | startDate, Some endDate ->
                        ts >= startDate && ts <= endDate

                let title = "mk.cm." + i18n + ".title"
                let text = "mk.cm." + i18n + ".description"
                if showMeasure then
                    Some {| x=ts; fillColor=color; title=I18N.t title; text=I18N.t text |}
                else None
            )
     |}

(* Trigger document event for iframe resizing *)
let onLoadEvent (name : String) =
    let res (e : Event) =
        let event = document.createEvent("Event")
        event.initEvent("chartLoaded", true, true)
        document.dispatchEvent(event)
    res

let optionsWithOnLoadEvent (className : string) =
    {| chart = pojo
        {| events = pojo
            {| load = onLoadEvent(className) |}
        |}
    |}

let parseDate (value: String) =
    match I18N.t "charts.common.numDateFormat" with
    | "%m/%d/%Y" -> // EN, ME
        let date = value.Replace(" ", "").Split('/')
        DateTime
            .Parse(date.[2] + "-" + date.[0] + "-" + date.[1])
            .Subtract(DateTime(1970,1,1))
            .TotalMilliseconds
    | "%d/%m/%Y" -> // IT
        let date = value.Replace(" ", "").Split('/')
        DateTime
            .Parse(date.[2] + "-" + date.[1] + "-" + date.[0])
            .Subtract(DateTime(1970,1,1))
            .TotalMilliseconds
    | _ -> // DE, HR, MK, SL, SQ
        let date = value.Replace(" ", "").Split('.')
        DateTime
            .Parse(date.[2] + "-" + date.[1] + "-" + date.[0])
            .Subtract(DateTime(1970,1,1))
            .TotalMilliseconds

let configureRangeSelector selectedRangeSelectionButtonIndex buttons =
           pojo {|
                enabled = true
                allButtonsEnabled = true
                selected = selectedRangeSelectionButtonIndex
                inputDateFormat = I18N.t "charts.common.numDateFormat"
                inputEditDateFormat = I18N.t "charts.common.numDateFormat"
                inputDateParser = parseDate
                x = 0
                inputBoxBorderColor = "#ced4da"
                buttonTheme = pojo {| r = 6; states = pojo {| select = pojo {| fill = "#ffd922" |} |} |}
                buttons = buttons
            |}

let credictsOptions =
    {| enabled = true
       text = sprintf "%s: %s, %s"
            (I18N.t "charts.common.dataSource")
            (I18N.tOptions ("charts.common.dsNIJZ") {| context = localStorage.getItem ("contextCountry") |})
            (I18N.tOptions ("charts.common.dsMZ") {| context = localStorage.getItem ("contextCountry") |})
    // SLO-spec   href = "https://www.nijz.si/sl/dnevno-spremljanje-okuzb-s-sars-cov-2-covid-19"
       href = "http://www.iph.mk"
    |} |> pojo


let basicChartOptions
    (scaleType:ScaleType)
    (className:string)
    (selectedRangeSelectionButtonIndex: int)
    (rangeSelectorButtonClickHandler: int -> (Event -> bool))
    =
    {|
        chart = pojo
            {|
                animation = false
                ``type`` = "line"
                zoomType = "x"
                className = className
                events = pojo {| load = onLoadEvent(className) |}
            |}
        title = pojo {| text = None |}
        xAxis = [|
            {|
                index=0; crosshair=true; ``type``="datetime"
                gridLineWidth=1 //; isX=true
                gridZIndex = -1
                tickInterval=86400000
                //labels = pojo {| align = "center"; y = 30; reserveSpace = false |} // style = pojo {| marginBottom = "-30px" |}
                labels = pojo {| align = "center"; y = 30; reserveSpace = true; distance = -20; |} // style = pojo {| marginBottom = "-30px" |}
                //labels = {| rotation= -45 |}
                plotLines=[|
(* SLO-spec 
                    {| value=jsTime <| DateTime(2020,3,13); label=Some {| text=I18N.t "phase.2.description"; rotation=270; align="right"; x=12 |} |}
                    {| value=jsTime <| DateTime(2020,3,20); label=Some {| text=I18N.t "phase.3.description"; rotation=270; align="right"; x=12 |} |}
                    {| value=jsTime <| DateTime(2020,4,8);  label=Some {| text=I18N.t "phase.4.description"; rotation=270; align="right"; x=12 |} |}
                    {| value=jsTime <| DateTime(2020,4,15); label=Some {| text=I18N.t "phase.5.description"; rotation=270; align="right"; x=12 |} |}
                    {| value=jsTime <| DateTime(2020,4,21); label=Some {| text=I18N.t "phase.6.description"; rotation=270; align="right"; x=12 |} |}
                    {| value=jsTime <| DateTime(2020,5,15); label=Some {| text=I18N.t "phase.7.description"; rotation=270; align="right"; x=12 |} |}
*)
                |]

                plotBands=[|
                    // SLO-spec - TODO: this is just placeholder/hack until you have MK phases 
                    {| ``from``=jsTime <| DateTime(2020,2,29);
                       ``to``=jsTime <| DateTime(2020,3,13);
                       color="transparent"
                       label=Some {| align="center"; text="" |}
                    |}
(* SLO-spec 
                    {| ``from``=jsTime <| DateTime(2020,2,29);
                       ``to``=jsTime <| DateTime(2020,3,13);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.1.title" |}
                    |}
                    {| ``from``=jsTime <| DateTime(2020,3,13);
                       ``to``=jsTime <| DateTime(2020,3,20);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.2.title" |}
                    |}
                    {| ``from``=jsTime <| DateTime(2020,3,20);
                       ``to``=jsTime <| DateTime(2020,4,8);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.3.title" |}
                    |}
                    {| ``from``=jsTime <| DateTime(2020,4,8);
                       ``to``=jsTime <| DateTime(2020,4,15);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.4.title" |}
                    |}
                    {| ``from``=jsTime <| DateTime(2020,4,15);
                       ``to``=jsTime <| DateTime(2020,4,21);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.5.title" |}
                    |}
                    {| ``from``=jsTime <| DateTime(2020,4,21);
                       ``to``=jsTime <| DateTime(2020,5,15);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.6.title" |}
                    |}
                    {| ``from``=jsTime <| DateTime(2020,5,15);
                       ``to``=jsTime <| DateTime(2020,9,10);
                       color="transparent"
                       label=Some {| align="center"; text=I18N.t "phase.7.title" |}
                    |}
*)
                    yield! shadedWeekendPlotBands
                |]
                // https://api.highcharts.com/highcharts/xAxis.dateTimeLabelFormats
                dateTimeLabelFormats = pojo
                    {|
                        week = I18N.t "charts.common.shortDateFormat"
                        day = I18N.t "charts.common.shortDateFormat"
                    |}
            |}
        |]
        yAxis = [|
            {|
                index = 0
                ``type`` = if scaleType=Linear then "linear" else "logarithmic"
                min = if scaleType=Linear then None else Some 1
                max = None
                //floor = if scaleType=Linear then None else Some 1.0
                opposite = true // right side
                maxPadding = if scaleType = Linear then None else Some 0.25
                title = {| text = null |} // "oseb" |}
                showFirstLabel = None
                tickInterval = if scaleType=Linear then None else Some 0.4
                gridZIndex = -1
                plotLines = [| {| value = 0; color = "black" |} |]
                crosshair = true
            |}
        |]

        tooltip = pojo
            {|
                shared = true
                split = false
                xDateFormat = "<b>" + I18N.t "charts.common.dateFormat" + "</b>"
            |}

        legend =
            {|
                enabled = false
                align = "left"
                verticalAlign = "top"
                borderColor = "#ddd"
                borderWidth = 1
                //labelFormatter = string //fun series -> series.name
                layout = "vertical"
                //backgroundColor = None :> string option
            |}

        navigator = pojo {| enabled = false |}
        scrollbar = pojo {| enabled = false |}
        rangeSelector = configureRangeSelector selectedRangeSelectionButtonIndex [|
                        {|
                            ``type`` = "month"
                            count = 2
                            text = I18N.tOptions "charts.common.x_months" {| count = 2 |}
                            events = pojo {| click = rangeSelectorButtonClickHandler 0 |}
                        |}
                        {|
                            ``type`` = "month"
                            count = 3
                            text = I18N.tOptions "charts.common.x_months" {| count = 3 |}
                            events = pojo {| click = rangeSelectorButtonClickHandler 1 |}
                        |}
                        {|
                            ``type`` = "all"
                            count = 1
                            text = I18N.t "charts.common.all"
                            events = pojo {| click = rangeSelectorButtonClickHandler 2 |}
                        |}
                    |]

        responsive = pojo
            {|
                rules =
                    [| {|
                        condition = {| maxWidth = 768 |}
                        chartOptions =
                            {|
                                yAxis = [| {| labels = pojo {| enabled = false |} |} |]
                            |}
                    |} |]
            |}

        plotOptions = pojo
            {|
                line = pojo
                    {|
                        //dataLabels = pojo {| enabled = true |}
                        marker = pojo {| symbol = "circle"; radius = 3 |}
                        //enableMouseTracking = false
                    |}
            |}

        credits = credictsOptions
    |}
