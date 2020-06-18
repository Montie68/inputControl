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
    public event EventHandler OnPress;
    // minimum deley to register the touch as a press.
    public float touchPressTime = .5f;
    // the distance the fingers needs to move to clasifies as a swipe.
    public float minimumSwipeDistance = 1f;
    // deley before restting the swipe direction
    public float swipeDeley = 0f;

    [Header("Debug Settings")]
    public bool mouseTesting = false;
    [Header("* Remember to disable before building to device")]
    // [HideInInspector]
    public SWIPEDIRECTION swipeDirection = SWIPEDIRECTION.NONE;
  #endregion

  #region Private
    //private members go here
    private Touch theTouch;
    private float timeTouchEnded;

    private locationMoveData moveData = new locationMoveData();

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

        // this is used convert mous pos to world pos
        Vector3 VScreen = new Vector3();
        VScreen.x = Input.mousePosition.x;
        VScreen.y = Input.mousePosition.y;
        VScreen.z = Camera.main.transform.position.z;

        if (Input.touchCount == 1 )
        {
            // get the touch data if one finger is touching the screen
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                // get the touch began and store the touch location

                moveData.firstTouch = Camera.main.ScreenToWorldPoint(VScreen); 
            }
            if (theTouch.phase == TouchPhase.Ended)
            {
                // get the touch Ended and store the touch location

                moveData.lastTouch = Camera.main.ScreenToWorldPoint(VScreen); 
                // get the time the the touch ended
                timeTouchEnded = Time.time;
                // get the distance the finger moved between the first and last touch.
                float distance = Vector3.Distance(moveData.firstTouch, moveData.lastTouch);
                // check if it moved over the mimimum distance
                if (distance > minimumSwipeDistance)
                {
                    // then invoke the Onswipe event.
                    OnSwipe?.Invoke(this, new directionMoveArgs(moveData));
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
        if (mouseTesting) MouseDebug(VScreen);
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
    }
    void OnDisable()
    {
        // unsubscribers to the touch events
        OnPress -= Touch_OnPress;
        OnSwipe -= Touch_OnSwipe;
    }
    //Place your public methods here
    public void Touch_OnPress(object sender, EventArgs e)
    {
        // add logic to handle the touch press here
        Debug.Log("Touch Press! " + theTouch.position);
       
    }
    public void Touch_OnSwipe(object sender, directionMoveArgs e)
    {
        // add logic to handle the touch swipe here
        Debug.Log("Touch Swipe! " + e.direction);
        swipeDirection = e.direction;
        // the time deley before resetting the swipeDirection -1 doesn't reset the swipe
        if (swipeDeley > -1) StartCoroutine(SwipeRest());
    }

    public IEnumerator SwipeRest()
    {
        // wait before resetting the swipedirection 
        yield return new WaitForSeconds(swipeDeley);
        swipeDirection = SWIPEDIRECTION.NONE;
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
    private void MouseDebug(Vector2 VScreen)
    {


       if (Input.GetMouseButtonDown(0))
            {
            // get the mouse down and store the touch location
                moveData.firstTouch = Camera.main.ScreenToWorldPoint(VScreen);

            }
            else if (Input.GetMouseButtonUp(0))
            {
            // get the mouse up and store the touch location

            moveData.lastTouch = Camera.main.ScreenToWorldPoint(VScreen);

            }
       // get the distance between the first touch and the last touch
        float dist = Vector3.Distance(moveData.firstTouch, moveData.lastTouch);
        // if touch is greater than the minimum distance to classify as a swipe 
        if (dist > minimumSwipeDistance)
        {
            // then invoke the Onswipe event.
            OnSwipe?.Invoke(this, new directionMoveArgs(moveData));
            // zero data
            moveData.zero();

        }


    }

    #endregion

}

// enum of swipe direction
public enum SWIPEDIRECTION
{
    NONE = default,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}
// class to store the first and last touch points then 
[Serializable]
public class locationMoveData
{
    public Vector2 firstTouch;
    public Vector2 lastTouch;
    public locationMoveData()
    {
        // zero the data on new 
        zero();
    }

    // zero the touch vectors when called
    public void zero()
    {
        firstTouch = Vector2.zero;
        lastTouch = Vector2.zero;
    }
}

public class directionMoveArgs : EventArgs
{
    // public getter of the direction
    public SWIPEDIRECTION direction
    {
        get
        {
            return _direction;
        }
    }
    // vectors to hold the touch locations
    Vector2 firstTouch;
    Vector2 lastTouch;
    // direction of the swipe
    private readonly SWIPEDIRECTION _direction;

    public directionMoveArgs(locationMoveData moveData)
    {
        // assign the vectors from the passed movedate to the local vectors
        firstTouch = moveData.firstTouch;
        lastTouch = moveData.lastTouch;
        // create the vector of the first and last x coordinates
        Vector2 xMag = new Vector2(firstTouch.x , lastTouch.x);
        // create the vector of the first and last y coordinates
        Vector2 yMag = new Vector2(firstTouch.y , lastTouch.y);

        // horizontal test
        if (xMag.magnitude > yMag.magnitude)
        {
            // left
            if (firstTouch.x < lastTouch.x)
            {
                _direction = SWIPEDIRECTION.LEFT;
            }
            //or right
            if (firstTouch.x > lastTouch.x)
            {
                _direction = SWIPEDIRECTION.RIGHT;
            }
        }
        // vertical test
        else if (xMag.magnitude < yMag.magnitude)
        {
            // up 
            if (firstTouch.y > lastTouch.y)
            {
                _direction = SWIPEDIRECTION.UP;
            }
            // or Down
            if (firstTouch.y < lastTouch.y)
            {
                _direction = SWIPEDIRECTION.DOWN;
            }
        }

    }


}