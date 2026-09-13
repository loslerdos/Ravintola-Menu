using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MenuItemDisplay menuItemCardPrefab;
    [SerializeField] private Transform menuContent;
    [SerializeField] private ScrollRect menuScrollView;

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
            card.Display(menuItem);
        }
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
