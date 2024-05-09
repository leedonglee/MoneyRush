using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2, TouchPhase> OnInputEvent;

    bool _isMobile = false;

    void Start()
    {
        _isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void Update()
    {
        if (_isMobile)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    OnInputEvent?.Invoke(touch.position, touch.phase);
                }
            }
        }
        else
        {
            TouchPhase touchPhase = TouchPhase.Canceled;

            if (Input.GetMouseButtonDown(0))
            {
                touchPhase = TouchPhase.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                touchPhase = TouchPhase.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touchPhase = TouchPhase.Ended;
            }

            if (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Ended)
            {
                if (!IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y))
                {
                    OnInputEvent?.Invoke(Input.mousePosition, touchPhase);
                }
            }
        }
    }

    bool IsPointerOverUIObject(float x, float y)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(x, y)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

}