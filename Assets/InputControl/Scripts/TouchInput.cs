using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : InputControl
{

    #region Public
    //public members go here
    public event EventHandler OnSwipe;
    public event EventHandler OnPress;
    #endregion

    #region Private
    //private members go here
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = .5f;
    #endregion
    // Place all unity Message Methods here like OnCollision, Update, Start ect. 
    #region Unity Messages 
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            Debug.Log( theTouch.phase.ToString());

            if (theTouch.phase == TouchPhase.Ended)
            {
                timeTouchEnded = Time.time;
            }
        }

        else if (Time.time - timeTouchEnded > displayTime)
        {
            OnPress?.Invoke(this, EventArgs.Empty);
        }

        base.Update();
    }
  #endregion
  #region Public Methods
	//Place your public methods here

  #endregion
  #region Private Methods
	//Place your public methods here

  #endregion

}
