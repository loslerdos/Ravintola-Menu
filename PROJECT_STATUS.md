# Ravintola Menu - projektin tila

Paivitetty: 2026-10-22

## Projektin tavoite

Rakennetaan Osteria del Sol -ravintolan menu Unityssa. Menun tulee esittaa tuotteet kategorioittain ja perustua rakenteiseen dataan, jotta tuotteita ja kategorioita voidaan lisata ilman UI-koodin kopiointia.

## Nykyinen tila

- Unity-versio: `6000.6.0f1`
- Render pipeline: Universal Render Pipeline (URP)
- Input System: Unityn uusi Input System
- Scene: `Assets/Scenes/Menu.unity`
- GitHub-repository: `https://github.com/loslerdos/Ravintola-Menu`
- Nykyinen pääpaino: myyntiyhteenvedon ID-törmäyskorjaus on käyttäjän Play-tilassa kokeilema: Makkaraperunat näkyy arkistomyynnissä, ja arkistoidun tilauksen poistaminen poistaa sen myynnistä odotetusti. Koko työkulun käyttöohje on kesken.

## Tehty

- Unity-projekti on luotu.
- GitHub-yhteys ja Unityn `.gitignore` ovat käytössä.
- Projektin jakaminen koti- ja koulukoneen välillä on valmisteltu.
- Tehtävänannon kuvat on tutkittu.
- Ensimmäisen vaiheen vaatimukset on kirjattu alle.
- Canvas UI on valittu käyttöliittymätekniikaksi.
- Canvas Scaler käyttää `Scale With Screen Size` -asetusta, viiteresoluutiolla `1920 x 1080`.
- `MenuItem`-dataluokassa on id, nimi, kuvaus, hinta, kategoria ja vapaaehtoinen lisähuomautus.
- `MenuManager` sisältää kategoriaryhmät: Alkuruoat, Pääruoat, Jälkiruoat ja Juomat.
- Kuhunkin kategoriaan on lisätty kaksi tuotetta Inspectorissa.
- `MenuItemCard` näyttää tuotteen kategorian, nimen, kuvauksen, lisähuomautuksen ja hinnan.
- `MenuItemCard` on tallennettu Prefabiksi: `Assets/Prefabs/MenuItemCard.prefab`.
- Scroll View ja Vertical Layout Group on luotu tuotekorttilistaa varten.
- Header sisältää logon, ravintolan nimen, alaotsikon ja oikean reunan viestin.
- CategoryNavigation-paneeli on sijoitettu Headerin alle ja sille on tehty kolmen 1 px viivan erotinreuna.
- CategoryNavigation on upotettu visuaalisesti Headerin alle haivytetyn varjon avulla.
- CategoryButtons-sailio ja viisi kategoriapainiketta on luotu Prefabista.
- Kategoriapainikkeilla on oma vihrea kehysgrafiikka, musta erotusura, hover-savy, painallussavy ja painumisefekti.
- Kategoriapainikkeet suodattavat tuotteet: Menu, Alkuruoat, Pääruoat, Jälkiruoat ja Juomat.
- Valittu kategoria jaa aktiiviseen savyyn ja vieritys palautuu kategorian vaihdossa listan alkuun.
- MenuScrollView vierittaa vain pystysuunnassa, käyttää Clamped-liikettä ja inertiaa.
- Tuotekorttien väliä ja varjostusta on viimeistelty vihrea-musta-oranssiin ilmeeseen.
- Lisää tilaukseen -painike on rakennettu uudelleen omilla ShadowLayer- ja Face-kerroksilla; se vaalenee hoverissa ja painuu alas klikattaessa.
- `MenuManager` sisältää `cartItems`-listan valittuja tuotteita varten.
- Jokaisen dynaamisesti luodun `MenuItemCard`-kortin Lisää tilaukseen -painike välittää oikean `MenuItem`-olion `MenuManager.AddToCart`-metodille.
- Tuotteen nimi ja ostoskorin tuotemäärä tulostuvat Consoleen jokaisen lisäyksen jälkeen.
- Footer on lisätty Canvasin alareunaan omalla varjollaan.
- Vaiheen 3.2 ostoskoridata on testattu Console-tulosteilla toimivaksi.
- MenuScrollView on siirretty vasemmalle ja RightPanel luotu oikealle ostoskorin ja ravintolainformaation alueeksi.
- RightPanelille on tehty MenuScrollViewin kanssa yhtenäinen visuaalinen tyyli, mukaan lukien reunat ja varjo.
- RightPanel sisältää InfoContent-objektin suljetun ostoskorin tilaa varten.
- Footeriin on lisätty CartStatusText, CartTotalText ja OpenCartButton.
- CartContentPanel on luotu RightPanelin sisään avattavaa ostoskorilistaa varten.
- CartContentPanelin CartScrollView täyttää RightPanelin, ja Viewport maskaa vierivan kuitin paneelin reunojen sisään.
- CartScrollViewn Contentissa ovat kuitin otsikko, CartRowsContainer, loppusumma ja tulevat kuitin toimintopainikkeet. Koko kuitti vierii yhtenä kokonaisuutena.
- CartRowsContainerin sisään luodaan dynaamiset CartItemRow-rivit. CartUI käyttää rivejä uudelleen ja piilottaa ylimääräiset rivit CanvasGroup- ja LayoutElement-asetuksilla.
- CartScrollViewn Content ja CartRowsContainer käyttävät Vertical Layout Group - ja Content Size Fitter -komponentteja.
- CartItemRow-prefab ja CartItemRow-skripti näyttävät ostoskorituotteen nimen, kappalemäärän, rivihinnan sekä + ja - painikkeet.
- CartItemRow-prefabin painiketaustat ovat läpinäkyviä, joten kuitissa näkyvät hallintapainikkeiden merkit ilman erillisiä taustalaatikoita.
- CartUI-skripti näyttää CartItem-rivit, laskee kappalemääriin perustuvan loppusumman ja hallitsee tyhjän korin sekä vahvistuksen tilaviestit.
- MenuManager päivittää CartUI-näkymän sekä Footerin tuotemäärän ja loppusumman aina, kun tuote lisätään ostoskoriin.
- CartContentPanel pidetään aktiivisena, mutta piilotetaan Canvas Group -komponentilla: Alpha 0, Interactable pois ja Blocks Raycasts pois.
- InfoContentilla on oma Canvas Group. Footerin OpenCartButton kutsuu CartUI.ToggleCart()-metodia ja vaihtaa tekstin muotojen "Näytä tilaus" ja "Sulje tilaus" välillä.
- CartContentPanelin avaamisessa ja sulkemisessa on CanvasGroup-häivytys, skaalaus, siirtymä Footer-painikkeen kohdalta sekä 3D-tyylinen X/Y/Z-käännös.
- CartUI-komponentissa on Inspectorin `Hidden Opacity` -liukusäädin. Se määrittää vain avausanimaation alkuhetken näkyvyyden; suljettu ostoskori pysyy aina täysin piilossa (alpha 0), jotta Footer-painike toimii.
- Unity 6.6:ssa Scrollbarin käytöstä poistaminen aiheutti IndexOutOfRangeException-virheen. Virhe vältetään Scrollbar Visibility -asetuksella Permanent ja piilottamalla ostoskoripaneeli Canvas Groupilla eikä Set Active -asetuksella.
- Info-kuvaan voidaan lisätä hento flip-tyylinen animaatio, joka suoritetaan vain silloin, kun ostoskori on kiinni.
- Päänavigaatio on erotettu kategoriavalinnasta. Pysyvät Ruokalista-, Tilaukset-, JSON-, Tietokanta- ja Ohje-painikkeet on valmisteltu tulevia henkilökunnan näkymiä varten.
- Päänavigaation muilla kuin Ruokalista-välilehdillä vasen menualue ja RightPanel piilotetaan toistaiseksi; varsinaista henkilökuntatoiminnallisuutta ei ole vielä toteutettu.
- Vaihe 3.4 on valmis: ostoskori käyttää CartItem-rivejä, joissa ovat MenuItem ja quantity.
- Sama tuote yhdistyy yhdelle ostoskoririville. Rivin + ja - painikkeet muuttavat kappalemäärää, ja nollaan vähennetty rivi poistuu ostoskorista.
- Footer, kuitin loppusumma ja ostoskoririvin hinta lasketaan kappalemäärien perusteella.
- Tyhjä ostoskori näyttää tekstin "Ostoskori on tyhjä". Vahvista tilaus -painike, ostoskoriotsikko ja loppusumma ovat silloin piilossa.
- Vahvistettu tilaus tyhjentää cartItems-listan ja näyttää tekstin "Kiitos tilauksesta!". Uuden tuotteen lisääminen palauttaa normaalin ostoskorinäkymän.
- Dynaamisia ostoskoririvejä käytetään uudelleen CanvasGroup- ja LayoutElement-asetuksilla, eikä niitä tuhota päivityksessä.
- MenuScrollViewn Vertical Scrollbar Visibility on asetettu Permanent-tilaan Unity 6.6 UGUI:n Selectable.OnDisable-/Scrollbar.OnDisable-virheen välttämiseksi.
- Vaihe 3.5 on valmis: OrderData-luokka kuvaa tilauksen tunnisteen, tuotteet ja loppusumman.
- `OrderLineData` säilyttää jokaisella tilausrivillä `MenuItem`-tiedot, kappalemäärän ja rivin loppusumman.
- Vahvistettu tilaus muodostetaan oikeasta ostoskorista, kirjoitetaan `Application.persistentDataPath`-kansioon JSON-tiedostoksi ja luetaan takaisin `JsonUtility.FromJson`-metodilla.
- Canvasin alle on rakennettu pysyvä JSON-näkymä: `JsonPage`, `JsonTitleText`, `JsonLoadButton`, `JsonOrderPanel`, `JsonViewport` ja `JsonOutputText`.
- JSON-näkymän scene-objektit on kytketty `MenuManager`-komponenttiin Inspector-viittauksilla. Käyttöliittymäobjekteja ei luoda ajonaikaisesti koodissa.
- JSON-painike avaa oman näkymänsä ja piilottaa vain Ruokalista-sivulle kuuluvat `MenuCategoryButtons`-, `MenuScrollView`- ja `RightPanel`-alueet.
- `JsonLoadButton` etsii uusimman `order_*.json`-tiedoston, deserialisoi sen ja näyttää muotoillun JSON-tekstin vieritettävässä `JsonOrderPanel`-paneelissa.
- Projektissa käytetään vain kahta vakiintunutta nappityyliä: isot navigaatio- ja kategoriapainikkeet sekä pienet `AddToOrderButtonMotion`-toimintopainikkeet.
- Vaihe 3.7 on toteutettu: keittiönäkymä lukee aktiiviset `order_*.json`-tilaukset ja näyttää ne olemassa olevan scene-rivin pohjalta.
- Keittiön `Aktiiviset`, `Arkisto` ja `Päivitä` -painikkeet on kytketty Inspectorin kautta.
- `MenuManager.GetActiveOrderFilePaths()` lukee aktiiviset tilaustiedostot ja `ArchiveOrder()` siirtää valmiin tilauksen `ArchivedOrders`-kansioon.
- Keittiön tilausrivi näyttää tilausnumeron, tuotteet, kappalemäärät ja loppusumman.
- Arkistoiduissa tilauksissa `Valmis`-painike piilotetaan; aktiivisissa tilauksissa se arkistoi kyseisen tilauksen.
- Alkuperäinen `KitchenTestOrderRow` toimii piilotettuna scene-template-rivinä, josta näkyvät tilausrivit kloonataan.
- Vaihe 3.7 on validoitu Play-tilassa: keittiörivien dynaamiset korkeudet, vieritys sekä aktiivisten ja arkistoitujen tilausten listat toimivat.
- Keittiön aktiivisten ja arkistoitujen tilausrivien tausta-, teksti- ja otsikkovärit ovat `MenuManager`-komponentin Inspectorissa. Värit eivät ole UI-logiikassa kovakoodattuja.
- Arkistorivissä käytetään samaa toimintopainiketta kuin aktiivisessa rivissä: aktiivisessa näkymässä se näyttää `Valmis` ja arkistoi tilauksen, arkistossa se näyttää `Poista`.
- Arkistotilauksen poisto avaa `KitchenDeleteConfirmPanel`-vahvistusikkunan. `Peruuta` säilyttää JSON-tiedoston, ja `Poista pysyvästi` poistaa valitun arkistotilaustiedoston sekä päivittää listan.
- Vaihe 3.8 on valmis: Unityyn on asennettu SQLite-net for Unity -paketti ja SQLite-yhteys on testattu toimivaksi.
- `DatabaseProduct` kuvaa `Products`-taulun rivin: id, tuotteen nimi, kuvaus, lisähuomautus, kategoria ja hinta.
- `ProductDatabase` luo `ravintola-menu.db`-tietokannan `Application.persistentDataPath`-hakemistoon, luo `Products`-taulun ja kategorian indeksin tarvittaessa sekä lisää aloitustuotteet vain tyhjään tauluun.
- Aloitusdata muodostetaan nykyisestä Inspectorissa olevasta `MenuManager.categories`-listasta. Asiakasmenua ei vielä ladata SQLite-tietokannasta.
- `DatabasePage` on pysyvä scene-käyttöliittymä. `TIETOKANTA`-navigaatiopainike avaa sen, ja `PÄIVITÄ` hakee Products-taulun tuotteet uudelleen.
- Tietokantatuotteet näytetään read-only-listana `DatabaseProductRowTemplate`-pohjasta. Lista järjestetään `id`-kentän mukaan, jotta se vastaa alkuperäisen menun järjestystä.
- Vaihe 3.9 on valmis: `MenuManager.ShowAllItems()` hakee tuotteet `ProductDatabase.GetAllProducts()`-metodilla.
- `MenuManager.ShowCategory()` käyttää parametrisoitua kategoriakyselyä ja muuntaa `DatabaseProduct`-rivit nykyisiksi `MenuItem`-olioiksi.
- Tuotekortit, `AddToCart` ja ostoskori säilyvät ennallaan, vaikka tuotetiedot tulevat nyt SQLite Products -taulusta.
- Ensimmäisen tuotteen lisääminen avaa ostoskorin automaattisesti.
- CRUD:n kategoriakohtaiset ID-alueet ovat `100–199` Alkuruoille, `200–299` Pääruoille, `300–399` Jälkiruoille ja `400–499` Juomille.
- `ProductDatabase` sisältää tuotteiden lisäys-, päivitys- ja poistotoiminnot sekä kategoriakohtaisen ID-generaattorin. ID haetaan alueen suurimmasta käytössä olevasta ID:stä `ExecuteScalar<int>()`-kutsulla.
- `MenuManager` sisältää uuden tuotteen lomakkeen, rivikohtaisen muokkauksen ja poistovahvistuksen scene-kytkennät.
- Käännös `dotnet build Assembly-CSharp.csproj --no-restore` onnistuu.
- Vaihe 3.9 on validoitu Unityn Play-tilassa: tuotteen lisäys, kategorian mukainen ID-generaattori, muokkaus, poistovahvistus, tietokantalistan päivitys ja asiakasmenun päivittyminen toimivat.
- Hakukäyttöliittymä on scene-pohjainen: `MenuSearchInput` suodattaa Ruokalistan aktiivisen kategorian tuotteita ja `DatabaseSearchInput` hakee tietokantalistaa.
- `DatabaseCategoryFilterDropdown` rajaa tietokantahaun kategoriaan; `Kaikki Kategoriat` näyttää kaikki tuotteet.
- `ProductDatabase.SearchProducts()` käyttää parametrisoitua ID-aluekyselyä ja suodattaa nimen kirjainkoosta riippumatta.
- `MenuManager` kytkee hakukenttien ja kategoriavalinnan muutokset listojen päivitykseen. Ruokalistan hakupaneeli näkyy vain Ruokalista-sivulla.
- C#-projekti kääntyy hakukytkennän jälkeen, ja käyttäjä on vahvistanut haun toimivan Play-tilassa.
- Arkistotilausten myyntiyhteenveto kokoaa arkisto-JSONien snapshot-tiedoista tilausten määrän, myynnin kokonaissumman sekä tuotteittaiset kappalemäärät ja summat. Tuotteet ryhmitellään ID:n, nimen ja kategorian perusteella, jotta eri historialliset tuotteet eivät yhdisty ID:n uudelleenkäytön vuoksi.
- `KitchenSalesSummaryButton` on lisätty Tilaukset-näkymään ja kytketty `MenuManager`iin. Yhteenveto käyttää keittiön nykyistä vieritettävää tilauslistaa.
- `ProductDatabase.GetNextProductId()` huomioi myös aktiivisten ja arkistoitujen tilausten snapshotit, jotta poistetun tuotteen ID:tä ei käytetä uudelleen.
- Käyttäjä vahvisti Play-tilassa, että Makkaraperunat näkyy myyntiyhteenvedossa ja arkistoidun tilauksen poistaminen poistaa sen yhteenvedosta odotetusti.

