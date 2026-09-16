# Ravintola Menu - projektin tila

Paivitetty: 2026-09-16

## Projektin tavoite

Rakennetaan Osteria del Sol -ravintolan menu Unityssa. Menun tulee esittaa tuotteet kategorioittain ja perustua rakenteiseen dataan, jotta tuotteita ja kategorioita voidaan lisata ilman UI-koodin kopiointia.

## Nykyinen tila

- Unity-versio: `6000.6.0f1`
- Render pipeline: Universal Render Pipeline (URP)
- Input System: Unityn uusi Input System
- Scene: `Assets/Scenes/Menu.unity`
- GitHub-repository: `https://github.com/loslerdos/Ravintola-Menu`
- Nykyinen pääpaino: UI-animaatioiden viimeistely ja ostoskorin hallinnan valmistelu

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
- CartRowsContainerin sisään luodaan dynaamiset CartItemRow-rivit. CartUI poistaa päivityksessä vain CartItemRow-rivit, joten otsikko ja loppusumma säilyvät.
- CartScrollViewn Content ja CartRowsContainer käyttävät Vertical Layout Group - ja Content Size Fitter -komponentteja.
- CartItemRow-prefab ja CartItemRow-skripti näyttävät ostoskorituotteen nimen sekä hinnan.
- CartItemRow-prefabin taustakuva on tehty läpinäkyväksi, joten kuitissa näkyvät vain tuotteen nimi ja hinta.
- CartUI-skripti luo ostoskoririvit cartItems-listasta ja laskee loppusumman.
- MenuManager päivittää CartUI-näkymän sekä Footerin tuotemäärän ja loppusumman aina, kun tuote lisätään ostoskoriin.
- CartContentPanel pidetään aktiivisena, mutta piilotetaan Canvas Group -komponentilla: Alpha 0, Interactable pois ja Blocks Raycasts pois.
- InfoContentilla on oma Canvas Group. Footerin OpenCartButton kutsuu CartUI.ToggleCart()-metodia ja vaihtaa tekstin muotojen "Näytä tilaus" ja "Sulje tilaus" välillä.
- CartContentPanelin avaamisessa ja sulkemisessa on CanvasGroup-häivytys, skaalaus, siirtymä Footer-painikkeen kohdalta sekä 3D-tyylinen X/Y/Z-käännös.
- CartUI-komponentissa on Inspectorin `Hidden Opacity` -liukusäädin. Se määrittää vain avausanimaation alkuhetken näkyvyyden; suljettu ostoskori pysyy aina täysin piilossa (alpha 0), jotta Footer-painike toimii.
- Unity 6.6:ssa Scrollbarin käytöstä poistaminen aiheutti IndexOutOfRangeException-virheen. Virhe vältetään Scrollbar Visibility -asetuksella Permanent ja piilottamalla ostoskoripaneeli Canvas Groupilla eikä Set Active -asetuksella.
- Info-kuvaan voidaan lisätä hento flip-tyylinen animaatio, joka suoritetaan vain silloin, kun ostoskori on kiinni.

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

1. Hienosäädä InfoContentin kuvaan hento flip-animaatio, joka toimii vain kun ostoskori on suljettu.
2. Tarkista, että animaatio ei häiritse oikean paneelin avaus/sulku-tilaa.
3. Tee vahvistusikkuna yksittäisen tuotteen poistolle ja koko korin tyhjennykselle.
4. Lisää kuitin loppusummaan "Vahvista tilaus"- ja "Peru tilaus" -painikkeet.
5. Lisää tarvittaessa kategoriapainikkeiden kuvakkeet, jos tehtävänanto niin vaatii.

## Keskeneräiset asiat ja ongelmat

- Kategoriapainikkeiden kuvakkeet ovat suunnitteilla, mutta niitä ei ole toteutettu.
- Oikean reunan InfoContent tarvitsee myöhemmin lopulliset annoskuvat.
- Yksittäisen tuotteen poisto, koko ostoskorin tyhjennys, tilauksen vahvistus ja tilauksen peruutus on suunniteltu, mutta niitä ei ole vielä toteutettu.
- Info-kuvan hento animaatio on suunniteltu ja testattavaksi, mutta sen viimeinen visual-sääto odottaa Unity-testin.

## Työskentelyohje seuraavalle kerralle

1. Lue tämä tiedosto ennen työn aloittamista.
2. Tarkista projektin nykyinen tila Unityssa ja Gitissä.
3. Tee yksi selkeä työvaihe kerrallaan.
4. Päivita tämän tiedoston kohdat `Nykyinen tila`, `Tehty`, `Seuraavaksi` ja `Keskeneraiset asiat ja ongelmat`.
5. Tallenna muutokset, tee commit ja suorita `git push`.

## Viimeisin tyokerta

- Tehty: UI-pohja, ostoskori, oikean paneelin luonti, layout, avaus/sulku-animaatio ja summalaskenta on saatu toimimaan.
- Nykyinen fokus: hento InfoContent-kuvan flip-animaatio, joka aktivoituu vain silloin, kun ostoskori on suljettu.
- Seuraava aloitus: tee kuvaan pieni, elegantti flip-efekti ja sen jälkeen siirry ostoskorin hallintavaiheeseen vahvistusikkunan kanssa.
