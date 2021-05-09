using System;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    [SerializeField]
    private float minimumDistance = 10f;
    private Vector2 swipeStart;
    private Vector2 swipeEnd;    

    public static event Action<SwipeDirection> OnSwipe;

    public enum SwipeDirection
    {
        Up, Down, Left, Right
    };

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                ProcessSwipe();
            }
        }

        // mouse touch simulation
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            ProcessSwipe();
        }
    }

    private void ProcessSwipe()
    {
        if (Vector2.Distance(swipeStart, swipeEnd) < minimumDistance)
        {
            return;
        }

        if (isVerticalSwipe())
        {
            if (swipeEnd.y > swipeStart.y)
            {
                OnSwipe?.Invoke(SwipeDirection.Up);
            }
            else
            {
                OnSwipe?.Invoke(SwipeDirection.Down);
            }
        }
        else
        {
            if (swipeEnd.x > swipeStart.x)
            {
                OnSwipe?.Invoke(SwipeDirection.Right);
            }
            else
            {
                OnSwipe?.Invoke(SwipeDirection.Left);
            }
        }
    }

    private bool isVerticalSwipe()
    {
        float distanceY = Mathf.Abs(swipeEnd.y - swipeStart.y);
        float distanceX = Mathf.Abs(swipeEnd.x - swipeStart.x);

        if (distanceY > distanceX)
        {
            return true;
        }
        return false;
    }
}
