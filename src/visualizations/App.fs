module App

open Browser
open Elmish
open Feliz

open Types
open CountriesChartViz.Analysis
open I18N

let init (query: obj) (visualization: string option) (page: string) (apiEndpoint: string)=
    let renderingMode =
        match visualization with
        | None -> Normal
        | Some viz ->
            match viz with
            | "Map" -> Some Map
            | "RegionMap" -> Some RegionMap
            | "SkopjeMunMap" -> Some SkopjeMunMap
            | "EuropeMap" -> Some EuropeMap
            | "WorldMap" -> Some WorldMap
            | "MetricsComparison" -> Some MetricsComparison
            | "DailyComparison" -> Some DailyComparison
            | "Patients" -> Some Patients
            | "CarePatients" -> Some CarePatients
            | "Ratios" -> Some Ratios
            | "Tests" -> Some Tests
            | "Cases" -> Some Cases
            | "Spread" -> Some Spread
            | "Regions" -> Some Regions
            | "Regions100k" -> Some Regions100k
            | "Weekly" -> Some Sources
            | "HcCases" -> Some HcCases
            | "Municipalities" -> Some Municipalities
            | "SkopjeMunicipalities" -> Some SkopjeMunicipalities
            | "AgeGroups" -> Some AgeGroups
            | "AgeGroupsTimeline" -> Some AgeGroupsTimeline
            | "HCenters" -> Some HCenters
            | "Hospitals" -> Some Hospitals
            | "Infections" -> Some Infections
            | "CountriesCasesPer1M" -> Some CountriesCasesPer1M
            | "CountriesActiveCasesPer1M" -> Some CountriesActiveCasesPer1M
            | "CountriesDeathsPer1M" -> Some CountriesDeathsPer1M
            | "PhaseDiagram" -> Some PhaseDiagram
            | "MetricsCorrelation" -> Some MetricsCorrelation
            | _ -> None
            |> Embedded

    let initialState =
        {
          ApiEndpoint = apiEndpoint
          Page = page
          Query = query
          StatsData = NotAsked
          WeeklyStatsData = NotAsked
          RegionsData = NotAsked
          SkopjeMunicipalitiesData = NotAsked
          RenderingMode = renderingMode }

    // Request data loading based on the page we are on
    let cmd =
        match page with
        | "local" ->
            Cmd.batch
                [ Cmd.ofMsg StatsDataRequested
                  Cmd.ofMsg WeeklyStatsDataRequested
                  Cmd.ofMsg RegionsDataRequest
                  Cmd.ofMsg SkopjeMunicipalitiesDataRequest ]
        | "world" ->
            Cmd.batch
                [ Cmd.ofMsg StatsDataRequested
                  Cmd.ofMsg WeeklyStatsDataRequested ]
        | _ ->
            Cmd.batch
                [ Cmd.ofMsg StatsDataRequested
                  Cmd.ofMsg WeeklyStatsDataRequested
                  Cmd.ofMsg RegionsDataRequest
                  Cmd.ofMsg SkopjeMunicipalitiesDataRequest ]

    initialState, cmd

let update (msg: Msg) (state: State) =
    match msg with
    | StatsDataRequested ->
        match state.StatsData with
        | Loading -> state, Cmd.none
        | _ -> { state with StatsData = Loading }, Cmd.OfAsync.result Data.Stats.load
    | StatsDataLoaded data -> { state with StatsData = data }, Cmd.none
    | WeeklyStatsDataRequested ->
        match state.WeeklyStatsData with
        | Loading -> state, Cmd.none
        | _ -> { state with WeeklyStatsData = Loading }, Cmd.OfAsync.result Data.WeeklyStats.load
    | WeeklyStatsDataLoaded data -> { state with WeeklyStatsData = data }, Cmd.none
    | RegionsDataRequest ->
        match state.RegionsData with
        | Loading -> state, Cmd.none
        | _ -> { state with RegionsData = Loading }, Cmd.OfAsync.result (Data.Regions.load state.ApiEndpoint)
    | RegionsDataLoaded data -> { state with RegionsData = data }, Cmd.none
    | SkopjeMunicipalitiesDataRequest ->
        match state.SkopjeMunicipalitiesData with
        | Loading -> state, Cmd.none
        | _ -> { state with SkopjeMunicipalitiesData = Loading }, Cmd.OfAsync.result (Data.Regions.loadSkMun state.ApiEndpoint)
    | SkopjeMunicipalitiesDataLoaded data -> { state with SkopjeMunicipalitiesData = data }, Cmd.none

open Elmish.React

