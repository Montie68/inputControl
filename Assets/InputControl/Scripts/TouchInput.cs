using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TouchInput : InputControl
{

    #region Public
    //public members go here
    
    public event EventHandler<directionMoveArgs> OnSwipe;
    public event EventHandler<pinchMoveArgs> OnPinch;
    public event EventHandler OnPress;
    // minimum deley to register the touch as a press.
    public float touchPressTime = .5f;
    // the distance the fingers needs to move to clasifies as a swipe.
    public float minimumSwipeDistance = 1f;
    // the distance the fingers needs to move to clasifies as a pinch.
    public float minimumPinchDistance = 1f;
    // deley before restting the swipe direction
    public float swipeDeley = 0f;
    public bool useCardinalDirectionForSwipe = true;
    [Header("Debug Settings")]
    public bool mouseTesting = false;
    [Header("* Remember to disable before building to device")]
    // [HideInInspector]
    public SWIPEDIRECTION swipeDirection = SWIPEDIRECTION.NONE;
    #endregion

    #region Private
    //private members go here
    private Touch theTouch;
    private Touch[] theTouchs = new Touch[2];
    private float timeTouchEnded;

    private locationMoveData moveData = new locationMoveData();
    private locationMoveData[] pinchData = { new locationMoveData(), new locationMoveData() };

    private bool pinchHasFinished = true;
    #endregion
    // Place all unity Message Methods here like OnCollision, Update, Start ect. 
    #region Unity Messages 
    public override void Start()
    {
        // base class start logic
        base.Start();
    }

    public override void Update()
    {


        if (Input.touchCount == 2)
        {
            theTouchs[0] = Input.GetTouch(0);

            theTouchs[1] = Input.GetTouch(1);

            if (theTouchs[0].phase == TouchPhase.Began)
            {
                pinchHasFinished = false;
                // get the touch began and store the touch location
                pinchData[0].firstTouch = theTouchs[0].position;
            }
            if (theTouchs[1].phase == TouchPhase.Began)
            {
                pinchData[1].firstTouch = theTouchs[1].position;
            }
            if (theTouchs[0].phase == TouchPhase.Moved || theTouchs[1].phase == TouchPhase.Moved)
            {
                Vector2 pos = theTouchs[0].position;
                pinchData[0].lastTouch = pos;

                pinchData[1].lastTouch = theTouchs[1].position;

                float distance = Vector3.Distance(pinchData[0].firstTouch,  pinchData[0].lastTouch);
                float distance2 = Vector3.Distance(pinchData[1].firstTouch, pinchData[1].lastTouch);

                if (distance > minimumPinchDistance && distance2 > minimumPinchDistance)
                {  // then invoke the Onswipe event.
                    OnPinch?.Invoke(this, new pinchMoveArgs(pinchData));
                }
            }
            else if (theTouchs[0].phase == TouchPhase.Stationary && theTouchs[1].phase == TouchPhase.Stationary)
            {
                theTouchs[0].phase = TouchPhase.Ended;
                theTouchs[1].phase = TouchPhase.Ended;
            }
            // zero data
            else if (theTouchs[0].phase == TouchPhase.Ended || theTouchs[1].phase == TouchPhase.Ended)
            { 
                pinchData[0].zero();
                pinchData[1].zero();
                return;
            }

        }
        else if(Input.touchCount == 1 && pinchHasFinished)
        {
            // get the touch data if one finger is touching the screen
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                // get the touch began and store the touch location

                moveData.firstTouch = theTouch.position; // Camera.main.ScreenToWorldPoint(VScreen); 
            }
            if (theTouch.phase == TouchPhase.Ended)
            {
                // get the touch Ended and store the touch location

                moveData.lastTouch = theTouch.position; // Camera.main.ScreenToWorldPoint(VScreen); 
                // get the time the the touch ended
                timeTouchEnded = Time.time;
                // get the distance the finger moved between the first and last touch.
                float distance = Vector3.Distance(moveData.firstTouch, moveData.lastTouch);
                // check if it moved over the mimimum distance
                if (distance > minimumSwipeDistance)
                {
                    // then invoke the Onswipe event.
                    OnSwipe?.Invoke(this, new directionMoveArgs(moveData, useCardinalDirectionForSwipe));
                    // zero data
                    moveData.zero();

                }
                else if (Time.time - timeTouchEnded < touchPressTime)
                {
                    // if the not a swipe
                    OnPress?.Invoke(this, EventArgs.Empty);
                }

            }


        }
         
        // mouse debuging
        if (mouseTesting) MouseDebug();
        base.Update();

    }

    #endregion

    #region Touch Subscription
    /**********************************************************/
    // Add this block to your custom swipe handler scripts //
    void OnEnable()
    {
        // subscribers to the touch events
        OnPress += Touch_OnPress;
        OnSwipe += Touch_OnSwipe;
        OnPinch += Touch_Pinch;
    }
    void OnDisable()
    {
        // unsubscribers to the touch events
        OnPress -= Touch_OnPress;
        OnSwipe -= Touch_OnSwipe;
        OnPinch -= Touch_Pinch;

    }
    //Place your public methods here
    public void Touch_OnPress(object sender, EventArgs e)
    {
        // add logic to handle the touch press here
       
    }
    public void Touch_OnSwipe(object sender, directionMoveArgs e)
    {
        // add logic to handle the touch swipe here
        swipeDirection = e.direction;
        // the time deley before resetting the swipeDirection -1 doesn't reset the swipe
        if (swipeDeley > -1) StartCoroutine(SwipeRest());
    }
    public void Touch_Pinch(object sender, pinchMoveArgs e)
    {
        StartCoroutine(PinchReset(e));
    }
    public IEnumerator SwipeRest()
    {
        // wait before resetting the swipedirection 
        yield return new WaitForSeconds(swipeDeley);
        swipeDirection = SWIPEDIRECTION.NONE;
    }
    public IEnumerator PinchReset(pinchMoveArgs e)
    {
        // wait before resetting the swipedirection 
        yield return new WaitForSeconds(swipeDeley);
        e.Rest();
        pinchHasFinished = true;
    }
    #endregion
    /**********************************************************/

    #region Public Methods

    public SWIPEDIRECTION GetSwipe()
    {
        return swipeDirection;
    }
    #endregion

    #region Private Methods
    //Place your public methods here
    private void MouseDebug()
    {

        // this is used convert touch pos to world pos
        Vector3 VScreen = new Vector3();
        VScreen.x = Input.mousePosition.x;
        VScreen.y = Input.mousePosition.y;
        VScreen.z = Camera.main.transform.position.z;

        if (Input.GetMouseButtonDown(0))
            {
            // get the mouse down and store the touch location
                moveData.firstTouch = Camera.main.ScreenToWorldPoint(VScreen) * -1;

            }
            else if (Input.GetMouseButtonUp(0))
            {
            // get the mouse up and store the touch location

            moveData.lastTouch = Camera.main.ScreenToWorldPoint(VScreen) * -1; 

            }
       // get the distance between the first touch and the last touch
        float dist = Vector3.Distance(moveData.firstTouch, moveData.lastTouch);
        
        // if touch is greater than the minimum distance to classify as a swipe 
        if (dist > minimumSwipeDistance/50 && moveData.lastTouch != Vector2.zero)
        {
            // then invoke the Onswipe event.
            OnSwipe?.Invoke(this, new directionMoveArgs(moveData, useCardinalDirectionForSwipe));
            // zero data
            moveData.zero();
        }


    }

    #endregion

}