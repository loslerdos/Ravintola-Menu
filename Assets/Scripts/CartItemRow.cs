using TMPro;
using UnityEngine;

public class CartItemRow : MonoBehaviour
{
    [SerializeField] private TMP_Text productNameText;
    [SerializeField] private TMP_Text priceText;

    public void Display(MenuItem menuItem)
    {
        productNameText.text = menuItem.productName;
        priceText.text = menuItem.price.ToString("0.00") + " \u20AC";
    }
}