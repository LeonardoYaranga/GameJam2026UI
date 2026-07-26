using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace GameJam2026UI.UI
{
    /// <summary>
    /// Agrega efectos interactivos visuales (Hover / Click) a los botones del menú.
    /// Escala suavemente, desplaza horizontalmente y cambia el color del texto al pasar el cursor.
    /// </summary>
    public class UI_ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Scale Settings")]
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float pressedScale = 0.95f;
        [SerializeField] private float transitionSpeed = 12f;

        [Header("Position Offset Settings")]
        [SerializeField] private float hoverXOffset = 15f; // Se desplaza un poco a la derecha al hacer hover

        [Header("Color Settings")]
        [SerializeField] private Color normalColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        [SerializeField] private Color hoverColor = new Color(1f, 0.25f, 0.25f, 1f); // Rojo elegante / destacado
        [SerializeField] private Color pressedColor = new Color(0.7f, 0.15f, 0.15f, 1f);

        [Header("Target References")]
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private Graphic targetGraphic;

        private Vector3 defaultScale;
        private Vector2 defaultTextAnchoredPos;
        private Vector3 targetScale;
        private Vector2 targetTextAnchoredPos;
        private Color targetColor;

        private RectTransform rectTransform;
        private RectTransform textRectTransform;
        private bool isInitialized = false;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (isInitialized) return;

            rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                defaultScale = rectTransform.localScale;
            }

            if (targetText == null)
            {
                targetText = GetComponentInChildren<TextMeshProUGUI>();
            }

            if (targetText != null)
            {
                textRectTransform = targetText.GetComponent<RectTransform>();
                if (textRectTransform != null)
                {
                    defaultTextAnchoredPos = textRectTransform.anchoredPosition;
                    targetTextAnchoredPos = defaultTextAnchoredPos;
                }
            }

            if (targetGraphic == null && targetText == null)
            {
                targetGraphic = GetComponent<Graphic>();
            }

            targetScale = defaultScale;
            targetColor = normalColor;

            ApplyColorImmediate(normalColor);
            isInitialized = true;
        }

        private void OnEnable()
        {
            Initialize();
            ResetToDefault();
        }

        private void Update()
        {
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
            }

            if (textRectTransform != null)
            {
                textRectTransform.anchoredPosition = Vector2.Lerp(textRectTransform.anchoredPosition, targetTextAnchoredPos, Time.unscaledDeltaTime * transitionSpeed);
            }

            if (targetText != null)
            {
                targetText.color = Color.Lerp(targetText.color, targetColor, Time.unscaledDeltaTime * transitionSpeed);
            }
            else if (targetGraphic != null)
            {
                targetGraphic.color = Color.Lerp(targetGraphic.color, targetColor, Time.unscaledDeltaTime * transitionSpeed);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            targetScale = defaultScale * hoverScale;
            targetTextAnchoredPos = defaultTextAnchoredPos + new Vector2(hoverXOffset, 0f);
            targetColor = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetToDefault();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            targetScale = defaultScale * pressedScale;
            targetColor = pressedColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            targetScale = defaultScale * hoverScale;
            targetColor = hoverColor;
        }

        public void ResetToDefault()
        {
            targetScale = defaultScale;
            targetTextAnchoredPos = defaultTextAnchoredPos;
            targetColor = normalColor;
        }

        private void ApplyColorImmediate(Color color)
        {
            if (targetText != null) targetText.color = color;
            else if (targetGraphic != null) targetGraphic.color = color;
        }
    }
}
