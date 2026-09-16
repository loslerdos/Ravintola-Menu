using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfoImageRotator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Image image;
    [SerializeField] private CartUI cartUI;

    [Header("Sprites")]
    [SerializeField] private Sprite[] sprites;

    [Header("Timing")]
    [SerializeField] private float minDelay = 8f;
    [SerializeField] private float maxDelay = 14f;
    [SerializeField] private float spinDuration = 1.1f;

    [Header("Flip settings")]
    [SerializeField] private float flipAngle = 90f;
    [SerializeField] private float swapAngle = 82f;
    [SerializeField] private bool allowSpriteSwap = true;

    private float nextTriggerTime;
    private bool isSpinning;

    private void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (image != null && sprites != null && sprites.Length > 0)
            image.sprite = sprites[0];

        ScheduleNext();
    }

    private void Update()
    {
        if (isSpinning)
            return;

        if (cartUI != null && cartUI.IsCartOpen)
            return;

        if (!allowSpriteSwap || sprites == null || sprites.Length < 2)
            return;

        if (Time.time >= nextTriggerTime)
            StartCoroutine(SpinRoutine());
    }

    private void ScheduleNext()
    {
        nextTriggerTime = Time.time + Random.Range(minDelay, maxDelay);
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;

        if (image == null)
        {
            isSpinning = false;
            yield break;
        }

        if (cartUI != null && cartUI.IsCartOpen)
        {
            ResetVisuals();
            isSpinning = false;
            yield break;
        }

        Sprite currentSprite = image.sprite;
        Sprite nextSprite = currentSprite;

        if (allowSpriteSwap && sprites != null && sprites.Length > 1)
        {
            int currentIndex = 0;

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == currentSprite)
                {
                    currentIndex = i;
                    break;
                }
            }

            nextSprite = sprites[(currentIndex + 1) % sprites.Length];
        }

        bool swapped = false;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            if (cartUI != null && cartUI.IsCartOpen)
            {
                ResetVisuals();
                isSpinning = false;
                yield break;
            }

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);

            float yRotation;
            float scaleX;

            if (t < 0.5f)
            {
                float p = t / 0.5f;
                yRotation = Mathf.Lerp(0f, flipAngle, p);
                scaleX = Mathf.Lerp(1f, 0.12f, p);

                if (allowSpriteSwap && !swapped && yRotation >= swapAngle && sprites != null && sprites.Length > 1)
                {
                    image.sprite = nextSprite;
                    swapped = true;
                }
            }
            else
            {
                float p = (t - 0.5f) / 0.5f;
                yRotation = Mathf.Lerp(flipAngle, 0f, p);
                scaleX = Mathf.Lerp(0.12f, 1f, p);
            }

            image.rectTransform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            image.rectTransform.localScale = new Vector3(scaleX, 1f, 1f);

            yield return null;
        }

        image.rectTransform.localRotation = Quaternion.identity;
        image.rectTransform.localScale = Vector3.one;

        if (allowSpriteSwap && sprites != null && sprites.Length > 1)
            image.sprite = nextSprite;

        isSpinning = false;
        ScheduleNext();
    }

    private void ResetVisuals()
    {
        image.rectTransform.localRotation = Quaternion.identity;
        image.rectTransform.localScale = Vector3.one;
    }
}