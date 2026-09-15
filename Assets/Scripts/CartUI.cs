using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private RectTransform cartTransform;
    private Coroutine cartAnimation;
    private bool isCartOpen;
    private Vector3 visiblePosition;
    private Vector3 hiddenPosition;

    private void Awake()
    {
        cartTransform = transform as RectTransform;
        visiblePosition = cartTransform.position;
        hiddenPosition = toggleButtonTransform == null
            ? visiblePosition
            : toggleButtonTransform.position;
        SetCartOpen(false, false);
    }

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

        cartTotalText.text = "YHTEENSÄ: " + total.ToString("0.00") + " €";
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
            hiddenPosition = toggleButtonTransform == null
                ? visiblePosition
                : toggleButtonTransform.position;
            cartAnimation = StartCoroutine(AnimateCart(shouldOpen));
            return;
        }

        ApplyCartVisuals(shouldOpen ? 1f : 0f);
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
        cartTransform.position = Vector3.Lerp(hiddenPosition, visiblePosition, visibility);
        cartTransform.localScale = Vector3.one * Mathf.Lerp(hiddenScale, 1f, visibility);
        cartTransform.localRotation = Quaternion.Euler(
            Mathf.Lerp(hiddenPitch, 0f, visibility),
            Mathf.Lerp(hiddenYaw, 0f, visibility),
            Mathf.Lerp(hiddenRoll, 0f, visibility));
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