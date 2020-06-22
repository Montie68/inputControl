using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo2 : MonoBehaviour
{
    #region Private
    //private members go here
    private TouchInput touchInput;
    private Touch theTouch;
    private GameObject m_Cursor;

    private SWIPEDIRECTION dir;
    private Vector3 axis;
    private float stage = 5f;

    #endregion
    // Place all unity Message Methods here like OnCollision, Update, Start ect. 
    #region Unity Messages 
    void Start()
    {
        touchInput = FindObjectOfType<TouchInput>();
        touchInput.OnSwipe += TouchInput_OnSwipe;
        touchInput.OnPinch += TouchInput_OnPinch;
    }



    void OnDisable()
    {
        touchInput.OnSwipe -= TouchInput_OnSwipe;
        touchInput.OnPinch -= TouchInput_OnPinch;
    }
        // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    private void TouchInput_OnPinch(object sender, pinchMoveArgs e)
    {
        Debug.Log("Pinch: " + e.direction);
    }

    private void TouchInput_OnSwipe(object sender, directionMoveArgs e)
    {
        Debug.Log("Swiper: " + e.direction);

    }
}