let render (state: State) (_: Msg -> unit) =
    let hospitals =
          { VisualizationType = Hospitals
            ClassName = "hospitals-chart"
            ChartTextsGroup = "hospitals"
            Explicit = true
            Renderer = fun _ -> lazyView HospitalsChart.hospitalsChart () }

    let metricsComparison =
          { VisualizationType = MetricsComparison
            ClassName = "metrics-comparison-chart"
            ChartTextsGroup = "metricsComparison"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView MetricsComparisonChart.metricsComparisonChart {| data = data |} }

    let dailyComparison =
          { VisualizationType = DailyComparison
            ClassName = "daily-comparison-chart"
            ChartTextsGroup = "dailyComparison"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView DailyComparisonChart.dailyComparisonChart {| data = data |} }

    let spread =
          { VisualizationType = Spread
            ClassName = "spread-chart"
            ChartTextsGroup = "spread"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView SpreadChart.spreadChart {| data = data |} }

    let map =
          { VisualizationType = Map
            ClassName = "map-chart"
            ChartTextsGroup = "map"
            Explicit = false
            Renderer =
                fun state ->
                    match state.RegionsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView Map.mapChart {| mapToDisplay = Map.MapToDisplay.Municipality; data = data |} }

    let regionMap =
          { VisualizationType = RegionMap
            ClassName = "rmap-chart"
            ChartTextsGroup = "rmap"
            Explicit = false
            Renderer =
                fun state ->
                    match state.RegionsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView Map.mapChart {| mapToDisplay = Map.MapToDisplay.Region; data = data |} }

    let skopjeMunMap =
          { VisualizationType = SkopjeMunMap
            ClassName = "sk-map-chart"
            ChartTextsGroup = "sk-map"
            Explicit = false
            Renderer =
                fun state ->
                    match state.SkopjeMunicipalitiesData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView Map.mapChart {| mapToDisplay = Map.MapToDisplay.SkopjeMunicipality; data = data |} }


    let municipalities =
          { VisualizationType = Municipalities
            ClassName = "municipalities-chart"
            ChartTextsGroup = "municipalities"
            Explicit = false
            Renderer =
                fun state ->
                    match state.RegionsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView MunicipalitiesChart.municipalitiesChart {| query = state.Query; dataToDisplay = MunicipalitiesChart.DataToDisplay.Municipality; data = data |} }
    
    let skopjeMunicipalities =
          { VisualizationType = SkopjeMunicipalities
            ClassName = "sk-municipalities-chart"
            ChartTextsGroup = "sk-municipalities"
            Explicit = false
            Renderer =
                fun state ->
                    match state.SkopjeMunicipalitiesData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView MunicipalitiesChart.municipalitiesChart {| query = state.Query; dataToDisplay = MunicipalitiesChart.DataToDisplay.SkopjeMunicipality; data = data |} }

    let europeMap =
          { VisualizationType = EuropeMap
            ClassName = "europe-chart"
            ChartTextsGroup = "europe"
            Explicit = false
            Renderer =
                fun state ->
                    match state.WeeklyStatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView EuropeMap.mapChart {| mapToDisplay = EuropeMap.MapToDisplay.Europe; data = data |} }

    let worldMap =
          { VisualizationType = WorldMap
            ClassName = "world-chart"
            ChartTextsGroup = "world"
            Explicit = false
            Renderer =
                fun state ->
                    match state.WeeklyStatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView EuropeMap.mapChart {| mapToDisplay = EuropeMap.MapToDisplay.World; data = data |} }

    let ageGroupsTimeline =
          { VisualizationType = AgeGroupsTimeline
            ClassName = "age-groups-trends-chart"
            ChartTextsGroup = "ageGroupsTimeline"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView AgeGroupsTimelineViz.Rendering.renderChart
                            {| data = data |} }

    let tests =
          { VisualizationType = Tests
            ClassName = "tests-chart"
            ChartTextsGroup = "tests"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView TestsChart.testsChart {| data = data |} }

    let hCenters =
          { VisualizationType = HCenters
            ClassName = "hcenters-chart"
            ChartTextsGroup = "hCenters"
            Explicit = false
            Renderer = fun _ -> lazyView HCentersChart.hCentersChart state.ApiEndpoint }

    let infections =
          { VisualizationType = Infections
            ClassName = "infections-chart"
            ChartTextsGroup = "infections"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView InfectionsChart.infectionsChart {| data = data |} }

    let cases =
          { VisualizationType = Cases
            ClassName = "cases-chart"
            ChartTextsGroup = "cases"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView CasesChart.casesChart {| data = data |} }

    let patients =
          { VisualizationType = Patients
            ClassName = "patients-chart"
            ChartTextsGroup = "patients"
            Explicit = false
            Renderer = fun _ -> lazyView PatientsChart.patientsChart {| hTypeToDisplay = PatientsChart.HospitalType.CovidHospitals |} }

    let patientsCare =
          { VisualizationType = CarePatients
            ClassName = "care-patients-chart"
            ChartTextsGroup = "carePatients"
            Explicit = false
            Renderer = fun _ -> lazyView PatientsChart.patientsChart {| hTypeToDisplay = PatientsChart.HospitalType.CareHospitals |} }

    let ratios =
          { VisualizationType = Ratios
            ClassName = "ratios-chart"
            ChartTextsGroup = "ratios"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView RatiosChart.ratiosChart {| data = data |} }

    let ageGroups =
          { VisualizationType = AgeGroups
            ClassName = "age-groups-chart"
            ChartTextsGroup = "ageGroups"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView AgeGroupsChart.renderChart {| data = data |} }

    let regionsAbs =
          { VisualizationType = Regions
            ClassName = "regions-chart"
            ChartTextsGroup = "regions"
            Explicit = false
            Renderer =
                fun state ->
                    match state.RegionsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        let config: RegionsChart.RegionsChartConfig =
                            { RelativeTo = RegionsChart.MetricRelativeTo.Absolute
                              ChartTextsGroup = "regions"
                            }
                        let props = {| data = data |}
                        lazyView (RegionsChart.renderChart config) props
            }

    let regions100k =
          { VisualizationType = Regions100k
            ClassName = "regions-chart-100k"
            ChartTextsGroup = "regions100k"
            Explicit = false
            Renderer =
                fun state ->
                    match state.RegionsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        let config: RegionsChart.RegionsChartConfig =
                            { RelativeTo = RegionsChart.MetricRelativeTo.Pop100k
                              ChartTextsGroup = "regions100k"
                            }
                        let props = {| data = data |}
                        lazyView (RegionsChart.renderChart config) props
         }

    let sources =
          { VisualizationType = Sources
            ClassName = "sources-chart"
            ChartTextsGroup = "sources"
            Explicit = false
            Renderer =
                fun state ->
                    match state.WeeklyStatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView SourcesChart.sourcesChart {| data = data |} }

    let hcCases =
          { VisualizationType = HcCases
            ClassName = "hc-cases-chart"
            ChartTextsGroup = "hcCases"
            Explicit = false
            Renderer =
                fun state ->
                    match state.WeeklyStatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView HcCasesChart.hcCasesChart {| data = data |} }

    let countriesCasesPer1M =
          { VisualizationType = CountriesCasesPer1M
            ClassName = "countries-cases-chart"
            ChartTextsGroup = "countriesNewCasesPer1M"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView CountriesChartViz.Rendering.renderChart
                            { StatsData = data
                              MetricToDisplay = NewCasesPer1M
                              ChartTextsGroup = "countriesNewCasesPer1M"
                              DataSource = "dsOWD_NIJZ"
                            }
          }

    let countriesActiveCasesPer1M =
          { VisualizationType = CountriesActiveCasesPer1M
            ClassName = "countries-active-chart"
            ChartTextsGroup = "countriesActiveCasesPer1M"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView CountriesChartViz.Rendering.renderChart
                            { StatsData = data
                              MetricToDisplay = ActiveCasesPer1M
                              ChartTextsGroup = "countriesActiveCasesPer1M"
                              DataSource = "dsOWD_NIJZ"
                            }
          }

    let countriesDeathsPer1M =
          { VisualizationType = CountriesDeathsPer1M
            ClassName = "countries-deaths-chart"
            ChartTextsGroup = "countriesTotalDeathsPer1M"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView CountriesChartViz.Rendering.renderChart
                            { StatsData = data
                              MetricToDisplay = TotalDeathsPer1M
                              ChartTextsGroup = "countriesTotalDeathsPer1M"
                              DataSource = "dsOWD_MZ"
                            }
          }

    let countriesDeathsPerCases =
          { VisualizationType = CountriesDeathsPer1M
            ClassName = "countries-deaths-per-cases"
            ChartTextsGroup = "countriesDeathsPerCases"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView CountriesChartViz.Rendering.renderChart
                            { StatsData = data
                              MetricToDisplay = DeathsPerCases
                              ChartTextsGroup = "countriesDeathsPerCases"
                              DataSource = "dsOWD_MZ"
                            }
          }

    let phaseDiagram =
          { VisualizationType = PhaseDiagram
            ClassName = "phase-diagram-chart"
            ChartTextsGroup = "phaseDiagram"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data -> lazyView PhaseDiagram.Chart.chart {| data = data |} }

    let metricsCorrelation =
          { VisualizationType = MetricsCorrelation
            ClassName = "metrics-correlation-chart"
            ChartTextsGroup = "metricsCorrelation"
            Explicit = false
            Renderer =
                fun state ->
                    match state.StatsData with
                    | NotAsked -> Html.none
                    | Loading -> Utils.renderLoading
                    | Failure error -> Utils.renderErrorLoading error
                    | Success data ->
                        lazyView MetricsCorrelationViz.Rendering.renderChart
                            {| data = data |} }

    let localVisualizations =
        [ metricsComparison; dailyComparison; spread; map; municipalities
          europeMap; tests; infections;
          cases; patients; metricsCorrelation;
          skopjeMunMap; // skopjeMunicipalities; Venko booljash
        ]

    let worldVisualizations =
        [ worldMap; countriesActiveCasesPer1M
          countriesCasesPer1M; countriesDeathsPerCases; countriesDeathsPer1M ]

    let allVisualizations =
        [ hospitals; metricsComparison; spread; dailyComparison; map
          municipalities; sources; metricsCorrelation
          europeMap; worldMap; ageGroupsTimeline; tests; hCenters; infections
          cases; patients; patientsCare; ratios; ageGroups; regionMap; regionsAbs
          regions100k; hcCases; sources
          countriesCasesPer1M
          countriesActiveCasesPer1M
          countriesDeathsPer1M
          phaseDiagram
          skopjeMunMap; skopjeMunicipalities;
        ]

    let embedded, visualizations =
        match state.Page, state.RenderingMode with
        | ("local", Normal) ->
            false,
            localVisualizations
            |> List.filter (fun viz -> not viz.Explicit)
        | ("world", Normal) ->
            false,
            worldVisualizations
            |> List.filter (fun viz -> not viz.Explicit)
        | (_, Embedded visualizationType) ->
            match visualizationType with
            | None -> true, []
            | Some visualizationType ->
                true,
                allVisualizations
                |> List.filter
                       (fun viz -> viz.VisualizationType = visualizationType)
        | _ -> invalidOp "BUG: this should never happen."

    let brandLink =
        match state.RenderingMode with
        | Normal -> Html.none
        | Embedded _ ->
            Html.a
                [ prop.className "brand-link"
                  prop.target "_blank"
                  prop.href "https://covid-19.treker.mk/"
                  prop.text (t "meta.title") ]


    let renderFaqLink (visualization: Visualization) =
        if visualization.Explicit then
            Html.none // we do not have FAQ for hidden charts yet
        else
            Html.div
                [ prop.className "faq-link-wrapper"
                  prop.children
                      [ Html.a
                          [ prop.className "faq-link"
                            prop.key visualization.ClassName
                            prop.target "_blank"
                            prop.href
                                ("/"
                                 + localStorage.getItem ("i18nextLng")
                                 + "/faq#"
                                 + visualization.ClassName)
                            prop.text "?" ]
                        |> Html.div ] ]


    let renderFaqAndShareBtn (visualization: Visualization) =
        match state.RenderingMode with
        | Embedded _ ->
            Html.div
                [ prop.className "faq-and-share-wrapper"
                  prop.children
                      [ renderFaqLink visualization
                        brandLink ] ]
        | Normal ->
            Html.div
                [ prop.className "faq-and-share-wrapper"
                  prop.children
                      [ renderFaqLink visualization
                        ShareButton.dropdown visualization () ] ]


    let renderChartTitle (visualization: Visualization) =

        let scrollToElement (e: MouseEvent) visualizationId =
            e.preventDefault ()

            let element =
                document.getElementById (visualizationId)

            let offset = -100.

            let position =
                element.getBoundingClientRect().top
                + window.pageYOffset
                + offset

            window.scrollTo
                ({| top = position
                    behavior = "smooth" |}
                 |> unbox) // behavior = smooth | auto
            window.history.pushState (null, null, "#" + visualizationId)

        let context = localStorage.getItem ("contextCountry")

        Html.div
            [ prop.className "title-brand-wrapper"
              prop.children
                  [ Html.h2
                      [ prop.children
                          [ Html.a
                              [ prop.href ("#" + visualization.ClassName)
                                prop.text (tOptions ("charts." + visualization.ChartTextsGroup + ".title") {| context = context |} )
                                prop.onClick (fun e -> scrollToElement e visualization.ClassName) ]
                            Html.span
                              [ prop.text ("charts." + visualization.ChartTextsGroup + ".titleMenu")
                                prop.className "hidden" ] ] ] ] ]

    Html.div
        [ Utils.classes [
            (true, "visualization container")
            (embedded, "embeded") ]
          prop.children (
            visualizations
            |> List.map (fun viz ->
                Html.section [
                    prop.className [ viz.ClassName ; "visualization-chart" ]
                    prop.id viz.ClassName
                    prop.children [
                        Html.div [
                            prop.className "title-chart-wrapper"
                            prop.children [
                                renderChartTitle viz
                                renderFaqAndShareBtn viz
                            ]
                        ]
                        IntersectionObserver.Component.intersectionObserver
                            {| targetElementId = viz.ClassName
                               content = state |> viz.Renderer
                               options = { IntersectionObserver.defaultOptions with rootMargin = "100px" }
                            |}
                    ] ] ) ) ]
