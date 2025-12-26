# Unity News Ticker System

A flexible and easy-to-use news ticker system for Unity with TextMeshPro support, featuring smooth animations, customizable positioning, and event-driven callbacks.

## Features

✨ **Smooth Coroutine-Based Animations** - No Update overhead, efficient performance  
🎯 **Customizable Start & End Points** - Full control over text movement path  
🎨 **Fade In/Out Animations** - Smooth alpha transitions with animation curves  
📍 **4 Corner Positioning** - Bottom-left, bottom-right, top-left, top-right  
🔄 **4 Movement Directions** - Right-to-left, left-to-right, top-to-bottom, bottom-to-top  
⏱️ **Hold at End Point** - Display complete text before fade out  
📢 **Event System** - Callbacks for end point reached and animation complete  
👁️ **Editor Preview** - Visualize positioning in edit mode  
🎮 **Easy Integration** - Single script attachment, auto-setup

---

## Installation

### Step 1: Import TextMeshPro
1. Open Unity
2. Go to **Window → TextMeshPro → Import TMP Essential Resources**
3. Click **Import** in the dialog

### Step 2: Add Scripts to Project
1. Create a new folder: `Assets/Scripts/NewsTicker`
2. Add the following scripts:
   - `NewsTicker.cs` (main script)
   - `NewsTickerExample.cs` (example usage - optional)

---

## Setup Guide

### Step 1: Create Canvas
1. Right-click in **Hierarchy** → **UI → Canvas**
2. Set Canvas **Render Mode** to **Screen Space - Overlay** (or Camera if needed)
3. Set Canvas **UI Scale Mode** to **Scale With Screen Size** (recommended)

### Step 2: Create Ticker GameObject
1. Right-click on **Canvas** → **UI → Panel**
2. Rename it to `NewsTicker`
3. **Delete** the default **Image** component (we only need the RectTransform)

### Step 3: Add TextMeshPro Text
1. Right-click on `NewsTicker` → **UI → Text - TextMeshPro**
2. Rename it to `TickerText`
3. Configure the text properties:
   - **Font Size**: 36 (or your preference)
   - **Alignment**: Center/Middle
   - **Color**: White (or your preference)
   - **Wrapping**: Disabled
   - **Overflow**: Overflow (important for long text)

### Step 4: Add Canvas Group (for Fade)
1. Select `NewsTicker` GameObject
2. Click **Add Component** → **Canvas Group**

### Step 5: Add Mask (Optional but Recommended)
1. With `NewsTicker` selected
2. Click **Add Component** → **UI → Mask**
3. Uncheck **Show Mask Graphic**
4. This will hide text outside the ticker bounds

### Step 6: Add NewsTicker Script
1. Select `NewsTicker` GameObject
2. Click **Add Component**
3. Search for **NewsTicker** and add it
4. The script will automatically find:
   - Canvas Group
   - RectTransform
   - TextMeshPro component (from child)

### Step 7: Configure in Inspector

#### **Positioning**
- **Position**: Choose corner (Bottom Right recommended for news tickers)
- **Offset**: Distance from screen edge (X: 20, Y: 20)

#### **Movement Points**
- **Start Point**: Where text appears (e.g., X: 1920, Y: 0 for right side)
- **End Point**: Where text finishes (e.g., X: -200, Y: 0 for left side)
- **Use Relative Points**: ✅ Checked (points relative to position)

#### **Movement Settings**
- **Direction**: Right To Left (for classic news ticker)
- **Move Duration**: 5 seconds (adjust for speed)
- **Move Curve**: Linear (or customize for easing)

#### **Fade Settings**
- **Fade In Duration**: 0.5 seconds
- **Fade Out Duration**: 0.5 seconds
- **Fade Curve**: Ease In Out (smooth fade)

#### **End Behavior**
- **Hold At End Duration**: 1 second (show complete text before fade out)