## Tehtävänannon vaatimukset

- Canvas UI on valittu.
- Suunnittele menu, jossa on kategoriat ja tuotteet.
- Tee yhdestä tuotteesta uudelleenkäytettävä UI-elementti, esimerkiksi prefab.
- Erottele tuotteen data ja sen esitystapa käyttöliittymässä.
- Tuotteen tietoja ovat vähintään tunniste, nimi, kuvaus, hinta ja kategoria.
- Rakenna ratkaisu, joka toimii useille tuotteille ja kategorioille.
- Menun pitää alustua tuotetiedon perusteella, ei kovakoodatuista yksittäisistä UI-objekteista.
- Myöhemmissä vaiheissa tuotetietoa käsitellään JSON-muodossa ja toteutetaan tilaus- ja maksutoimintoja.

## Seuraavaksi

1. Varmista Play-tilassa, että myyntiyhteenveto säilyy ennallaan, kun arkistossa olevan tuotteen tuote poistetaan Products-taulusta.
2. Varmista, ettei uuden tuotteen ID:tä valita aiemmissa tilaus-snapshotissa käytetyistä ID:istä.
3. Laadi koko työkulun käyttöohje asiakkaan menusta ja ostoskorista tilausten JSON-tallennukseen, keittiön aktiivisiin ja arkistoituihin tilauksiin, tuotteiden CRUD-toimintoihin ja hakuihin sekä myyntiyhteenvetoon. Päätä ohjeen sijoitus ja toteutusmuoto ennen sen toteuttamista.

