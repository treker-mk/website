# Следење на податоци за ширењето на COVID-19 во Северна Македонија 

*Проектот  **"Covid-19 Трекер Северна Македонија"** собира, анализира и објавува податоци за ширењето на SARS-CoV-2 коронавирусот, предизвикувачот на COVID-19 заболувањето, во Северна Македонија. Целта е да и дадеме на јавноста подобар преглед на важноста на проблемот и соодветна процена на ризик.*

## Зошто ги собираме овие податоци?

Врз основа на искуствата на земјите кои најефективно го сузбиле ширењето на вирусот, точно собрани, навремени и транспарентно објавени податоци се клучни за ефективен одговор на јавно-здравствените системи. Објавените податоци со таков квалитет можат да бидат основа на разбирањето на состојбата, за да луѓето спроведуваат активна самозаштита и да ја прифатат итноста на преземените мерки на заштита од властите. Податоците се собираат од различни јавно-достапни извори и се обидуваме да воспоставиме директна соработка со здравствените институции и Институтот за Јавно Здравје  ([ИЈЗ](https://www.iph.mk)). Би сакале да имаме структурирани податоци кои би ги потврдиле и оформиле во формат соодветен за визуелизација, достапен за јавноста а и за понатамошна работа во развивање на математички модели и прогнозирање. Бидејќи податоците објавени во медиумите и од одредени извори можат некогаш да бидат нејасни и неконзистентни, табелата на податоци вклучува и белешки за изворите и заклучоците изведени од нецелосни податоци.

## TBD What data are we collecting?

The following data from the IJZ and various public sources is included in database on a daily basis (with history):

-   number of tests performed and number of confirmed infections
    
-   number of confirmed infections by category: by age, gender, region and municipality
    
-   hospital records for patients with COVID-19: hospitalized, in the intensive care unit (ICU), in critical condition, discharged from hospital care, recovered
    
-   monitoring of individual cases, particularly those in critical activities: working in healthcare, senior citizens’ homes, civil protection
    
-   healthcare system capacity: number of beds, intensive care units, respirators for ventilation...
    
    We are also constantly striving to add new important categories.
    All data is collected and available in form of [GSheets, CSV or via REST API.](/en/datasources)
    

<details>
  <summary>How is the data edited and verified?</summary>

The database is updated with the IJZ data (by category). The data by region and age is sometimes updated subsequently and cross-checked as the data may change as a result of epidemiological research.

TBD Municipalities are tracked in [TBD the Places table](https://docs.google.com/spreadsheets/.
Updating the hospital care data – the Patients table process:

-   All hospital announcements for COVID-19 are monitored (TBD) – around TBD oClock.
    
-   The number of hospitalizations monitored: all departments, hospitalizations in intensive care units, and patients in critical condition.
    
-   Transitions (admissions/discharges) between individual conditions are also recorded (when detectable from the data).
    
-   Where the transition data (admission/discharge) is incomplete, the values are determined by means of deduction (using a formula).
    
-   All sources and deductions are recorded as comments in individual cells (possibility of verification).
    
-   The data is compared with the summary data on hospitalized patients and patients in intensive care published daily by the TBD at TBD pm.
    

</details>

## Use of the data

The data is used for various visualizations and statistics, such as [charts, infographics and maps with information on confirmed infections and hospitalized patients](/en/stats) on our own website. 
Our data is also freely avaliable and hence used by some other portals and projects - you can find them on the [Links](/en/links) page.

<details>
  <summary>Disclaimer of responsibility (click for more)</summary>

**Please note: The information published on our site, including links to models and other sites to which we are not directly connected, is prepared with the utmost care, using available sources of data, knowledge, methodologies and technologies, in accordance with scientific standards. We believe that the visualizations and models can help explain the various factors behind the spread of the virus, including the impact of the safety measures taken and of possible future measures. Through this, we wish to emphasize that we all play an important role in this pandemic. Nonetheless, we cannot fully guarantee the accuracy, completeness or usefulness of the information on these sites, and we explicitly disclaim any responsibility for further interpretations and simulations which cite our visualizations as a source.*

</details>

## Lend a helping hand – to us, yourself and others

The project was initiated by [Luka Renko](https://twitter.com/LukaRenko) in Slovenia, for Republic of Slovenia. He began collecting data in the begining of COVID-19 epidemiy and has grown steadily into a team of 20 to 45 volunteers and active participants due to increasing need for data input and verification, as well as programming. It is a crowdsourcing project, supported by massive voluntary participation, where everyone can contribute with their resources or data to the best of their ability. Join in and lend a helping hand. Treker.mk which is a clone of sledilnik.org was initiated by Luka Renko and Vladimir Nešković (also citizen of Republic of North Macedonia) living in Slovenia and part of the Sledilnik team in Slovenia. 
The team in North MAcedonia is slowly growing, now at a number of 5-10 active participants.

## Terms of use

Use and collaboration are desired: the data is collected from sources in the public domain and can be freely used, edited, processed or incorporated into any non-marketable content if citing the source – covid-19.treker.mk.
To export data to other formats, or use for visualization use please contact us at info@treker.mk
