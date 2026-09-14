# Ravintola Menu - projektin tila

Paivitetty: 2026-09-14

## Projektin tavoite

Rakennetaan Osteria del Sol -ravintolan menu Unityssa. Menun tulee esittaa
tuotteet kategorioittain ja perustua rakenteiseen dataan, jotta tuotteita ja
kategorioita voidaan lisata ilman UI-koodin kopiointia.

## Nykyinen tila

- Unity-versio: `6000.6.0f1`
- Render pipeline: Universal Render Pipeline (URP)
- Input System: Unityn uusi Input System
- Scene: `Assets/Scenes/Menu.unity`
- GitHub-repository: `https://github.com/loslerdos/Ravintola-Menu`

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
- MenuScrollView vierittaa vain pystysuunnassa, kayttaa Clamped-liiketta ja inertiaa.
- Tuotekorttien valia ja varjostusta on viimeistelty vihrea-musta-oranssiin ilmeeseen.
- Lisaa tilaukseen -painike on rakennettu uudelleen omilla ShadowLayer- ja Face-kerroksilla; se vaalenee hoverissa ja painuu alas klikattaessa.
- `MenuManager` sisältää `cartItems`-listan valittuja tuotteita varten.
- Jokaisen dynaamisesti luodun `MenuItemCard`-kortin Lisaa tilaukseen -painike välittää oikean `MenuItem`-olion `MenuManager.AddToCart`-metodille.
- Tuotteen nimi ja ostoskorin tuotemäärä tulostuvat Consoleen jokaisen lisäyksen jälkeen.
- Footer on lisatty Canvasin alareunaan omalla varjollaan.

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

1. Testaa Play-tilassa, että jokaisen tuotteen Lisaa tilaukseen -painike lisää oikean tuotteen ostoskoriin ja Consoleen tulostuu nimi sekä määrä.
2. Varmista, että sama tuote voidaan lisätä useita kertoja.
3. Seuraavassa vaiheessa suunnitellaan MenuScrollView vasemmalle ja ostoskoripaneeli oikealle.

## Keskeneräiset asiat ja ongelmat

- Kategoriapainikkeiden kuvakkeet ovat suunnitteilla, mutta niitä ei ole toteutettu.
- Ostoskori on toistaiseksi vain `cartItems`-datalista ja Console-tuloste; sen käyttöliittymää ei ole vielä toteutettu.
- Ostoskori tulee myohemmin MenuScrollViewn oikealle puolelle. Nykyinen keskitetty Scroll View on valiaikainen asettelu.

## Työskentelyohje seuraavalle kerralle

1. Lue tämä tiedosto ennen työn aloittamista.
2. Tarkista projektin nykyinen tila Unityssa ja Gitissä.
3. Tee yksi selkeä työvaihe kerrallaan.
4. Päivita tämän tiedoston kohdat `Nykyinen tila`, `Tehty`, `Seuraavaksi` ja `Keskeneraiset asiat ja ongelmat`.
5. Tallenna muutokset, tee commit ja suorita `git push`.

## Viimeisin tyokerta

- Tehty: Paivitetty projekti Unity-versioon `6000.6.0f1` ja toteutettu vaihe 3.2: Lisaa tilaukseen -painike lisää oikean `MenuItem`-olion `cartItems`-listaan.
- Seuraava aloitus: Testaa ostoskoridatan toiminta Play-tilassa ja varmista Console-tulosteista, että tuote ja ostoskorin määrä ovat oikein.