## Keskeneräiset asiat ja ongelmat

- Kategoriapainikkeiden kuvakkeet ovat suunnitteilla, mutta niitä ei ole toteutettu.
- Oikean reunan InfoContent tarvitsee myöhemmin lopulliset annoskuvat.
- JSON-näkymän lopullinen layout ja vieritys voidaan vielä viimeistellä eripituisilla tilauksilla.
- Koko työkulun käyttöohje puuttuu. Ohje kattaa asiakkaan menun ja ostoskorin, tilausten tallennuksen, keittiön tilaukset ja arkiston, tuotteiden hallinnan ja haun sekä myyntiyhteenvedon. Ohjeen sijoitus ja toteutusmuoto ovat vielä avoinna; Ohje-välilehti on placeholder.
- Kategoriapainikkeiden ja tietokannan kategorianimien täytyy pysyä yhdenmukaisina uusien CRUD-tuotteiden kanssa.
- Products-taulusta poistetun tuotteen arkistosnapshotin säilyminen myyntiyhteenvedossa odottaa vielä erillistä Play-testiä.

## Työskentelyohje seuraavalle kerralle

1. Lue tämä tiedosto ennen työn aloittamista.
2. Tarkista projektin nykyinen tila Unityssa ja Gitissä.
3. Tee yksi selkeä työvaihe kerrallaan.
4. Rakenna käyttöliittymä pysyvinä scene-objekteina Canvasin alle ja kytke viittaukset Inspectorissa. Älä luo sovelluksen UI:ta ajonaikaisesti koodissa.
5. Käytä vain projektin kahta olemassa olevaa nappityyliä.
6. Varmista jokainen Hierarchy- ja Inspector-vaihe ennen seuraavaa koodimuutosta.
7. Päivita tämän tiedoston kohdat `Nykyinen tila`, `Tehty`, `Seuraavaksi` ja `Keskeneraiset asiat ja ongelmat`.
8. Tallenna muutokset, tee commit ja suorita `git push`.

