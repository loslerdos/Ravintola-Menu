# Ravintola Menu - projektin tila

Paivitetty: 2026-09-10

## Projektin tavoite

Rakennetaan Osteria del Sol -ravintolan menu Unityssa. Menun tulee esittaa
tuotteet kategorioittain ja perustua rakenteiseen dataan, jotta tuotteita ja
kategorioita voidaan lisata ilman UI-koodin kopiointia.

## Nykyinen tila

- Unity-versio: `6000.5.7f1`
- Render pipeline: Universal Render Pipeline (URP)
- Input System: Unityn uusi Input System
- Aloitusscene: `Assets/Scenes/SampleScene.unity`
- GitHub-repository: `https://github.com/loslerdos/Ravintola-Menu`

## Tehty

- Unity-projekti on luotu.
- GitHub-yhteys ja Unityn `.gitignore` ovat käytössä.
- Projektin jakaminen koti- ja koulukoneen välillä on valmisteltu.
- Tehtävänannon kuvat on tutkittu.
- Ensimmäisen vaiheen vaatimukset on kirjattu alle.

## Tehtävänannon vaatimukset

- Valitse UI-tavaksi Canvas UI tai UI Toolkit. Canvas UI on alustava valinta.
- Suunnittele menu, jossa on kategoriat ja tuotteet.
- Tee yhdestä tuotteesta uudelleenkäytettävä UI-elementti, esimerkiksi prefab.
- Erottele tuotteen data ja sen esitystapa käyttöliittymässä.
- Tuotteen tietoja ovat vähintään tunniste, nimi, kuvaus, hinta ja kategoria.
- Rakenna ratkaisu, joka toimii useille tuotteille ja kategorioille.
- Menun pitää alustua tuotetiedon perusteella, ei kovakoodatuista yksittäisistä UI-objekteista.
- Myöhemmissä vaiheissa tuotetietoa käsitellään JSON-muodossa ja toteutetaan tilaus- ja maksutoimintoja.

## Seuraavaksi

1. Valitse ja perustele Canvas UI:n käyttö.
2. Määrittele ensimmäiset kategoriat ja tuotteet.
3. Luo tuotteen data- ja UI-rakenne.
4. Rakenna ensimmäinen toimiva näkymä `SampleScene`-sceneen.
5. Testaa projekti Unityssa ja tallenna muutokset GitHubiin.

## Keskeneräiset asiat ja ongelmat

- Ei vielä kirjattuja ongelmia.

## Työskentelyohje seuraavalle kerralle

1. Lue tämä tiedosto ennen työn aloittamista.
2. Tarkista projektin nykyinen tila Unityssa ja Gitissä.
3. Tee yksi selkeä työvaihe kerrallaan.
4. Päivita tämän tiedoston kohdat `Nykyinen tila`, `Tehty`, `Seuraavaksi` ja `Keskeneraiset asiat ja ongelmat`.
5. Tallenna muutokset, tee commit ja suorita `git push`.

## Viimeisin tyokerta

- Tehty: Projektin jatkamista varten luotu tämä tilatiedosto ja kirjattu tehtävänannon ensimmäiset vaatimukset.
- Seuraava aloitus: Suunnitellaan ravintolamenun ensimmäinen näkymä ja tuotetietojen rakenne.
