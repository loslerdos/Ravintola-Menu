using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CartUI : MonoBehaviour
{
    [SerializeField] private Transform cartContent;
    [SerializeField] private CartItemRow cartItemRowPrefab;
    [SerializeField] private TMP_Text cartTotalText;

    public void UpdateCartUI(List<MenuItem> cartItems)
    {
        ClearCartRows();

        float total = 0f;

        foreach (MenuItem menuItem in cartItems)
        {
            CartItemRow row = Instantiate(cartItemRowPrefab, cartContent);
            row.Display(menuItem);

            total += menuItem.price;
        }

        cartTotalText.text = "YHTEENSÄ: " + total.ToString("0.00") + " \u20AC";
    }

    private void ClearCartRows()
    {
        foreach (Transform child in cartContent)
        {
            Destroy(child.gameObject);
        }
    }
}