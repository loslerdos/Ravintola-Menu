using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CartUI : MonoBehaviour
{
    [SerializeField] private Transform cartContent;
    [SerializeField] private CartItemRow cartItemRowPrefab;
    [SerializeField] private TMP_Text cartTotalText;
    [SerializeField] private CanvasGroup cartCanvasGroup;
    [SerializeField] private CanvasGroup infoContentCanvasGroup;
    [SerializeField] private TMP_Text toggleButtonLabel;
    [SerializeField] private RectTransform toggleButtonTransform;
    [SerializeField] private float animationDuration = 0.38f;
    [SerializeField, Range(0f, 1f)] private float hiddenOpacity = 0.25f;
    [SerializeField] private float hiddenScale = 0.68f;
    [SerializeField] private float hiddenPitch = 12f;
    [SerializeField] private float hiddenYaw = -78f;
    [SerializeField] private float hiddenRoll = -10f;
    [SerializeField] private Button confirmOrderButton;
    [SerializeField] private TMP_Text orderStatusText;
    [SerializeField] private TMP_Text cartTitleText;

    private RectTransform cartTransform;
    private Coroutine cartAnimation;
    private bool isCartOpen;
    private Vector2 visibleAnchoredPosition;
    private Vector2 hiddenAnchoredPosition;
    private readonly List<CartItemRow> cartRows = new List<CartItemRow>();

    private CanvasGroup confirmOrderCanvasGroup;
    private LayoutElement confirmOrderContainerLayoutElement;
    private LayoutElement orderStatusLayoutElement;
    private CanvasGroup cartTotalCanvasGroup;
    private CanvasGroup orderStatusCanvasGroup;
    private LayoutElement cartTotalLayoutElement;
    private bool orderWasConfirmed;
    private CanvasGroup cartTitleCanvasGroup;
    private LayoutElement cartTitleLayoutElement;

    public bool IsCartOpen => isCartOpen;
    private void Awake()
    {
        cartTransform = transform as RectTransform;
        visibleAnchoredPosition = cartTransform.anchoredPosition;
        UpdateHiddenCartPosition();
        SetCartOpen(false, false);
    }

    public void Initialize(Action onConfirmOrder)
{
    confirmOrderButton.onClick.RemoveAllListeners();
    confirmOrderButton.onClick.AddListener(() => onConfirmOrder());

    Transform confirmOrderContainer = confirmOrderButton.transform.parent;

    confirmOrderCanvasGroup = confirmOrderContainer.GetComponent<CanvasGroup>();
    if (confirmOrderCanvasGroup == null)
    {
        confirmOrderCanvasGroup = confirmOrderContainer.gameObject.AddComponent<CanvasGroup>();
    }

    confirmOrderContainerLayoutElement =
        confirmOrderContainer.GetComponent<LayoutElement>();

    orderStatusLayoutElement = orderStatusText.GetComponent<LayoutElement>();
    if (orderStatusLayoutElement == null)
    {
        orderStatusLayoutElement = orderStatusText.gameObject.AddComponent<LayoutElement>();
    }

    orderStatusLayoutElement.minHeight = 42f;
    orderStatusLayoutElement.preferredHeight = 42f;

    orderStatusText.text = string.Empty;

    cartTotalCanvasGroup = cartTotalText.GetComponent<CanvasGroup>();
    if (cartTotalCanvasGroup == null)
    {
        cartTotalCanvasGroup = cartTotalText.gameObject.AddComponent<CanvasGroup>();
    }

    cartTitleCanvasGroup = cartTitleText.GetComponent<CanvasGroup>();
    if (cartTitleCanvasGroup == null)
    {
        cartTitleCanvasGroup = cartTitleText.gameObject.AddComponent<CanvasGroup>();
    }

    cartTitleLayoutElement = cartTitleText.GetComponent<LayoutElement>();
    if (cartTitleLayoutElement == null)
    {
        cartTitleLayoutElement = cartTitleText.gameObject.AddComponent<LayoutElement>();
    }

    orderStatusCanvasGroup = orderStatusText.GetComponent<CanvasGroup>();
    if (orderStatusCanvasGroup == null)
    {
        orderStatusCanvasGroup = orderStatusText.gameObject.AddComponent<CanvasGroup>();
    }

    cartTotalLayoutElement = cartTotalText.GetComponent<LayoutElement>();
    if (cartTotalLayoutElement == null)
    {
        cartTotalLayoutElement = cartTotalText.gameObject.AddComponent<LayoutElement>();
    }
    }

    public void ShowOrderConfirmation()
    {
    orderWasConfirmed = true;
    orderStatusText.text = "Kiitos tilauksesta!";
    }

    public void ClearOrderStatus()
    {
        orderWasConfirmed = false;
        orderStatusText.text = string.Empty;
    }
    public void UpdateCartUI(
    List<CartItem> cartItems,
    Action<CartItem, int> onQuantityChanged)
{
    for (int index = 0; index < cartItems.Count; index++)
    {
        CartItemRow row;

        if (index < cartRows.Count)
        {
            row = cartRows[index];
        }
        else
        {
            row = Instantiate(cartItemRowPrefab, cartContent);
            cartRows.Add(row);
        }

        row.Display(cartItems[index], onQuantityChanged);
    }

    for (int index = cartItems.Count; index < cartRows.Count; index++)
    {
        cartRows[index].Hide();
    }

    float total = 0f;

    foreach (CartItem cartItem in cartItems)
    {
        total += cartItem.menuItem.price * cartItem.quantity;
    }

    bool cartIsEmpty = cartItems.Count == 0;

    cartTotalText.text = "YHTEENSÄ: " + total.ToString("0.00") + " €";

    SetCartTitleVisibility(!cartIsEmpty);
    SetCartTotalVisibility(!cartIsEmpty);
    SetConfirmOrderVisibility(!cartIsEmpty);

    if (cartIsEmpty)
    {
        SetOrderStatusVisibility(true);

        orderStatusText.text = orderWasConfirmed
            ? "Kiitos tilauksesta!"
            : "Ostoskori on tyhjä";
    }
    else
    {
        SetOrderStatusVisibility(false);
    }
        
    }

    public void OpenCart()
    {
        SetCartOpen(true, true);
    }

    public void CloseCart()
    {
        SetCartOpen(false, true);
    }

    public void ToggleCart()
    {
        SetCartOpen(!isCartOpen, true);
    }

    private void SetCartOpen(bool shouldOpen, bool animate)
    {
        isCartOpen = shouldOpen;
        cartCanvasGroup.interactable = shouldOpen;
        cartCanvasGroup.blocksRaycasts = shouldOpen;

        infoContentCanvasGroup.interactable = !shouldOpen;
        infoContentCanvasGroup.blocksRaycasts = !shouldOpen;

        toggleButtonLabel.text = shouldOpen ? "Sulje tilaus" : "Näytä tilaus";

        if (cartAnimation != null)
        {
            StopCoroutine(cartAnimation);
        }

        if (animate)
        {
            UpdateHiddenCartPosition();
            cartAnimation = StartCoroutine(AnimateCart(shouldOpen));
            return;
        }

        ApplyCartVisuals(shouldOpen ? 1f : 0f);
    }

    private void SetConfirmOrderVisibility(bool isVisible)
    {
    confirmOrderButton.interactable = isVisible;

    confirmOrderCanvasGroup.alpha = isVisible ? 1f : 0f;
    confirmOrderCanvasGroup.interactable = isVisible;
    confirmOrderCanvasGroup.blocksRaycasts = isVisible;

    confirmOrderContainerLayoutElement.ignoreLayout = !isVisible;
    }

    private void SetCartTotalVisibility(bool isVisible)
    {
    cartTotalCanvasGroup.alpha = isVisible ? 1f : 0f;
    cartTotalCanvasGroup.interactable = isVisible;
    cartTotalCanvasGroup.blocksRaycasts = isVisible;
    cartTotalLayoutElement.ignoreLayout = !isVisible;
    }

    private void SetCartTitleVisibility(bool isVisible)
    {
    cartTitleCanvasGroup.alpha = isVisible ? 1f : 0f;
    cartTitleCanvasGroup.interactable = false;
    cartTitleCanvasGroup.blocksRaycasts = false;
    cartTitleLayoutElement.ignoreLayout = !isVisible;
    }

    private void SetOrderStatusVisibility(bool isVisible)
    {
        orderStatusCanvasGroup.alpha = isVisible ? 1f : 0f;
        orderStatusCanvasGroup.interactable = false;
        orderStatusCanvasGroup.blocksRaycasts = false;
        orderStatusLayoutElement.ignoreLayout = !isVisible;
    }

    private System.Collections.IEnumerator AnimateCart(bool shouldOpen)
    {
        float elapsed = 0f;
        float startValue = shouldOpen ? hiddenOpacity : cartCanvasGroup.alpha;
        float targetValue = shouldOpen ? 1f : 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, elapsed / animationDuration);
            ApplyCartVisuals(Mathf.Lerp(startValue, targetValue, progress));
            yield return null;
        }

        ApplyCartVisuals(targetValue);
        cartAnimation = null;
    }

    private void ApplyCartVisuals(float visibility)
    {
        cartCanvasGroup.alpha = visibility;
        infoContentCanvasGroup.alpha = 1f - visibility;
        cartTransform.anchoredPosition = Vector2.Lerp(
            hiddenAnchoredPosition,
            visibleAnchoredPosition,
            visibility);
        cartTransform.localScale = Vector3.one * Mathf.Lerp(hiddenScale, 1f, visibility);
        cartTransform.localRotation = Quaternion.Euler(
            Mathf.Lerp(hiddenPitch, 0f, visibility),
            Mathf.Lerp(hiddenYaw, 0f, visibility),
            Mathf.Lerp(hiddenRoll, 0f, visibility));
    }

    private void UpdateHiddenCartPosition()
    {
        if (toggleButtonTransform == null ||
            cartTransform.parent is not RectTransform parentTransform)
        {
            hiddenAnchoredPosition = visibleAnchoredPosition;
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        Camera camera = canvas != null &&
                        canvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? canvas.worldCamera
            : null;

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(
            camera,
            toggleButtonTransform.position);

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentTransform,
                screenPosition,
                camera,
                out hiddenAnchoredPosition))
        {
            hiddenAnchoredPosition = visibleAnchoredPosition;
        }
    }

    private void ClearCartRows()
    {
        foreach (Transform child in cartContent)
        {
            if (child.GetComponent<CartItemRow>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}