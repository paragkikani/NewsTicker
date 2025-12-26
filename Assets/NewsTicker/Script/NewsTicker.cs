using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace NewsTicker.Script
{
    public class NewsTicker : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] private TickerPosition position = TickerPosition.BottomRight;
        [SerializeField] private Vector2 offset = new Vector2(20f, 20f);
    
        [Header("Movement Points")]
        [SerializeField] private Vector2 startPoint = new Vector2(1920f, 0f);
        [SerializeField] private Vector2 endPoint = new Vector2(-200f, 0f);
        [SerializeField] private bool useRelativePoints = true; // If true, points are relative to position
    
        [Header("Movement Settings")]
        [SerializeField] private TickerDirection direction = TickerDirection.RightToLeft;
        [SerializeField] private float moveDuration = 5f;
        [SerializeField] private AnimationCurve moveCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
        [Header("Fade Settings")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float fadeOutDuration = 0.5f;
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
        [Header("End Behavior")]
        [SerializeField] private float holdAtEndDuration = 1f; // Time to show complete text at end before fade out
    
        [Header("References")]
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;
    
        [Header("Events")]
        public UnityEvent OnTickerComplete;
        public UnityEvent OnReachEndPoint;
    
        [Header("Preview (Editor Only)")]
        [SerializeField] private bool previewPosition = false;
        [SerializeField] private bool previewStartPoint = false;
        [SerializeField] private bool previewEndPoint = false;
    
        private Canvas parentCanvas;
        private RectTransform canvasRect;
        private Coroutine tickerCoroutine;
        private bool isAnimating = false;
    
        private void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                canvasRect = parentCanvas.GetComponent<RectTransform>();
            }
        
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
        
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    
        private void OnValidate()
        {
            if (!Application.isPlaying && Application.isEditor)
            {
                if (previewPosition)
                {
                    UpdatePosition();
                }
            
                if (previewStartPoint)
                {
                    PreviewPoint(startPoint);
                }
                else if (previewEndPoint)
                {
                    PreviewPoint(endPoint);
                }
            }
        }
    
        private void PreviewPoint(Vector2 point)
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
        
            UpdatePosition();
        
            if (useRelativePoints)
            {
                rectTransform.anchoredPosition += point;
            }
            else
            {
                rectTransform.anchoredPosition = point;
            }
        }
    
        public void ShowTicker(string text)
        {
            if (isAnimating)
            {
                StopTicker();
            }
        
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        
            gameObject.SetActive(true);
            tickerCoroutine = StartCoroutine(TickerAnimationCoroutine());
        }
    
        public void StopTicker()
        {
            if (tickerCoroutine != null)
            {
                StopCoroutine(tickerCoroutine);
                tickerCoroutine = null;
            }
            isAnimating = false;
            gameObject.SetActive(false);
        }
    
        private IEnumerator TickerAnimationCoroutine()
        {
            isAnimating = true;
        
            // Setup initial position
            UpdatePosition();
            Vector2 basePosition = rectTransform.anchoredPosition;
            Vector2 actualStartPoint = useRelativePoints ? basePosition + startPoint : startPoint;
            Vector2 actualEndPoint = useRelativePoints ? basePosition + endPoint : endPoint;
        
            rectTransform.anchoredPosition = actualStartPoint;
        
            yield return StartCoroutine(FadeCoroutine(0f, 1f, fadeInDuration));
        
            yield return StartCoroutine(MoveCoroutine(actualStartPoint, actualEndPoint, moveDuration));
        
            OnReachEndPoint?.Invoke();
            yield return new WaitForSeconds(holdAtEndDuration);
        
            yield return StartCoroutine(FadeCoroutine(1f, 0f, fadeOutDuration));
        
            isAnimating = false;
            OnTickerComplete?.Invoke();
            gameObject.SetActive(false);
        }
    
        private IEnumerator FadeCoroutine(float fromAlpha, float toAlpha, float duration)
        {
            float elapsed = 0f;
        
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float curveValue = fadeCurve.Evaluate(t);
                canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, curveValue);
                yield return null;
            }
        
            canvasGroup.alpha = toAlpha;
        }
    
        private IEnumerator MoveCoroutine(Vector2 from, Vector2 to, float duration)
        {
            float elapsed = 0f;
        
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float curveValue = moveCurve.Evaluate(t);
                rectTransform.anchoredPosition = Vector2.Lerp(from, to, curveValue);
                yield return null;
            }
        
            rectTransform.anchoredPosition = to;
        }
    
        private void UpdatePosition()
        {
            if (canvasRect == null || rectTransform == null) return;
        
            Vector2 anchorMin = Vector2.zero;
            Vector2 anchorMax = Vector2.zero;
            Vector2 pivot = Vector2.zero;
            Vector2 anchoredPos = Vector2.zero;
        
            switch (position)
            {
                case TickerPosition.BottomLeft:
                    anchorMin = anchorMax = new Vector2(0f, 0f);
                    pivot = new Vector2(0f, 0f);
                    anchoredPos = offset;
                    break;
                case TickerPosition.BottomRight:
                    anchorMin = anchorMax = new Vector2(1f, 0f);
                    pivot = new Vector2(1f, 0f);
                    anchoredPos = new Vector2(-offset.x, offset.y);
                    break;
                case TickerPosition.TopLeft:
                    anchorMin = anchorMax = new Vector2(0f, 1f);
                    pivot = new Vector2(0f, 1f);
                    anchoredPos = new Vector2(offset.x, -offset.y);
                    break;
                case TickerPosition.TopRight:
                    anchorMin = anchorMax = new Vector2(1f, 1f);
                    pivot = new Vector2(1f, 1f);
                    anchoredPos = new Vector2(-offset.x, -offset.y);
                    break;
            }
        
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = pivot;
            rectTransform.anchoredPosition = anchoredPos;
        }
    
        // Helper method to set start/end points programmatically
        public void SetMovementPoints(Vector2 start, Vector2 end, bool relative = true)
        {
            startPoint = start;
            endPoint = end;
            useRelativePoints = relative;
        }
    
        // Helper method to set durations
        public void SetDurations(float fadeIn, float move, float hold, float fadeOut)
        {
            fadeInDuration = fadeIn;
            moveDuration = move;
            holdAtEndDuration = hold;
            fadeOutDuration = fadeOut;
        }
    }

    public enum TickerPosition
    {
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight
    }

    public enum TickerDirection
    {
        RightToLeft,
        LeftToRight,
        TopToBottom,
        BottomToTop
    }
}