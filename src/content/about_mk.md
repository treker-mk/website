# Следење на податоци за ширењето на COVID-19 во Северна Македонија 

*Проектот  **"Covid-19 Трекер Северна Македонија"** собира, анализира и објавува податоци за ширењето на SARS-CoV-2 коронавирусот, предизвикувачот на COVID-19 заболувањето, во Северна Македонија. Целта е да и дадеме на јавноста подобар преглед на важноста на проблемот и соодветна процена на ризик.*

## Зошто ги собираме овие податоци?

Врз основа на искуствата на земјите кои најефективно го сузбиле ширењето на вирусот, точно собрани, навремени и транспарентно објавени податоци се клучни за ефективен одговор на јавно-здравствените системи. Објавените податоци со таков квалитет можат да бидат основа на разбирањето на состојбата, за да луѓето спроведуваат активна самозаштита и да ја прифатат итноста на преземените мерки на заштита од властите. Податоците се собираат од различни јавно-достапни извори и се обидуваме да воспоставиме директна соработка со здравствените институции и Институтот за Јавно Здравје  ([ИЈЗ](https://www.iph.mk)). Би сакале да имаме структурирани податоци кои би ги потврдиле и оформиле во формат соодветен за визуелизација, достапен за јавноста а и за понатамошна работа во развивање на математички модели и прогнозирање. Бидејќи податоците објавени во медиумите и од одредени извори можат некогаш да бидат нејасни и неконзистентни, табелата на податоци вклучува и белешки за изворите и заклучоците изведени од нецелосни податоци.

## TBD Кои податоци ги собираме?

Следниве податоци од ИЈЗ и разни јавни извори би сакале да вклучени во датабазата, на дневна основа (со  архива):):

-   број на изработени тестови и број на потврдени инфекции

-   број на потврдени инфекции по категорија: возраст, пол, регион и општина

-   болнички записи за пациентите со COVID-19: хоспитализирани, на единица за интензивно лекување (ЕИЛ), во критична состојба, отпишани од болница, опоравен

-   мониторирање на засебни случаи, особено оние лица со ризични активности: здравствени работници, лица во старечки домови, цивилна заштита

-   капацитет на здравствениот систем: број на кревети, број на единици за интензивно лекување, респиратори за вентилација...

Постојано тежнееме кон тоа да додадеме нови важни категории. 
Сите податоци се собрани и достапни во овие формати: [GSheets, CSV or via REST API.](/mk/datasources)
    

<details>
  <summary>Како се уредени и верифицирани податоците? </summary>

Датабазата е ажурирана со податоци од ИЈЗ (по категорија). Податоците по региони и возрасни групи се понекогаш дополнително додадени и вкрстено проверени бидејќи овие податоци можно е да се сменат следствено на епидемиолошки истражувања. 

Општините се следени во  [TBD табела со општини](https://docs.google.com/spreadsheets/.
Жечбата ни е да стигнеме до оваква процедура на ажурирање на податоци за пациенти на болничка нега. Моментално зависна од достапот до податоците: 
    
-   Се следат сите болнички објави за COVID-19 преку целиот ден.

-   Бројот на хоспитализации се следи по: цели оддели, единици за интензивно лекување и пациенти во критична состојба.

-   Каде што е можно, се следи преминот од една во друга болничка состојба (прием/отпуст).

-   Каде не е можно да се следи, преминот од една во друга болничка состојба (прием/отпуст) се изведува по формула.

-   Сите извори и изведувања на заклучоци се анотирани како коментари во засебни ќелии (отворена можност за верификација).

-   Податоците се споредуваат со сумираните податоци за хоспитализирани пациенти и пациентите на интензивно лекување, објавувани дневно од TBD во TBD pm.


</details>

## Употреба на податоците

Податоците се користат за разни визуелизации и статистика, како што се [графикони, инфографици и мапи со податоци за потврдени случаи и хоспитализирани пациенти ](/mk/stats) на нашиот веб сајт. 
Нашите податоци се јавно достапни и се користат од други портали и проекти -  можете да ги најдете на оваа страна [Линкови](/mk/links).

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
