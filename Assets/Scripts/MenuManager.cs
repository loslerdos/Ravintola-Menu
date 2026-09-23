using System.Collections.Generic;
using System.IO;
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

    [Header("JSON-näkymä")]
    [SerializeField] private GameObject jsonPage;
    [SerializeField] private GameObject jsonOrderPanel;
    [SerializeField] private Button jsonLoadButton;
    [SerializeField] private TMP_Text jsonOutputText;
    [SerializeField] private ScrollRect jsonOrderScrollView;

    public List<CartItem> cartItems = new List<CartItem>();

    private CanvasGroup menuCategoryButtonsCanvasGroup;
    private CanvasGroup menuScrollViewCanvasGroup;
    private CanvasGroup rightPanelCanvasGroup;

    private enum AppPage
    {
        Menu,
        Json
    }

    private AppPage currentPage = AppPage.Menu;

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
        InitializeJsonView();
        SelectApplicationPage(true, null, AppPage.Menu);
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

        OrderData orderData = BuildOrderData();
        SaveOrderToJson(orderData);

        cartItems.Clear();
        cartUI.ShowOrderConfirmation();
        RefreshCart();
    }

    private OrderData BuildOrderData()
    {
        OrderData orderData = new OrderData
        {
            orderId = "ORDER-" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss"),
            items = new List<OrderLineData>(),
            total = 0f
        };

        foreach (CartItem cartItem in cartItems)
        {
            float lineTotal = cartItem.menuItem.price * cartItem.quantity;
            orderData.items.Add(new OrderLineData
            {
                menuItem = cartItem.menuItem,
                quantity = cartItem.quantity,
                lineTotal = lineTotal
            });
            orderData.total += lineTotal;
        }

        return orderData;
    }

    private void SaveOrderToJson(OrderData orderData)
    {
        string directory = Application.persistentDataPath;
        Directory.CreateDirectory(directory);

        string fileName = "order_" + orderData.orderId + ".json";
        string path = Path.Combine(directory, fileName);

        string json = JsonUtility.ToJson(orderData, true);
        File.WriteAllText(path, json);

        Debug.Log("TILAUS TALLENNETTU: " + path + "\n" + json);
    }

    private OrderData LoadOrderFromJson(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning("Tiedostoa ei löytynyt: " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        OrderData orderData = JsonUtility.FromJson<OrderData>(json);

        if (orderData == null)
        {
            Debug.LogWarning("Ladattu JSON oli tyhjä tai virheellinen: " + path);
            return null;
        }

        Debug.Log("TILAUS LUETTU: " + path + "\n" + json);
        return orderData;
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

    private void InitializeJsonView()
{
    if (jsonPage == null ||
        jsonOrderPanel == null ||
        jsonLoadButton == null ||
        jsonOutputText == null ||
        jsonOrderScrollView == null)
    {
        Debug.LogError("JSON-näkymän Inspector-viittauksia puuttuu.");
        return;
    }

    jsonPage.SetActive(false);
    jsonOrderPanel.SetActive(false);
    jsonOutputText.text = string.Empty;

    jsonLoadButton.onClick.RemoveAllListeners();
    jsonLoadButton.onClick.AddListener(LoadLatestOrderToJsonView);
}

private void LoadLatestOrderToJsonView()
{
    string directoryPath = Application.persistentDataPath;

    if (!Directory.Exists(directoryPath))
    {
        Debug.LogWarning("Tallennuskansiota ei löytynyt.");
        return;
    }

    DirectoryInfo directory = new DirectoryInfo(directoryPath);
    FileInfo[] files = directory.GetFiles("order_*.json");

    if (files.Length == 0)
    {
        Debug.LogWarning("Tallennettuja tilauksia ei löytynyt.");
        return;
    }

    FileInfo latestFile = files[0];

    for (int index = 1; index < files.Length; index++)
    {
        if (files[index].LastWriteTimeUtc > latestFile.LastWriteTimeUtc)
        {
            latestFile = files[index];
        }
    }

    OrderData loadedOrder = LoadOrderFromJson(latestFile.Name);
    if (loadedOrder == null)
    {
        return;
    }

    jsonOutputText.text = JsonUtility.ToJson(loadedOrder, true);
    jsonOrderPanel.SetActive(true);

    Canvas.ForceUpdateCanvases();
    jsonOrderScrollView.verticalNormalizedPosition = 1f;
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

        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_MenuButton", "RUOKALISTA", true, AppPage.Menu);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_OrdersButton", "TILAUKSET", false, AppPage.Menu);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_JsonButton", "JSON", false, AppPage.Json);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_DatabaseButton", "TIETOKANTA", false, AppPage.Menu);
        ConfigureApplicationNavigationButton(applicationNavigation, "ApplicationNavigation_HelpButton", "OHJE", false, AppPage.Menu);
    }

    private void ConfigureApplicationNavigationButton(
        Transform parent,
        string objectName,
        string buttonLabel,
        bool showsMenu,
        AppPage targetPage)
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
        button.onClick.AddListener(() => SelectApplicationPage(showsMenu, navigationMotion, targetPage));

        if (showsMenu)
        {
            navigationMotion.Select();
        }
    }

    private void SelectApplicationPage(bool showsMenu, CategoryButtonMotion navigationMotion, AppPage targetPage)
    {
        currentPage = targetPage;

        if (navigationMotion != null)
        {
            navigationMotion.Select();
        }

        SetCanvasGroupVisible(menuCategoryButtonsCanvasGroup, showsMenu);
        SetCanvasGroupVisible(menuScrollViewCanvasGroup, showsMenu);
        SetCanvasGroupVisible(rightPanelCanvasGroup, showsMenu);

        bool showJsonPage = targetPage == AppPage.Json;

        if (jsonPage != null)
        {
            jsonPage.SetActive(showJsonPage);
        }
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
