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
    [SerializeField] private string archivedOrdersDirectoryName = "ArchivedOrders";

    [Header("Keittiö")]
    [SerializeField] private GameObject kitchenPage;
    [SerializeField] private Transform kitchenOrdersContent;
    [SerializeField] private TMP_Text kitchenEmptyStateText;
    [SerializeField] private Button kitchenActiveButton;
    [SerializeField] private Button kitchenArchiveButton;
    [SerializeField] private Button kitchenRefreshButton;
    [SerializeField] private GameObject kitchenOrderRowTemplate;

    [Header("Keittiön poistovahvistus")]
    [SerializeField] private GameObject kitchenDeleteConfirmPanel;
    [SerializeField] private Button kitchenDeleteConfirmButton;
    [SerializeField] private Button kitchenDeleteCancelButton;

    [Header("Keittiön tilausrivien värit")]
    [SerializeField] private Color activeOrderBackgroundColor = new Color(0.94f, 0.88f, 0.73f, 1f);
    [SerializeField] private Color activeOrderTextColor = Color.black;
    [SerializeField] private Color activeOrderHeaderTextColor = Color.white;
    [SerializeField] private Color archivedOrderBackgroundColor = new Color(0.53f, 0.62f, 0.64f, 1f);
    [SerializeField] private Color archivedOrderTextColor = Color.black;
    [SerializeField] private Color archivedOrderHeaderTextColor = Color.white;

    public List<CartItem> cartItems = new List<CartItem>();

    private CanvasGroup menuCategoryButtonsCanvasGroup;
    private CanvasGroup menuScrollViewCanvasGroup;
    private CanvasGroup rightPanelCanvasGroup;
    private ScrollRect kitchenOrdersScrollView;
    private bool showingArchivedKitchenOrders;
    private bool kitchenViewInitialized;
    private string pendingArchivedOrderPath;

    private enum AppPage
    {
        Menu,
        Json,
        Kitchen
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
        ProductDatabase.Initialize(categories);

        ShowAllItems();
        cartUI.Initialize(ConfirmOrder);
        RefreshCart();
        CreateApplicationNavigation();
        InitializeJsonView();
        InitializeKitchenView();
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

    public string[] GetActiveOrderFilePaths()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            return new string[0];
        }

        return Directory.GetFiles(Application.persistentDataPath, "order_*.json");
    }

    public bool ArchiveOrder(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Debug.LogWarning("Arkistoitavaa tilaustiedostoa ei löytynyt: " + filePath);
            return false;
        }

        string archiveDirectory = Path.Combine(
            Application.persistentDataPath,
            archivedOrdersDirectoryName);

        Directory.CreateDirectory(archiveDirectory);

        string archivePath = Path.Combine(archiveDirectory, Path.GetFileName(filePath));

        if (File.Exists(archivePath))
        {
            archivePath = Path.Combine(
                archiveDirectory,
                Path.GetFileNameWithoutExtension(filePath)
                + "_"
                + System.DateTime.Now.ToString("yyyyMMdd-HHmmss")
                + ".json");
        }

        File.Move(filePath, archivePath);
        Debug.Log("TILAUS ARKISTOITU: " + archivePath);
        return true;
    }

    private void RequestArchivedOrderDeletion(string orderPath)
    {
        if (!showingArchivedKitchenOrders || string.IsNullOrWhiteSpace(orderPath))
        {
            return;
        }

        pendingArchivedOrderPath = orderPath;
        kitchenDeleteConfirmPanel.SetActive(true);
    }

    private void CancelArchivedOrderDeletion()
    {
        pendingArchivedOrderPath = null;
        kitchenDeleteConfirmPanel.SetActive(false);
    }

    private void ConfirmArchivedOrderDeletion()
    {
        string orderPath = pendingArchivedOrderPath;
        CancelArchivedOrderDeletion();

        if (DeleteArchivedOrder(orderPath))
        {
            RefreshKitchenOrders();
        }
    }

    private bool DeleteArchivedOrder(string orderPath)
    {
        if (string.IsNullOrWhiteSpace(orderPath) || !File.Exists(orderPath))
        {
            Debug.LogWarning("Poistettavaa arkistotilausta ei löytynyt: " + orderPath);
            return false;
        }

        try
        {
            File.Delete(orderPath);
            Debug.Log("ARKISTOTILAUS POISTETTU: " + orderPath);
            return true;
        }
        catch (IOException exception)
        {
            Debug.LogError("Arkistotilausta ei voitu poistaa: " + exception.Message);
            return false;
        }
        catch (System.UnauthorizedAccessException exception)
        {
            Debug.LogError("Ei oikeutta poistaa arkistotilausta: " + exception.Message);
            return false;
        }
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

    private void InitializeKitchenView()
    {
        kitchenPage ??= FindSceneObject("KitchenPage")?.gameObject;
        kitchenOrdersContent ??= FindSceneObject("KitchenOrdersContent");
        kitchenOrdersScrollView ??= kitchenOrdersContent?.GetComponentInParent<ScrollRect>();
        kitchenOrdersScrollView ??= FindSceneObject("KitchenOrderPanel")?.GetComponent<ScrollRect>();
        kitchenEmptyStateText ??= FindSceneObject("KitchenEmptyStateText")?.GetComponent<TMP_Text>();
        kitchenActiveButton ??= FindSceneObject("KitchenActiveButton")?.GetComponent<Button>();
        kitchenArchiveButton ??= FindSceneObject("KitchenArchiveButton")?.GetComponent<Button>();
        kitchenRefreshButton ??= FindSceneObject("KitchenRefreshButton")?.GetComponent<Button>();
        kitchenOrderRowTemplate ??= FindSceneObject("KitchenTestOrderRow")?.gameObject;

        if (kitchenPage == null ||
            kitchenOrdersContent == null ||
            kitchenEmptyStateText == null ||
            kitchenActiveButton == null ||
            kitchenArchiveButton == null ||
            kitchenRefreshButton == null ||
            kitchenOrderRowTemplate == null ||
            kitchenDeleteConfirmPanel == null ||
            kitchenDeleteConfirmButton == null ||
            kitchenDeleteCancelButton == null)
        {
            Debug.LogError("Keittiönäkymän Inspector-viittauksia puuttuu.");
            return;
        }

        kitchenPage.SetActive(false);
        kitchenOrderRowTemplate.SetActive(false);
        kitchenDeleteConfirmPanel.SetActive(false);

        kitchenActiveButton.onClick.RemoveAllListeners();
        kitchenArchiveButton.onClick.RemoveAllListeners();
        kitchenRefreshButton.onClick.RemoveAllListeners();
        kitchenDeleteConfirmButton.onClick.RemoveAllListeners();
        kitchenDeleteCancelButton.onClick.RemoveAllListeners();

        kitchenActiveButton.onClick.AddListener(ShowActiveKitchenOrders);
        kitchenArchiveButton.onClick.AddListener(ShowArchivedKitchenOrders);
        kitchenRefreshButton.onClick.AddListener(RefreshKitchenOrders);
        kitchenDeleteConfirmButton.onClick.AddListener(ConfirmArchivedOrderDeletion);
        kitchenDeleteCancelButton.onClick.AddListener(CancelArchivedOrderDeletion);

        kitchenViewInitialized = true;
        ShowActiveKitchenOrders();
    }

    private void ShowActiveKitchenOrders()
    {
        showingArchivedKitchenOrders = false;
        RefreshKitchenOrders();
    }

    private void ShowArchivedKitchenOrders()
    {
        showingArchivedKitchenOrders = true;
        RefreshKitchenOrders();
    }

    private void RefreshKitchenOrders()
    {
        if (!kitchenViewInitialized ||
            kitchenOrdersContent == null ||
            kitchenEmptyStateText == null ||
            kitchenOrderRowTemplate == null)
        {
            return;
        }

        ClearKitchenOrderRows();

        string directory = showingArchivedKitchenOrders
            ? Path.Combine(Application.persistentDataPath, archivedOrdersDirectoryName)
            : Application.persistentDataPath;

        string[] orderPaths = Directory.Exists(directory)
            ? Directory.GetFiles(directory, "order_*.json")
            : new string[0];

        kitchenEmptyStateText.text = showingArchivedKitchenOrders
            ? "Ei arkistoituja tilauksia."
            : "Ei aktiivisia tilauksia.";

        kitchenEmptyStateText.gameObject.SetActive(orderPaths.Length == 0);

        foreach (string orderPath in orderPaths)
        {
            OrderData orderData = LoadOrderFromJsonAtPath(orderPath);

            if (orderData != null)
            {
                CreateKitchenOrderRow(orderPath, orderData);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(kitchenOrdersContent as RectTransform);

        if (kitchenOrdersScrollView != null)
        {
            kitchenOrdersScrollView.verticalNormalizedPosition = 1f;
        }
    }

    private OrderData LoadOrderFromJsonAtPath(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        return JsonUtility.FromJson<OrderData>(File.ReadAllText(path));
    }

    private void CreateKitchenOrderRow(string orderPath, OrderData orderData)
    {
        GameObject row = Instantiate(kitchenOrderRowTemplate, kitchenOrdersContent);
        row.name = "KitchenOrderRow_" + orderData.orderId;
        row.SetActive(true);

        ConfigureKitchenOrderRowLayout(row, orderData);

        TMP_Text orderText = row.transform.Find("KitchenTestOrderText")?.GetComponent<TMP_Text>();

        if (orderText == null)
        {
            orderText = row.GetComponentInChildren<TMP_Text>(true);
        }

        if (orderText != null)
        {
            Color headerTextColor = showingArchivedKitchenOrders
                ? archivedOrderHeaderTextColor
                : activeOrderHeaderTextColor;

            orderText.text = FormatKitchenOrder(orderData, headerTextColor);

            orderText.color = showingArchivedKitchenOrders
                ? archivedOrderTextColor
                : activeOrderTextColor;
        }

        SetKitchenOrderRowBackgroundColor(row, showingArchivedKitchenOrders);

        Button actionButton = row.transform.Find("KitchenTestArchiveButton")?.GetComponent<Button>();

        if (actionButton == null)
        {
            actionButton = row.GetComponentInChildren<Button>(true);
        }

        if (actionButton == null)
        {
            return;
        }

        TMP_Text actionButtonText = actionButton.GetComponentInChildren<TMP_Text>(true);

        actionButton.onClick.RemoveAllListeners();
        actionButton.gameObject.SetActive(true);
        actionButton.interactable = true;

        if (showingArchivedKitchenOrders)
        {
            if (actionButtonText != null)
            {
                actionButtonText.text = "POISTA";
            }

            actionButton.onClick.AddListener(() =>
            {
                RequestArchivedOrderDeletion(orderPath);
            });
        }
        else
        {
            if (actionButtonText != null)
            {
                actionButtonText.text = "VALMIS";
            }

            actionButton.onClick.AddListener(() =>
            {
                if (ArchiveOrder(orderPath))
                {
                    RefreshKitchenOrders();
                }
            });
        }
    }

    private static void ConfigureKitchenOrderRowLayout(GameObject row, OrderData orderData)
    {
        LayoutElement layoutElement = row.GetComponent<LayoutElement>();

        if (layoutElement == null)
        {
            return;
        }

        int lineCount = 2 + (orderData.items == null ? 0 : orderData.items.Count);

        layoutElement.preferredHeight = Mathf.Max(
            100f,
            28f + lineCount * 24f);
    }

    private void SetKitchenOrderRowBackgroundColor(GameObject row, bool isArchived)
    {
        Image background = row.GetComponent<Image>();

        if (background == null)
        {
            background = row.AddComponent<Image>();
        }

        background.raycastTarget = false;

        background.color = isArchived
            ? archivedOrderBackgroundColor
            : activeOrderBackgroundColor;
    }

    private static string FormatKitchenOrder(OrderData orderData, Color headerTextColor)
    {
        string headerColorHtml = ColorUtility.ToHtmlStringRGBA(headerTextColor);

        List<string> lines = new List<string>
        {
            $"<color=#{headerColorHtml}>{orderData.orderId}</color>"
        };

        if (orderData.items != null)
        {
            foreach (OrderLineData line in orderData.items)
            {
                string productName = line.menuItem == null
                    ? "Tuote"
                    : line.menuItem.productName;

                lines.Add($"{productName} x{line.quantity}");
            }
        }

        lines.Add($"Yhteensä: {orderData.total:0.00} €");

        return string.Join("\n", lines);
    }

    private void ClearKitchenOrderRows()
    {
        for (int index = kitchenOrdersContent.childCount - 1; index >= 0; index--)
        {
            Transform child = kitchenOrdersContent.GetChild(index);

            if (child.gameObject != kitchenOrderRowTemplate &&
                child.name != "KitchenEmptyStateText")
            {
                LayoutElement layoutElement = child.GetComponent<LayoutElement>();

                if (layoutElement != null)
                {
                    layoutElement.ignoreLayout = true;
                }

                Destroy(child.gameObject);
            }
        }
    }

    private Transform FindSceneObject(string objectName)
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();

        if (canvas == null)
        {
            return null;
        }

        foreach (Transform child in canvas.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
            {
                return child;
            }
        }

        return null;
    }

    private void CreateApplicationNavigation()
    {
        Transform applicationNavigation = FindSceneObject("ApplicationNavigation");
        Transform menuCategoryButtons = FindSceneObject("MenuCategoryButtons");
        Transform rightPanel = FindSceneObject("RightPanel");

        if (applicationNavigation == null ||
            menuCategoryButtons == null ||
            rightPanel == null)
        {
            Debug.LogError("Päänavigaation käyttöliittymäobjekteja ei löytynyt scenestä.");
            return;
        }

        menuCategoryButtonsCanvasGroup = GetOrAddCanvasGroup(menuCategoryButtons.gameObject);
        menuScrollViewCanvasGroup = GetOrAddCanvasGroup(menuScrollView.gameObject);
        rightPanelCanvasGroup = GetOrAddCanvasGroup(rightPanel.gameObject);

        ConfigureApplicationNavigationButton(
            applicationNavigation,
            "ApplicationNavigation_MenuButton",
            "RUOKALISTA",
            true,
            AppPage.Menu);

        ConfigureApplicationNavigationButton(
            applicationNavigation,
            "ApplicationNavigation_JsonButton",
            "JSON",
            false,
            AppPage.Json);

        ConfigureApplicationNavigationButton(
            applicationNavigation,
            "ApplicationNavigation_DatabaseButton",
            "TIETOKANTA",
            false,
            AppPage.Menu);

        ConfigureApplicationNavigationButton(
            applicationNavigation,
            "ApplicationNavigation_HelpButton",
            "OHJE",
            false,
            AppPage.Menu);

        ConfigureApplicationNavigationButton(
            applicationNavigation,
            "ApplicationNavigation_OrdersButton",
            "TILAUKSET",
            false,
            AppPage.Kitchen);
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

        CategoryButtonMotion navigationMotion =
            buttonTransform.GetComponent<CategoryButtonMotion>();

        button.onClick.AddListener(() =>
            SelectApplicationPage(showsMenu, navigationMotion, targetPage));

        if (showsMenu)
        {
            navigationMotion.Select();
        }
    }

    private void SelectApplicationPage(
        bool showsMenu,
        CategoryButtonMotion navigationMotion,
        AppPage targetPage)
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
        bool showKitchenPage = targetPage == AppPage.Kitchen;

        if (jsonPage != null)
        {
            jsonPage.SetActive(showJsonPage);
        }

        if (kitchenPage != null)
        {
            kitchenPage.SetActive(showKitchenPage);
        }

        if (showKitchenPage)
        {
            ShowActiveKitchenOrders();
        }
    }

    private static CanvasGroup GetOrAddCanvasGroup(GameObject target)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();

        return canvasGroup != null
            ? canvasGroup
            : target.AddComponent<CanvasGroup>();
    }

    private static void SetCanvasGroupVisible(
        CanvasGroup canvasGroup,
        bool isVisible)
    {
        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
}