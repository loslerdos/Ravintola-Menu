using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuItemDisplay : MonoBehaviour
{
    public MenuItem tuote;
    public TMP_Text nimi;
    public TMP_Text hinta;
    public TMP_Text kategoria;
    public TMP_Text kuvaus;
    public TMP_Text lisahuomautus;
    [SerializeField] private Button addToCartButton;

    public void Display(MenuItem menuItem, Action<MenuItem> onAddToCart)
    {
        tuote = menuItem;
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

        addToCartButton.onClick.RemoveAllListeners();
        addToCartButton.onClick.AddListener(() => onAddToCart(tuote));
    }
}
