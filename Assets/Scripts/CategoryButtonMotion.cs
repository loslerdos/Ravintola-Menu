using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CategoryButtonMotion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonGroup
    {
        MenuCategory,
        ApplicationNavigation
    }

    [SerializeField] private Image buttonFace;
    [SerializeField] private TMP_Text label;
    [SerializeField] private ButtonGroup buttonGroup;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.86f, 0.92f, 0.86f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.42f, 0.5f, 0.42f, 1f);
    [SerializeField] private Color selectedColor = new Color(0.72f, 0.54f, 0.38f, 1f);
    [SerializeField] private float colorSpeed = 18f;
    [SerializeField] private float pressedOffset = 3f;
    [SerializeField] private float pressSpeed = 45f;
    [SerializeField] private float labelTilt = 3f;
    [SerializeField] private float labelTiltSpeed = 55f;

    private Color targetColor;
    private Vector2 defaultFacePosition;
    private Vector2 targetFacePosition;
    private float targetLabelRotation;
    private bool pointerIsOver;
    private bool isSelected;

    private static CategoryButtonMotion selectedCategoryButton;
    private static CategoryButtonMotion selectedApplicationButton;

    private void Awake()
    {
        targetColor = normalColor;
        defaultFacePosition = buttonFace.rectTransform.anchoredPosition;
        targetFacePosition = defaultFacePosition;
        SetButtonColor(normalColor);
    }

    private void Update()
    {
        Color currentColor = Color.Lerp(
            buttonFace.color,
            targetColor,
            colorSpeed * Time.unscaledDeltaTime);
        SetButtonColor(currentColor);

        buttonFace.rectTransform.anchoredPosition = Vector2.Lerp(
            buttonFace.rectTransform.anchoredPosition,
            targetFacePosition,
            pressSpeed * Time.unscaledDeltaTime);

        float labelRotation = Mathf.LerpAngle(
            label.rectTransform.localEulerAngles.z,
            targetLabelRotation,
            labelTiltSpeed * Time.unscaledDeltaTime);
        label.rectTransform.localEulerAngles = new Vector3(0f, 0f, labelRotation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIsOver = true;
        targetColor = isSelected ? selectedColor : hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIsOver = false;
        targetColor = isSelected ? selectedColor : normalColor;
        targetLabelRotation = 0f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetColor = pressedColor;
        targetFacePosition = defaultFacePosition + Vector2.down * pressedOffset;
        targetLabelRotation = -labelTilt;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetColor = isSelected
            ? selectedColor
            : pointerIsOver ? hoverColor : normalColor;
        targetFacePosition = defaultFacePosition;
        targetLabelRotation = 0f;
    }

    public void Select()
    {
        CategoryButtonMotion selectedButton = GetSelectedButton();
        if (selectedButton != null && selectedButton != this)
        {
            selectedButton.isSelected = false;
            selectedButton.targetColor = selectedButton.pointerIsOver
                ? selectedButton.hoverColor
                : selectedButton.normalColor;
        }

        SetSelectedButton(this);
        isSelected = true;
        targetColor = selectedColor;
    }

    private CategoryButtonMotion GetSelectedButton()
    {
        return buttonGroup == ButtonGroup.MenuCategory
            ? selectedCategoryButton
            : selectedApplicationButton;
    }

    private void SetSelectedButton(CategoryButtonMotion button)
    {
        if (buttonGroup == ButtonGroup.MenuCategory)
        {
            selectedCategoryButton = button;
        }
        else
        {
            selectedApplicationButton = button;
        }
    }

    private void SetButtonColor(Color color)
    {
        buttonFace.color = color;
    }
}
