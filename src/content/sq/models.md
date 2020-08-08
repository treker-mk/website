# Modelet dhe parashikimet e tyre

Në grupin tonë bashkëpunojnë edhe ekspertë për modelime statistikore dhe simulime kompjuterike.
Në këtë faqe do t’i publikojmë linqet e disa prej punimeve të tyre, që i kanë bërë me ndihmën e të dhënave të grumbulluara nga [COVID-19 Treker](https://covid-19.treker.mk).

Modelet i marrin  parasysh të gjithë informacionet e njohura për përhapjen e virusit në vend, por prapëseprapë **nuk mund të ofrojnë parashikimet më të sakta** për rrjedhën e ardhshme të epidemisë dhe **për këtë arsye ka nevojë të lexohen me kujdes të gjithë supozimet e modelit**.

Të dhënat për testet dhe numrin e personave të infektuar në shtet, si dhe në vende tjera të botës, nuk janë më të saktat. Andaj, numri më i madh i modeleve bazohen në të dhënat për personat e shtruar në spital apo, fatkeqësisht, të të vdekurve. Në qoftë se edhe këto të dhëna janë të pasakta, modelet do t’i shfaqin vlerat e ardhshme me saktësi më të ulët.

Shkenca botërore bën përpjekje marramendëse për të luftuar sëmundjen, por sërish një numër i madh i aspekteve të përhapjes dhe zhvillimit të sëmundjes nuk janë hulumtuar mirë akoma. Pasiguria në masat të cilat qeveritë nëpër botë i ndërmarrin për të frenuar përhapjen e sëmundjes është veçanërisht e madhe. Gjithashtu, për shkak të vonesave kohore, pas infektimit dhe zbulimit të sëmundjes përmes testeve, është praktikisht e pamundur të vlerësohet gjendja momentale e popullatës së infektuar dhe shpejtësia e përhapjes së infektimit në të. Të gjitha këto janë arsye dhe pengesa shtesë për modelet. Sa më larg shikojmë në të ardhmen, aq më shumë rritet pasiguria në parashikimet. 


Më shumë për [aspektet e modelimeve](https://content.sciendo.com/view/journals/sjph/59/3/article-p117.xml) nga profesori Janez Zhibert. 

## Modeli SIR

Modeli është version i përshtatur i modelit SIR të Sllovenisë (Susceptible, Infected, and Recovered) i cili është shfrytëzuar për të modeluar dinamikën e COVID-19 në Slloveni dhe për të prognozuar datën e numrit maksimal të rasteve aktive. Modeli është dizajnuar nga [prof. Ljupço Todorovski]( http://kt.ijs.si/~ljupco/), [Nikola Simixhievski](https://simidjievskin.github.io/) dhe Matej Petkoviq nga Instituti Jozhef Stefan në Lubjanë, Slloveni.
Bëhet fjalë për model elementar SIR, i kalibruar vetëm për të dhënat e rasteve aktive nga [Treker](https://covid-19.treker.mk/). Modeli kalibrohet rregullisht. Rezultatet dhe parashikimet e ardhshme mund të gjenden [këtu](http://kt.ijs.si/~ljupco/covid-19-sir.mk/report.nb.html).

<a href="http://kt.ijs.si/~ljupco/covid-19-sir.mk/daily_report.png" class="img-link">
<img alt="SIR модел" src="http://kt.ijs.si/~ljupco/covid-19-sir.mk/daily_report.png"></a>

## SEIR modeli

Modeli në vijim është adoptuar nga modeli slloven SEIR (Susceptible, Exposed, Infected, and Recovered), që është përdorur dhe përdoret për modelimin e dinamikës COVID-19 në Slloveni. Është i disponueshëm në faqen [Sledilnik](https://covid-19.sledilnik.org/) Modeli është përpunuar nga [prof. Janez Zhibert](https://pacs.zf.uni-lj.si/janez-zibert/) nga Fakulteti i Shkencave Mjekësore dhe Fakulteti i Shkencave Kompjuterike dhe Informatike në Universitetin e Lubjanës, Slloveni.

Ky është një model SEIR ndarës i shtrirë nga ndarje shtesë për modelimin e numrit të personave të shtruar në spital, NjKI dhe të vdekur. Modeli është i kalibruar mbi të dhënat e të shtrirëve në spital dhe rasteve të vdekjes nga [Treker](https://treker.mk/sq/stats). Modeli kalibrohet çdo ditë. Rezultatet dhe parashikimet e mëtutjeshme mund të gjenden [në](https://apps.lusy.fri.uni-lj.si/appsR/CoronaMK/) 

<a href="https://apps.lusy.fri.uni-lj.si/~janezz/last_simulation_MK.png" class="img-link">
<img alt="SEIR model" src="https://apps.lusy.fri.uni-lj.si/~janezz/last_simulation_MK.png"></a>

# Vlerësimi i numrit riprodhues

Numri riprodhues është numri mesatar i të infektuarve të ri nga një i infektuar. 
Përllogaritja që vijon është bërë nga [Haris Babaçiq](https://www.linkedin.com/in/harisbabacic/) nga instituti Karolinska në Stokholm, Suedi.

Numri riprodhues, i paraqitur këtu, sipas ditëve (Rt) i virusit SARS-CoV-2 në Maqedoni është llogaritur me metodë të përshtatshmërisë përmes së cilës kryhet dinamika kohore e numrit riprodhues, bazuar në lakoren e epidemisë (numri i rasteve të reja në ditë)1, ku koha e gjenerimit bazohet në modelin Ferretti et al.(2020)2. Vlerësimi i Rt është më pak i sigurt në fillim të epidemisë dhe gjatë disa ditëve të kaluara.

Kur Rt mbahet mbi 1 për një periudhë të caktuar, epidemia përhapet, dhe e kundërta - gjatë mbajtjes së Rt nën 1, epidemia dobësohet. Thënë thjesht, po të ulet numri riprodhues në 0.7, do të thotë se mesatarisht nga 10 të infektuar, shtatë do të infektonin edhe nga një person tjetër, ndërsa tre nuk do të infektonin askënd. Dhe me kalimin e kohës, me shërimin apo vdekjen e të infektuarve, numri i përgjithshëm i rasteve aktive, d.m.th. individë të cilët mund të infektojnë të tjerë, do të ulet dhe epidemia do të dobësohet derisa personi i fundit të shërohet apo të vdesë. 

<a href="https://drive.google.com/uc?export=view&id=1lN77ngzSU6M4Al3yLvo-vPsppph0mhPG" class="img-link">
<img alt="Rt" src="https://drive.google.com/uc?export=view&id=1lN77ngzSU6M4Al3yLvo-vPsppph0mhPG"></a>

**Grafikë Rt.** Numri riprodhues sipas ditëve (Rt) në Maqedoni. Pikat dhe vijat e tregojnë lëvizjen e Rt-së së vlerësuar me kalimin e kohës. Hijet me ngjyrë të kaltër të zbehtë i tregojnë 95% intervalet e besueshmërisë, d.m.th. tregojnë prej ku deri ku mund të jetë Rt i vërtetë dhe pasqyrojnë pasigurinë në Rt-në e vlerësuar.

Vlerësim të ngjashëm ka dhënë edhe [Bazhe Petrushev](https://www.linkedin.com/in/petrushev), kësaj here me metoda statistikore të Bejzit (Bayes). Në model, Rt shqyrtohet si faktor i cili ndryshon ngadalë me lëvizjen e rastësishme të Gausit, në shkallë logaritmike, ndërsa probabiliteti i rezultatit të pacientëve të rinj shqyrtohet si shpërndarje Gamma-Poisson.

<a href="https://raw.githubusercontent.com/petrushev/bayesian-modeling/covid-19-mk/07%20covid19/mk_daily_R.png" class="img-link">
<img alt="Rt-bayes" src="https://raw.githubusercontent.com/petrushev/bayesian-modeling/covid-19-mk/07%20covid19/mk_daily_R.png"></a>

Lartë: Numri riprodhues sipas ditëve (Rt) në Maqedoni. Vija e errët e tregon vlerësimin mesatar, hija e çelë dhe e errët tregojnë 95% dhe 50% interval me dendësi më të madhe, përkatësisht. 

Poshtë: Pikat e zeza i tregojnë numrat zyrtar të të infektuarve të rinj sipas ditëve. Vija e errët dhe hijet janë vlerësimi mesatar dhe intervalet.

*1.       Wallinga J, Teunis P. Different epidemic curves for severe acute respiratory syndrome reveal similar impacts of control measures. Am J Epidemiol. 2004;160(6):509-516. doi:10.1093/aje/kwh255*

*2.       Ferretti L, Wymant C, Kendall M, et al. Quantifying SARS-CoV-2 transmission suggests epidemic control with digital contact tracing. Science. March 2020. doi:10.1126/science.abb6936*


