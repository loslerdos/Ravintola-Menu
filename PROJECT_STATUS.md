# Ravintola Menu - projektin tila

Paivitetty: 2026-09-15

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
- Vaiheen 3.2 ostoskoridata on testattu Console-tulosteilla toimivaksi.
- MenuScrollView on siirretty vasemmalle ja RightPanel luotu oikealle ostoskorin ja ravintolainformaation alueeksi.
- RightPanelille on tehty MenuScrollViewin kanssa yhtenäinen visuaalinen tyyli, mukaan lukien reunat ja varjo.
- RightPanel sisältää InfoContent-objektin suljetun ostoskorin tilaa varten.
- Footeriin on lisätty CartStatusText, CartTotalText ja OpenCartButton.
- CartContentPanel on luotu RightPanelin sisään avattavaa ostoskorilistaa varten.
- CartContentPanelin CartScrollView tayttaa RightPanelin, ja Viewport maskaa vierivan kuitin paneelin reunojen sisaan.
- CartScrollViewn Contentissa ovat kuitin otsikko, CartRowsContainer, loppusumma ja tulevat kuitin toimintopainikkeet. Koko kuitti vierii yhtena kokonaisuutena.
- CartRowsContainerin sisaan luodaan dynaamiset CartItemRow-rivit. CartUI poistaa paivityksessa vain CartItemRow-rivit, joten otsikko ja loppusumma sailyvat.
- CartScrollViewn Content ja CartRowsContainer kayttavat Vertical Layout Group- ja Content Size Fitter -komponentteja.
- CartItemRow-prefab ja CartItemRow-skripti näyttävät ostoskorituotteen nimen sekä hinnan.
- CartItemRow-prefabin taustakuva on tehty lapinakyvaksi, joten kuitissa nakyvat vain tuotteen nimi ja hinta.
- CartUI-skripti luo ostoskoririvit cartItems-listasta ja laskee loppusumman.
- MenuManager päivittää CartUI-näkymän sekä Footerin tuotemäärän ja loppusumman aina, kun tuote lisätään ostoskoriin.
- CartContentPanel pidetään aktiivisena, mutta piilotetaan Canvas Group -komponentilla: Alpha 0, Interactable pois ja Blocks Raycasts pois.
- InfoContentilla on oma Canvas Group. Footerin OpenCartButton kutsuu CartUI.ToggleCart()-metodia ja vaihtaa tekstin muotojen "Näytä tilaus" ja "Sulje tilaus" välillä.
- CartContentPanelin avaamisessa ja sulkemisessa on CanvasGroup-häivytys, skaalaus, siirtymä Footer-painikkeen kohdalta sekä 3D-tyylinen X/Y/Z-käännös.
- CartUI-komponentissa on Inspectorin `Hidden Opacity` -liukusäädin. Se määrittää vain avausanimaation alkuhetken näkyvyyden; suljettu ostoskori pysyy aina täysin piilossa (alpha 0), jotta Footer-painike toimii.
- Unity 6.6:ssa Scrollbarin käytöstä poistaminen aiheutti IndexOutOfRangeException-virheen. Virhe vältetään Scrollbar Visibility -asetuksella Permanent ja piilottamalla ostoskoripaneeli Canvas Groupilla eikä Set Active -asetuksella.

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

1. Odota seuraavaa tehtävänantoa ennen uusien ostoskoritoimintojen toteuttamista.
2. Toteuta myöhemmin yksi uudelleenkaytettava vahvistusikkuna Canvasin alle: ostoskoririvin klikkaus vahvistaa yksittaisen tuotteen poiston ja kuitin loppusumman klikkaus vahvistaa koko korin tyhjennyksen.
3. Lisaa myohemmin kuitin loppusumman alle vierivat "Vahvista tilaus"- ja "Peru tilaus" -painikkeet. Peru tilaus tarvitsee vahvistusikkunan ennen korin tyhjennysta.
4. Lisää kategoriapainikkeille kuvakkeet, jos ne kuuluvat tulevaan tehtävänantoon.

## Keskeneräiset asiat ja ongelmat

- Kategoriapainikkeiden kuvakkeet ovat suunnitteilla, mutta niitä ei ole toteutettu.
- Oikean reunan InfoContent tarvitsee myöhemmin lopulliset annoskuvat.
- Yksittaisen tuotteen poisto, koko ostoskorin tyhjennys, tilauksen vahvistus ja tilauksen peruutus on suunniteltu, mutta niita ei ole viela toteutettu.

## Työskentelyohje seuraavalle kerralle

1. Lue tämä tiedosto ennen työn aloittamista.
2. Tarkista projektin nykyinen tila Unityssa ja Gitissä.
3. Tee yksi selkeä työvaihe kerrallaan.
4. Päivita tämän tiedoston kohdat `Nykyinen tila`, `Tehty`, `Seuraavaksi` ja `Keskeneraiset asiat ja ongelmat`.
5. Tallenna muutokset, tee commit ja suorita `git push`.

## Viimeisin tyokerta

- Tehty: Vaihe 3.3 on viimeistelty. Kuitin otsikko, ostoskoririvit ja loppusumma vierivat yhtena kokonaisuutena RightPanelissa. Footerin yksi painike avaa ja sulkee kuitin, ja avauksessa/sulkemisessa on 3D-tyylinen animaatio. Avausanimaation alkuopasiteettia voi säätää CartUI-komponentin `Hidden Opacity` -asetuksella.
- Seuraava aloitus: Odota seuraavaa tehtävänantoa. Tuleva ostoskorin hallintavaihe alkaa vahvistusikkunan suunnittelusta ja toteutuksesta, ei yksittaisista poisto- tai tyhjennyspainikkeista.
