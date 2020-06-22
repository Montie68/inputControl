using System;
using UnityEngine;

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

    public directionMoveArgs(locationMoveData moveData, bool isCardinal = true )
    {
        // assign the vectors from the passed movedate to the local vectors
        firstTouch = moveData.firstTouch;
        lastTouch = moveData.lastTouch;

        float angle = calcAngle();
        if (isCardinal) _direction = CardinalDirections(angle);
        else _direction = OrdinalDirections(angle);
    }

    private SWIPEDIRECTION CardinalDirections(float angle)
    {
        if ((angle >= 0 && angle < 45) || (angle <= 360 && angle > 315))
        {
            return SWIPEDIRECTION.RIGHT;
        }
        else if (angle >= 45 && angle < 135)
        {
            return SWIPEDIRECTION.UP;
        }
        else if (angle >= 135 && angle < 225)
        {
            return SWIPEDIRECTION.LEFT;
        }
        else if (angle >= 225 && angle <= 315)
        {
            return SWIPEDIRECTION.DOWN;
        }
        return SWIPEDIRECTION.NONE;
    }
    private SWIPEDIRECTION OrdinalDirections(float angle)
    {
        if ((angle >= 0 && angle < 22.5f) || (angle <= 360 && angle > 337.5f))
        {
            return SWIPEDIRECTION.RIGHT;
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            return SWIPEDIRECTION.UP_RIGHT;
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            return SWIPEDIRECTION.UP;
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            return SWIPEDIRECTION.UP_LEFT;
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            return SWIPEDIRECTION.LEFT;
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            return SWIPEDIRECTION.DOWN_LEFT;
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            return SWIPEDIRECTION.DOWN;
        }
        else if (angle >= 292.5f && angle <= 337.5f)
        {
            return SWIPEDIRECTION.DOWN_RIGHT;
        }
        return SWIPEDIRECTION.NONE;
    }
    private float calcAngle()
    {
        Vector2 direction = lastTouch - firstTouch;
        float Angle = 0;

        if (direction.x == 0) direction.x = 0.001f;
        Angle = Mathf.Atan(direction.x / direction.y) * Mathf.Rad2Deg;


        // using trig 0 deg is x+ y0, sp 90deg is x0 y+ etc. so goes counter-clockwise
        if (direction.x >= 0 && direction.y >= 0)
        {
            // first quadrant use any sign
            Angle = Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg;

        }
        else if (direction.x < 0 && direction.y >= 0)
        {
            Angle = Mathf.Atan(direction.x / direction.y) * Mathf.Rad2Deg;

            // second quadrant sin is positve
            Angle = -1 * Angle + 90;

        }
        else if (direction.x < 0 && direction.y < 0)
        {
            Angle = Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg;

            // third quadrant tan is positive
            Angle += 180;

        }
        else if (direction.x >= 0 && direction.y < 0)
        {
            Angle = Mathf.Atan(direction.x / direction.y) * Mathf.Rad2Deg;

            // fourth quadrant cos is positive
            Angle = -1 * Angle + 270;

        }
        return Angle;
    }
}
