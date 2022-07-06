using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swiper : MonoBehaviour
{
    private Vector3 _firstTouch;
    private bool _isSwiping;
    private int _screenWidth;

    private void Awake()
    {
        _screenWidth = Screen.width;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isSwiping = true;
            _firstTouch = Input.mousePosition;
        }
        if (!_isSwiping)
            return;
        if(Mathf.Abs(Input.mousePosition.x - _firstTouch.x) > _screenWidth / 9.5f)
        {
            _isSwiping = false;
            EventsPool.UserSwipedEvent.Invoke(Input.mousePosition.x > _firstTouch.x ? AttackDirection.Right : AttackDirection.Left);
        }
    }
}
