using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartItemRow : MonoBehaviour
{
    [SerializeField] private TMP_Text productNameText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button increaseButton;

    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        layoutElement = GetComponent<LayoutElement>();
    }

    public void Display(CartItem cartItem, Action<CartItem, int> onQuantityChanged)
    {
        SetVisible(true);

        productNameText.text = cartItem.menuItem.productName;
        quantityText.text = cartItem.quantity.ToString();

        float rowTotal = cartItem.menuItem.price * cartItem.quantity;
        priceText.text = rowTotal.ToString("0.00") + " €";

        decreaseButton.onClick.RemoveAllListeners();
        increaseButton.onClick.RemoveAllListeners();

        decreaseButton.onClick.AddListener(() => onQuantityChanged(cartItem, -1));
        increaseButton.onClick.AddListener(() => onQuantityChanged(cartItem, 1));
    }

    public void Hide()
    {
        SetVisible(false);
    }

    private void SetVisible(bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;

        layoutElement.ignoreLayout = !isVisible;
    }
}