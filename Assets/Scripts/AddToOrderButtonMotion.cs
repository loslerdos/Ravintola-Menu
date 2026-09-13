using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddToOrderButtonMotion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform face;
    [SerializeField] private Image faceImage;
    [SerializeField] private TMP_Text label;
    [SerializeField] private Color normalColor = new Color(0.66f, 0.31f, 0.2f, 1f);
    [SerializeField] private Color hoverColor = new Color(0.82f, 0.42f, 0.27f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.42f, 0.16f, 0.1f, 1f);
    [SerializeField] private float pressedOffset = 5f;
    [SerializeField] private float motionSpeed = 45f;

    private Vector2 defaultFacePosition;
    private Vector2 targetFacePosition;
    private Color targetColor;
    private bool pointerIsOver;

    private void Awake()
    {
        defaultFacePosition = face.anchoredPosition;
        targetFacePosition = defaultFacePosition;
        targetColor = normalColor;
        faceImage.color = normalColor;
    }

    private void Update()
    {
        face.anchoredPosition = Vector2.Lerp(
            face.anchoredPosition,
            targetFacePosition,
            motionSpeed * Time.unscaledDeltaTime);
        faceImage.color = Color.Lerp(
            faceImage.color,
            targetColor,
            motionSpeed * Time.unscaledDeltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIsOver = true;
        targetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIsOver = false;
        targetColor = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetFacePosition = defaultFacePosition + Vector2.down * pressedOffset;
        targetColor = pressedColor;
        label.rectTransform.localScale = Vector3.one * 0.96f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetFacePosition = defaultFacePosition;
        targetColor = pointerIsOver ? hoverColor : normalColor;
        label.rectTransform.localScale = Vector3.one;
    }
}
