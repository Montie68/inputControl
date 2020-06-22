using System;
using UnityEngine;
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
