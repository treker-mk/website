module CountriesChartViz.CountrySets

open CountriesChartViz.Analysis
open Synthesis

let setNeighboringCountries = {
      Label = "groupNeighbouring"
      CountriesCodes = [| "BGR"; "GRC"; "ALB"; "OWID_KOS"; "SRB"; "BIH"; "ROU"; "TUR"; "MNE"; "SVN"; "HRV"; "HUN" |]  // SLO-spec - changed for Macedonia
    }

let setHighestNewCases = {
    Label = "groupHighestNewCases"
    CountriesCodes = [| "CZE"; "BEL"; "NLD"; "ARM"; "SVN"; "FRA"; "CHE"; "MNE"; "ARG"; "ESP" |]
}

let setHighestActiveCases = {
    Label = "groupHighestActiveCases"
    CountriesCodes = [| "CZE"; "BEL"; "NLD"; "ARM"; "SVN"; "FRA"; "CHE"; "MNE"; "ARG"; "ESP" |]
}

let setHighestTotalDeaths = {
    Label = "groupHighestTotalDeaths"
    CountriesCodes = [| "PER"; "BEL"; "BOL"; "ESP"; "BRA"; "CHL"; "ECU"; "MEX"; "USA"; "GBR" |]
}

let setLargestEuCountries = {
    Label = "groupLargestEuCountries"
    CountriesCodes = [| "DEU"; "GBR"; "FRA"; "ITA"; "ESP"; "POL"; "ROU"; "NLD"; "BEL" |]
}

let setLargestWorldCountries = {
    Label = "groupLargestWorldCountries"
    CountriesCodes = [| "CHN"; "IND"; "USA"; "IDN"; "PAK"; "BRA"; "NGA"; "BGD"; "RUS"; "MEX"; "JPN" |]
}

let setNordic = {
    Label = "groupNordic"
    CountriesCodes = [| "DNK"; "FIN"; "ISL"; "NOR"; "SWE" |]
}

let setExYU = {
    Label = "groupExYu"
    CountriesCodes = [| "BIH"; "HRV"; "SVN"; "MNE"; "OWID_KOS"; "SRB" |]  // SLO-spec - changed for Macedonia
}

let setEastAsiaOceania = {
    Label = "groupEastAsiaOceania"
    CountriesCodes = [| "AUS"; "CHN"; "JPN"; "KOR"; "NZL"; "SGP"; "TWN" |]
}

let setLatinAmerica = {
    Label = "groupLatinAmerica"
    CountriesCodes = [| "ARG"; "BRA"; "CHL"; "COL"; "ECU"; "MEX"; "PER" |]
}

// SLO-spec
let setCriticalEU = {
    Label = "groupCriticalEU"
    CountriesCodes = [| "BEL"; "ESP"; "FRA"; "GBR"; "ITA"; "SWE" |]
}

let setCriticalWorld = {
    Label = "groupCriticalWorld"
    CountriesCodes = [| "BRA"; "ECU"; "ITA"; "RUS"; "SWE"; "USA" |]
}

let countriesDisplaySets (metric: MetricToDisplay) =
    [| setCriticalEU; setCriticalWorld
       setNeighboringCountries; setNordic
       setEastAsiaOceania; setLatinAmerica
    |]

(* SLO-spec
let countriesDisplaySets (metric: MetricToDisplay) =
    match metric with
    | NewCasesPer1M ->
        [| setNeighboringCountries; setHighestNewCases
           setLargestEuCountries; setLargestWorldCountries
           setNordic; setExYU; setEastAsiaOceania; setLatinAmerica
        |]
    | ActiveCasesPer1M ->
        [| setNeighboringCountries; setHighestActiveCases
           setLargestEuCountries; setLargestWorldCountries
           setNordic; setExYU; setEastAsiaOceania; setLatinAmerica
        |]
    | TotalDeathsPer1M ->
        [| setNeighboringCountries; setHighestTotalDeaths
           setLargestEuCountries; setLargestWorldCountries
           setNordic; setExYU; setEastAsiaOceania; setLatinAmerica
        |]
    | DeathsPerCases ->
        [| setNeighboringCountries
           setLargestEuCountries; setLargestWorldCountries
           setNordic; setExYU; setEastAsiaOceania; setLatinAmerica
        |]
*)
