using UnityEngine;
using TMPro;

public class MenuItemDisplay : MonoBehaviour
{
    public MenuItem tuote;
    public TMP_Text nimi;
    public TMP_Text hinta;

    private void Start()
    {
        nimi.text = tuote.productName;
        hinta.text = tuote.price.ToString("0.00") + " €";
    }
}
