# TBD Tracking data on the COVID-19 spread in North Macedonia *SQ

*The **"Covid-19 Tracker North Macednia"** project collects, analyses and publishes data on the spread of the SARS-CoV-2 coronavirus, the cause of COVID-19, in North Macedonia. We wish to give the public a better overview of the magnitude of the issue and a proper assessment of the risk.*

## Why are we collecting this data?

In the experience of those countries where the spread of the virus has been most effectively curbed, correctly collected, up-to-date and transparently published data is vital for the effective response of public healthcare systems. 
Only then the published data can stand as the basis for understanding of what is happening, for the active self-protective behaviour of people and for accepting the urgency of the safety measures taken.
Data is collected from various publicly avalilable sources, and since TBD, we are trying to establish a direct connection with healthcare institutions and the  Institute of Public Health ([IJZ](https://www.iph.mk)). We would like to have shared with us structured data, which is then validated and shaped into a format suitable for visualization to be presented to the public as well as for further work in model development and forecasting. As data published in the media and certain other sources may sometimes be vague and inconsistent, the table also includes notes on sources and deductions based on incomplete data.

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
The team in North MAcedonia is slowly growing, not at a number of 5-10 active participants.

## Terms of use

Use and collaboration are desired: the data is collected from sources in the public domain and can be freely used, edited, processed or incorporated into any non-marketable content if citing the source – covid-19.treker.mk.
To export data to other formats, or use for visualization use please contact us at info@treker.mk