#### **References** (Auto-assigned, but verify)
- **Text Component**: Drag `TickerText` if not auto-assigned
- **Canvas Group**: Should be auto-assigned
- **Rect Transform**: Should be auto-assigned

---

## Usage Examples

### Example 1: Basic Usage
```csharp
using UnityEngine;
using NewsTicker;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NewsTicker.NewsTicker ticker;
    
    void Start()
    {
        // Show a news ticker
        ticker.ShowTicker("Welcome to the game!");
    }
}
```

### Example 2: With Events
```csharp
using UnityEngine;
using NewsTicker;

public class NewsManager : MonoBehaviour
{
    [SerializeField] private NewsTicker.NewsTicker ticker;
    
    void Start()
    {
        // Subscribe to events
        ticker.OnReachEndPoint.AddListener(OnTextAtEnd);
        ticker.OnTickerComplete.AddListener(OnAnimationDone);
        
        // Show ticker
        ticker.ShowTicker("BREAKING: Event system working!");
    }
    
    void OnTextAtEnd()
    {
        Debug.Log("Text reached end point and is fully visible!");
    }
    
    void OnAnimationDone()
    {
        Debug.Log("Animation complete, ticker hidden!");
        // Show next ticker or trigger other events
    }
}
```

### Example 3: Custom Movement Points
```csharp
using UnityEngine;
using NewsTicker;

public class CustomTicker : MonoBehaviour
{
    [SerializeField] private NewsTicker.NewsTicker ticker;
    
    public void ShowCustomTicker()
    {
        // Set custom start and end points
        ticker.SetMovementPoints(
            new Vector2(2000f, 0f),    // Start from far right
            new Vector2(-300f, 0f),    // End far left
            true                        // Relative to base position
        );
        
        // Customize timing
        ticker.SetDurations(
            fadeIn: 0.3f,      // Quick fade in
            move: 8f,          // Slow movement
            hold: 2f,          // Hold for 2 seconds
            fadeOut: 0.5f      // Normal fade out
        );
        
        ticker.ShowTicker("Custom animation settings!");
    }
}
```

### Example 4: Stop Ticker Mid-Animation
```csharp
public void StopCurrentTicker()
{
    ticker.StopTicker(); // Immediately stops and hides
}
```

### Example 5: Queue Multiple Tickers
```csharp
using System.Collections.Generic;
using UnityEngine;
using NewsTicker;

public class TickerQueue : MonoBehaviour
{
    [SerializeField] private NewsTicker.NewsTicker ticker;
    private Queue<string> newsQueue = new Queue<string>();
    private bool isPlaying = false;
    
    void Start()
    {
        ticker.OnTickerComplete.AddListener(ShowNextTicker);
    }
    
    public void AddNews(string newsText)
    {
        newsQueue.Enqueue(newsText);
        if (!isPlaying)
        {
            ShowNextTicker();
        }
    }
    
    void ShowNextTicker()
    {
        if (newsQueue.Count > 0)
        {
            isPlaying = true;
            string news = newsQueue.Dequeue();
            ticker.ShowTicker(news);
        }
        else
        {
            isPlaying = false;
        }
    }
}
```

---

## Inspector Preview Features

### Preview Position
1. Select `NewsTicker` GameObject
2. Check **Preview Position** in inspector
3. The ticker will move to its configured corner position
4. Uncheck when done to reset

### Preview Start Point
1. Check **Preview Start Point**
2. See where text will appear when animation starts
3. Uncheck to return to base position

### Preview End Point
1. Check **Preview End Point**
2. See where text will finish before fade out
3. Uncheck to return to base position

**Note**: Only one preview can be active at a time.

---

## Configuration Reference

### Position Options
- **Bottom Left**: News ticker at bottom-left corner
- **Bottom Right**: News ticker at bottom-right corner (default)
- **Top Left**: News ticker at top-left corner
- **Top Right**: News ticker at top-right corner

