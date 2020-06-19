using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{
    #region Public
    public Camera m_Camera;
    public GameObject cube;
    public GameObject cursor;
    #endregion

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
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Vector3 VScreen = new Vector3();
            VScreen.x = Input.mousePosition.x;
            VScreen.y = Input.mousePosition.y;
            VScreen.z = Camera.main.transform.position.z;

            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                m_Cursor = Instantiate(cursor);
                m_Cursor.transform.position = Camera.main.ScreenToWorldPoint(VScreen);
            }
            if (theTouch.phase == TouchPhase.Moved)
            {
                m_Cursor.transform.position = Camera.main.ScreenToWorldPoint(VScreen);
            }
            if (theTouch.phase == TouchPhase.Ended)
            {
                Destroy(m_Cursor);
            }
        }
        stage = 5f;
        switch (dir)
        {
            case (SWIPEDIRECTION.UP):
                axis = Vector3.left;
                stage *= -1;

                break;
            case (SWIPEDIRECTION.DOWN):
                axis = Vector3.left;

                break;
            case (SWIPEDIRECTION.LEFT):
                axis = Vector3.up;
                break;
            case (SWIPEDIRECTION.RIGHT):
                axis = Vector3.up;
                stage *= -1;
                break;
            default: 
                stage =0.0001f;
                axis = Vector3.up;
                break;
        }
        cube.transform.rotation *= Quaternion.AngleAxis(stage, axis);
        if (Input.touchCount == 3)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Ended)
                cube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        }
    }
    #endregion
    #region Public Methods
    //Place your public methods here

    #endregion
    #region Private Methods
    //Place your public methods here
    private void TouchInput_OnSwipe(object sender, directionMoveArgs e)
    {
        dir = e.direction;

    }
    private void TouchInput_OnPinch(object sender, pinchMoveArgs e)
    {
        int stage = 50;
        switch(e.direction)
        {
            case (PINCHDIRECTION.IN):
                stage *= -1;
                stage += stage;
            break;
            default:break;
        }
        m_Camera.fieldOfView += stage * Time.deltaTime;
    }
    #endregion

}
