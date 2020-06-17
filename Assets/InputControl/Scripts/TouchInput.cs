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

    public float minimumSwipeDistance = 1f;

    // enum of swipe direction[
    // [HideInInspector]
    public SWIPEDIRECTION swipeDirection = SWIPEDIRECTION.NONE;
  #endregion

  #region Private
    //private members go here
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = .5f;

    private locationMoveData moveData = new locationMoveData();

  #endregion
    // Place all unity Message Methods here like OnCollision, Update, Start ect. 
  #region Unity Messages 
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {

        Vector3 VScreen = new Vector3();

        VScreen.x = Input.mousePosition.x;
        VScreen.y = Input.mousePosition.y;
        VScreen.z = Camera.main.transform.position.z;


        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            Debug.Log( theTouch.phase.ToString());
            if (theTouch.phase == TouchPhase.Began)
            {
                moveData.firstTouch = Camera.main.ScreenToWorldPoint(VScreen);
            }
            if (theTouch.phase == TouchPhase.Ended)
            {
                moveData.lastTouch = Camera.main.ScreenToWorldPoint(VScreen);

                timeTouchEnded = Time.time;
                float distance = Vector3.Distance(moveData.firstTouch, moveData.lastTouch);
                if (distance > minimumSwipeDistance)
                {
                    OnSwipe?.Invoke(this, new directionMoveArgs(moveData));
                    moveData.zero();

                }
                else if (Time.time - timeTouchEnded < displayTime)
                {
                    OnPress?.Invoke(this, EventArgs.Empty);
                }

            }


        }
     /*   if (Input.GetMouseButtonDown(0))
        {
            moveData.firstTouch = Camera.main.ScreenToWorldPoint(VScreen);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveData.lastTouch = Camera.main.ScreenToWorldPoint(VScreen);

        }
        float dist = Vector3.Distance(moveData.firstTouch, moveData.lastTouch);
        if (dist > minimumSwipeDistance)
        {
            OnSwipe?.Invoke(this, new directionMoveArgs(moveData));
            moveData.zero();

        }*/

        base.Update();
    }
    #endregion

    #region Touch Subscription
    /**********************************************************/
    // Add this block to your custom swipe handler scripts //
    void OnEnable()
    {
        OnPress += Touch_OnPress;
        OnSwipe += Touch_OnSwipe;
    }
    void OnDisable()
    {
        OnPress -= Touch_OnPress;
        OnSwipe -= Touch_OnSwipe;
    }
    //Place your public methods here
    public void Touch_OnPress(object sender, EventArgs e)
    {
        Debug.Log("Touch Press!");
    }
    public void Touch_OnSwipe(object sender, directionMoveArgs e)
    {
        Debug.Log("Touch Swipe! " + e.direction);
        swipeDirection = e.direction;
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

    #endregion

}

public enum SWIPEDIRECTION
{
    NONE = default,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

[Serializable]
public class locationMoveData
{
    public Vector2 firstTouch;
    public Vector2 lastTouch;
    public locationMoveData()
    {
        zero();
    }

    public void zero()
    {
        firstTouch = Vector2.zero;
        lastTouch = Vector2.zero;
    }
}

public class directionMoveArgs : EventArgs
{

    public SWIPEDIRECTION direction
    {
        get
        {
            return _direction;
        }
    }

    Vector2 firstTouch;
    Vector2 lastTouch;

    private readonly SWIPEDIRECTION _direction;

    public directionMoveArgs(locationMoveData moveData)
    {
        firstTouch = moveData.firstTouch;
        lastTouch = moveData.lastTouch;
        Vector2 xMag = new Vector2(firstTouch.x , lastTouch.x);
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