## Viimeisin tyokerta

- Vaihe 3.7 viimeisteltiin ja validoitiin: arkiston poisto vahvistusikkunalla, Inspectorista säädettävät keittiörivien värit sekä toimiva vieritys ja layout.
- Vaihe 3.8 toteutettiin ja validoitiin: SQLite-net, Products-taulu, kertaluonteinen aloitusdata, `DatabasePage`, Products-lista ja `PÄIVITÄ`-toiminto.
- Vaihe 3.9 on toteutettu ja validoitu: asiakasmenu käyttää SQLite Products -taulua, kategoriat toimivat, ensimmäinen tuotelisäys avaa ostoskorin, ja Products CRUD toimii Play-tilassa.
- Nimihaku ja kategoriarajaus kytkettiin molempiin näkymiin; `dotnet build Assembly-CSharp.csproj --no-restore` onnistui, ja käyttäjä vahvisti toiminnan Play-tilassa.
- Vaiheeseen 3.10 lisättiin arkistoitujen tilausten myyntiyhteenveto ja koko työkulun käyttöohje. Ohje kattaa asiakkaan ja henkilökunnan työnkulut; toteutusmuoto jätettiin avoimeksi.
- Myyntiyhteenvedon ID-törmäys korjattiin: paikallisessa arkistossa ID 204 kuuluu sekä `TestiRuoka`- että `Makkaraperunat`-snapshotille, ja raportti käsittelee ne nyt eri tuotteina. ID-generaattori tarkistaa tilaus-snapshotit.
- Käyttäjän Play-testin mukaan Makkaraperunat näkyy myyntiyhteenvedossa, ja koko arkistotilauksen poistaminen poistaa myynnin yhteenvedosta odotetusti.
- `dotnet build Assembly-CSharp.csproj --no-restore` onnistui: 0 virhettä ja 7 editor/framework-varoitusta. Products-taulusta poistamisen vaikutusta arkistoituun myyntiin ei ole vielä Play-testattu.
- Seuraava aloitus: testaa Products-tuotteen poiston vaikutus säilytettyyn arkistotilauksen snapshotiin; tämän jälkeen aloita koko työkulun käyttöohje.
- Viimeisin Git-paluupiste: `3.8 tietokanta näyttää Products-taulun tuotteet`.
