using System.Collections.Generic;
using TMPro;
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

    public List<CartItem> cartItems = new List<CartItem>();

    private CanvasGroup menuCategoryButtonsCanvasGroup;
    private CanvasGroup menuScrollViewCanvasGroup;
    private CanvasGroup rightPanelCanvasGroup;

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
        cartUI.Initialize(ConfirmOrder);
        RefreshCart();
        CreateApplicationNavigation();
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
    cartUI.ClearOrderStatus();

    CartItem cartItem = cartItems.Find(item => item.menuItem.id == menuItem.id);

    if (cartItem == null)
    {
        cartItems.Add(new CartItem(menuItem));
    }
    else
    {
        cartItem.quantity++;
    }

    RefreshCart();
}

public void ChangeCartItemQuantity(CartItem cartItem, int change)
{
    cartItem.quantity += change;

    if (cartItem.quantity <= 0)
    {
        cartItems.Remove(cartItem);
    }

    RefreshCart();
}

private void RefreshCart()
{
    cartUI.UpdateCartUI(cartItems, ChangeCartItemQuantity);
    UpdateFooterUI();
}

public void ConfirmOrder()
{
    if (cartItems.Count == 0)
    {
        return;
    }

    cartItems.Clear();
    cartUI.ShowOrderConfirmation();
    RefreshCart();
}

[ContextMenu("Testaa JSON-muunnos")]
private void TestJsonSerialization()
{
    OrderData testOrder = new OrderData
    {
        orderId = "TEST-001",
        items = new List<MenuItem>
        {
            new MenuItem
            {
                id = 101,
                productName = "Margherita",
                description = "Tomaattia, mozzarellaa ja basilikaa",
                specialNote = string.Empty,
                category = "Pääruoka",
                price = 12.9f
            },
            new MenuItem
            {
                id = 401,
                productName = "Limonata",
                description = "Italialainen sitruunalimonadi",
                specialNote = string.Empty,
                category = "Juoma",
                price = 4f
            }
        },
        total = 16.9f
    };

    string json = JsonUtility.ToJson(testOrder, true);
    Debug.Log("SERIALISOITU JSON:\n" + json);

    OrderData loadedOrder = JsonUtility.FromJson<OrderData>(json);

    Debug.Log(
        $"DESERIALISOITU TILAUS: {loadedOrder.orderId}, " +
        $"tuotteita: {loadedOrder.items.Count}, " +
        $"yhteensä: {loadedOrder.total:0.00} €");
}

    private void UpdateFooterUI()
{
    int productCount = 0;
    float total = 0f;

    foreach (CartItem cartItem in cartItems)
    {
        productCount += cartItem.quantity;
        total += cartItem.menuItem.price * cartItem.quantity;
    }

    cartStatusText.text = $"OSTOSKORI: {productCount} TUOTETTA";
    cartTotalText.text = "YHTEENSÄ: " + total.ToString("0.00") + " €";
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

    private void CreateApplicationNavigation()
    {
        Transform applicationNavigation = GameObject.Find("ApplicationNavigation")?.transform;
        Transform menuCategoryButtons = GameObject.Find("MenuCategoryButtons")?.transform;
        Transform rightPanel = GameObject.Find("RightPanel")?.transform;

        if (applicationNavigation == null || menuCategoryButtons == null || rightPanel == null)
        {
            Debug.LogError("Päänavigaation käyttöliittymäobjekteja ei löytynyt scenestä.");
            return;
        }

        menuCategoryButtonsCanvasGroup = GetOrAddCanvasGroup(menuCategoryButtons.gameObject);
        menuScrollViewCanvasGroup = GetOrAddCanvasGroup(menuScrollView.gameObject);
        rightPanelCanvasGroup = GetOrAddCanvasGroup(rightPanel.gameObject);

        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_MenuButton", "RUOKALISTA", true);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_OrdersButton", "TILAUKSET", false);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_JsonButton", "JSON", false);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_DatabaseButton", "TIETOKANTA", false);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_HelpButton", "OHJE", false);
    }

    private void ConfigureApplicationNavigationButton(
        Transform parent,
        string objectName,
        string buttonLabel,
        bool showsMenu)
    {
        Transform buttonTransform = parent.Find(objectName);
        if (buttonTransform == null)
        {
            Debug.LogError($"Päänavigaation painiketta {objectName} ei löytynyt scenestä.");
            return;
        }

        Button button = buttonTransform.GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();

        TMP_Text label = buttonTransform.GetComponentInChildren<TMP_Text>(true);
        label.text = buttonLabel;

        CategoryButtonMotion navigationMotion = buttonTransform.GetComponent<CategoryButtonMotion>();
        button.onClick.AddListener(() => SelectApplicationPage(showsMenu, navigationMotion));

        if (showsMenu)
        {
            navigationMotion.Select();
        }
    }

    private void SelectApplicationPage(bool showsMenu, CategoryButtonMotion navigationMotion)
    {
        navigationMotion.Select();
        SetCanvasGroupVisible(menuCategoryButtonsCanvasGroup, showsMenu);
        SetCanvasGroupVisible(menuScrollViewCanvasGroup, showsMenu);
        SetCanvasGroupVisible(rightPanelCanvasGroup, showsMenu);
    }

    private static CanvasGroup GetOrAddCanvasGroup(GameObject target)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        return canvasGroup != null ? canvasGroup : target.AddComponent<CanvasGroup>();
    }

    private static void SetCanvasGroupVisible(CanvasGroup canvasGroup, bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
}
