using UnityEngine;
using TMPro;

public class MenuItemDisplay : MonoBehaviour
{
    public MenuItem tuote;
    public TMP_Text nimi;
    public TMP_Text hinta;
    public TMP_Text kategoria;
    public TMP_Text kuvaus;
    public TMP_Text lisahuomautus;

    private void Start()
    {
        nimi.text = tuote.productName;
        hinta.text = tuote.price.ToString("0.00") + " €";
        kategoria.text = tuote.category.ToUpper();
        kuvaus.text = tuote.description;

        bool hasSpecialNote = !string.IsNullOrWhiteSpace(tuote.specialNote);
        lisahuomautus.gameObject.SetActive(hasSpecialNote);

        if (hasSpecialNote)
        {
            lisahuomautus.text = tuote.specialNote;
        }
    }
}
