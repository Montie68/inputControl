using System;
using UnityEngine;

public class pinchMoveArgs : EventArgs
{
    // public getter of the direction
    public PINCHDIRECTION direction
    {
        get
        {
            return _direction;
        }
    }
    // vectors to hold the touch locations
    Vector2[] firstTouch =  new Vector2[2];
    Vector2[] lastTouch = new Vector2[2];

    // direction of the swipe
    private PINCHDIRECTION _direction;

    public pinchMoveArgs(locationMoveData[] moveData)
    {
        // assign the vectors from the passed movedate to the local vectors
        firstTouch[0] = moveData[0].firstTouch;
        lastTouch[0] = moveData[0].lastTouch;
        firstTouch[1] = moveData[1].firstTouch;
        lastTouch[1] = moveData[1].lastTouch;
        float firstMag, lastMag;

            firstMag = Vector2.Distance(firstTouch[0], firstTouch[1]);
            lastMag = Vector2.Distance(lastTouch[0], lastTouch[1]);

        // pinch distance test
        if (firstMag > lastMag)
        {
            _direction = PINCHDIRECTION.IN;
        }
        else if (lastMag > firstMag)
        {
            _direction = PINCHDIRECTION.OUT;
        }
    }

    public void Rest()
    {
        _direction = PINCHDIRECTION.NONE;
    }
}