### Direction Options
- **Right To Left**: Classic news ticker movement →
- **Left To Right**: Reverse movement ←
- **Top To Bottom**: Vertical downward ↓
- **Bottom To Top**: Vertical upward ↑

### Animation Curves
- **Linear**: Constant speed throughout
- **Ease In Out**: Slow start, fast middle, slow end
- **Ease In**: Gradual acceleration
- **Ease Out**: Gradual deceleration
- **Custom**: Create your own curve in inspector

---

## Public Methods

### ShowTicker(string text)
Shows the ticker with specified text and starts animation.
```csharp
ticker.ShowTicker("Your message here");
```

### StopTicker()
Immediately stops current animation and hides ticker.
```csharp
ticker.StopTicker();
```

### SetMovementPoints(Vector2 start, Vector2 end, bool relative)
Programmatically set start and end points.
```csharp
ticker.SetMovementPoints(
    new Vector2(1920, 0),  // start
    new Vector2(-200, 0),  // end
    true                    // relative to position
);
```

### SetDurations(float fadeIn, float move, float hold, float fadeOut)
Customize all animation durations.
```csharp
ticker.SetDurations(0.5f, 5f, 1f, 0.5f);
```

---

## Events

### OnReachEndPoint
Triggered when text reaches end point (before hold duration).
```csharp
ticker.OnReachEndPoint.AddListener(() => {
    Debug.Log("Text at end!");
});
```

### OnTickerComplete
Triggered when entire animation completes (after fade out).
```csharp
ticker.OnTickerComplete.AddListener(() => {
    Debug.Log("Animation done!");
});
```

---

## Tips & Best Practices

### Performance
- ✅ Use coroutines (already implemented) - more efficient than Update
- ✅ Disable ticker GameObject when not in use
- ✅ Reuse single ticker instead of creating multiple

### Visual Quality
- ✅ Use TextMeshPro for crisp text at any size
- ✅ Add Canvas Group for smooth alpha fading
- ✅ Use Mask component to hide overflow text
- ✅ Set proper text overflow settings

### Movement
- ✅ Use relative points for easy repositioning
- ✅ Test with different screen resolutions
- ✅ Adjust move duration based on text length
- ✅ Use animation curves for natural movement

### Timing
- ✅ Keep fade in/out short (0.3-0.5s)
- ✅ Adjust move duration for readability
- ✅ Use hold duration to ensure text is readable
- ✅ For long text, increase move duration

---

## Troubleshooting

### Text not visible
- ✅ Check Canvas Group alpha is not stuck at 0
- ✅ Verify TextMeshPro component is assigned
- ✅ Check text color is not same as background
- ✅ Ensure GameObject is active in hierarchy

### Text not moving
- ✅ Verify start and end points are different
- ✅ Check move duration is greater than 0
- ✅ Ensure Canvas reference is found (check console)
- ✅ Try toggling Use Relative Points

### Text moving wrong direction
- ✅ Swap start and end points
- ✅ Change Direction setting in inspector
- ✅ Verify position setting matches intended corner

### Text cut off by mask
- ✅ Increase ticker GameObject size
- ✅ Adjust text size
- ✅ Check mask bounds in Scene view
- ✅ Disable mask temporarily to test

### Events not firing
- ✅ Ensure listeners are added before ShowTicker call
- ✅ Check for errors in console
- ✅ Verify animation completes (not stopped early)

---

## Requirements

- Unity 2020.3 or higher
- TextMeshPro package (installed via Package Manager)
- UI Toolkit (included in Unity)

---

## License

Free to use in personal and commercial projects.

---

## Support

For issues or questions:
1. Check the Troubleshooting section above
2. Review the example scripts
3. Test with the preview features in inspector

---

## Changelog

### Version 1.0
- Initial release
- Coroutine-based animations
- Configurable start/end points
- Fade in/out with animation curves
- Hold at end point feature
- Event system
- Editor preview tools
