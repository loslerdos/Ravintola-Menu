# Ravintola Menu - projektin tila

Paivitetty: 2026-09-11

## Projektin tavoite

Rakennetaan Osteria del Sol -ravintolan menu Unityssa. Menun tulee esittaa
tuotteet kategorioittain ja perustua rakenteiseen dataan, jotta tuotteita ja
kategorioita voidaan lisata ilman UI-koodin kopiointia.

## Nykyinen tila

- Unity-versio: `6000.5.7f1`
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
- CategoryButtons-sailio ja viisi kategoriapainiketta on luotu.

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

1. Viimeistele kategoriapainikkeiden yhtenäinen ulkoasu ja värit.
2. Tee `MenuManager`-logiikka, joka luo tuotekortit Prefabista kategorioiden tuotteille.
3. Kytke kategoriapainikkeet näyttämään valitun kategorian tuotteet.
4. Varmista, että tuotelista on selattava ja luettava eri ruutukoissa.
5. Lisää vasta toimivan perusmenun jälkeen hover- ja muut bonusanimaatiot.

## Keskeneräiset asiat ja ongelmat

- Category-painikkeiden hover-efektit ja kuvakkeet ovat suunnitteilla, mutta niitä ei ole toteutettu.
- Lisaystilauspainikkeessa ei ole viela toiminnallisuutta; tilaustoiminto toteutetaan tehtavan myohemmassa vaiheessa.

## Työskentelyohje seuraavalle kerralle

1. Lue tämä tiedosto ennen työn aloittamista.
2. Tarkista projektin nykyinen tila Unityssa ja Gitissä.
3. Tee yksi selkeä työvaihe kerrallaan.
4. Päivita tämän tiedoston kohdat `Nykyinen tila`, `Tehty`, `Seuraavaksi` ja `Keskeneraiset asiat ja ongelmat`.
5. Tallenna muutokset, tee commit ja suorita `git push`.

## Viimeisin tyokerta

- Tehty: Rakennettu Canvas UI:n perusrakenne, kategoriadata, tuotekortti-Prefab, vieritettävä sisältö, Header ja kategorianavigoinnin alku.
- Seuraava aloitus: Viimeistellään kategoriapainikkeiden ulkoasu ja toteutetaan tuotekorttien automaattinen luonti `MenuManager`-datan perusteella.
