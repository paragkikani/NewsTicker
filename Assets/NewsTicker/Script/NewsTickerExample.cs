using System.Collections;
using TMPro;
using UnityEngine;

namespace NewsTicker.Script {
    public class NewsTickerExample : MonoBehaviour
    {
        [SerializeField] private NewsTicker newsTicker;
        [SerializeField] private TMP_Text pressKeyText;
        private void Start()
        {
            // Subscribe to events
            if (newsTicker != null)
            {
                newsTicker.OnReachEndPoint.AddListener(OnReachedEndPoint);
                newsTicker.OnTickerComplete.AddListener(OnNewsTickerCompleted);
            }
        }

        private void PressKey(KeyCode kCode)
        {
            pressKeyText.gameObject.SetActive(true);
            pressKeyText.SetText($"When Press : {kCode}");
            StartCoroutine(PressKeyCoroutine());
        }

        private IEnumerator PressKeyCoroutine()
        {
            yield return new WaitForSeconds(1f);
            pressKeyText.gameObject.SetActive(false);
        }
        
       private void Update()
        {
            // Example: Press Space to show ticker
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowBreakingNews();
                PressKey(KeyCode.Space);
            }
        
            // Example: Press R to show with custom points
            if (Input.GetKeyDown(KeyCode.R))
            {
                ShowWithCustomPoints();
                PressKey(KeyCode.R);
            }
        
            // Example: Press S to stop current ticker
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (newsTicker != null)
                {
                    newsTicker.StopTicker();
                    PressKey(KeyCode.S);
                }
            }
        }

        private void ShowBreakingNews()
        {
            if (newsTicker != null)
            {
                newsTicker.ShowTicker("BREAKING NEWS: Unity news ticker system with coroutines!");
            }
        }

        private void ShowWithCustomPoints()
        {
            if (newsTicker != null)
            {
                // Set custom movement points (relative to position)
                newsTicker.SetMovementPoints(
                    new Vector2(1500f, 0f),  // Start point
                    new Vector2(-300f, 0f),  // End point
                    true                      // Relative to position
                );
            
                // Set custom durations (fadeIn, move, hold, fadeOut)
                newsTicker.SetDurations(0.3f, 4f, 1.5f, 0.3f);
            
                newsTicker.ShowTicker("Custom timing and movement points!");
            }
        }
    
        private void OnReachedEndPoint()
        {
            Debug.Log("News ticker reached end point - showing complete text!");
        }
    
        private void OnNewsTickerCompleted()
        {
            Debug.Log("News ticker animation fully completed!");
            // You can trigger next ticker or any other action here
        }
    }
}