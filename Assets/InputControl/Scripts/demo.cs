using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{
    #region Public
    public Camera m_Camera;
    public GameObject cube;
    #endregion

    #region Private
    //private members go here
    private TouchInput touchInput;
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
    #endregion
    #region Public Methods
    //Place your public methods here

    #endregion
    #region Private Methods
    //Place your public methods here
    private void TouchInput_OnSwipe(object sender, directionMoveArgs e)
    {
        float stage = 45f;
        switch (e.direction)
        {
            case (SWIPEDIRECTION.UP):
                cube.transform.rotation *= Quaternion.AngleAxis(stage, Vector3.up);
                break;
            case (SWIPEDIRECTION.DOWN):
                cube.transform.rotation *= Quaternion.AngleAxis(stage, Vector3.down);

                break;
            case (SWIPEDIRECTION.LEFT):
                cube.transform.rotation *= Quaternion.AngleAxis(stage, Vector3.left);

                break;
            case (SWIPEDIRECTION.RIGHT):
                cube.transform.rotation *= Quaternion.AngleAxis(stage, Vector3.right);

                break;
            default: break;
        }
    }
    private void TouchInput_OnPinch(object sender, pinchMoveArgs e)
    {
        int stage = 10;
        switch(e.direction)
        {
            case (PINCHDIRECTION.IN):
                stage *= -1;
            break;
            default:break;
        }
        m_Camera.fieldOfView += stage;
    }
    #endregion

}
