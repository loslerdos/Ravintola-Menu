using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MenuItemDisplay menuItemCardPrefab;
    [SerializeField] private Transform menuContent;
    [SerializeField] private ScrollRect menuScrollView;

    [SerializeField] private CartUI cartUI;
    [SerializeField] private TMP_Text cartStatusText;
    [SerializeField] private TMP_Text cartTotalText;

    public List<MenuItem> cartItems = new List<MenuItem>();

    public List<MenuCategory> categories = new List<MenuCategory>
    {
        new MenuCategory { categoryName = "Alkuruoat" },
        new MenuCategory { categoryName = "Pääruoat" },
        new MenuCategory { categoryName = "Jälkiruoat" },
        new MenuCategory { categoryName = "Juomat" }
    };

    private void Start()
    {
        ShowAllItems();
        cartUI.UpdateCartUI(cartItems);
        UpdateFooterUI();
    }

    public void ShowAllItems()
    {
        ClearCards();

        foreach (MenuCategory category in categories)
        {
            CreateCards(category.menuItems);
        }

        ScrollToTop();
    }

    public void ShowCategory(string categoryName)
    {
        ClearCards();

        MenuCategory category = categories.Find(item => item.categoryName == categoryName);
        if (category != null)
        {
            CreateCards(category.menuItems);
            ScrollToTop();
        }
    }

    private void CreateCards(List<MenuItem> menuItems)
    {
        foreach (MenuItem menuItem in menuItems)
        {
            MenuItemDisplay card = Instantiate(menuItemCardPrefab, menuContent);
            card.Display(menuItem, AddToCart);
        }
    }

    public void AddToCart(MenuItem menuItem)
    {
        cartItems.Add(menuItem);

        cartUI.UpdateCartUI(cartItems);
        UpdateFooterUI();

        Debug.Log($"Lisätty ostoskoriin: {menuItem.productName}. Ostoskorissa tuotteita: {cartItems.Count}");
    }

    private void UpdateFooterUI()
    {
        float total = 0f;

        foreach (MenuItem menuItem in cartItems)
        {
            total += menuItem.price;
        }

        cartStatusText.text = $"OSTOSKORI: {cartItems.Count} TUOTETTA";
        cartTotalText.text = "YHTEENSÄ: " + total.ToString("0.00") + " \u20AC";
    }

    private void ClearCards()
    {
        foreach (Transform child in menuContent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ScrollToTop()
    {
        Canvas.ForceUpdateCanvases();
        menuScrollView.verticalNormalizedPosition = 1f;
    }
}
