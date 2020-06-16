using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------//
// Package by DR
//--------------//
/// This is a container class where you can house all of the events in your project.
/// It's pretty simple to use and makes it super easy for the devs.
/// To Use: Simply copy & paste the example and rename to suit your needs.
//--------------//

/*
    [System.Serializable]
    public class EventName : UnityEngine.Events.UnityEvent
    { 
    
    }
*/

/// Or, if you want to pass through variables...

/*
    [System.Serializable]
    public class EventName : UnityEngine.Events.UnityEvent<var>
    { 
    
    }
*/

namespace IgnitionImmersive.Events
{
    [System.Serializable]
    public class EventName : UnityEngine.Events.UnityEvent
    {

    }

    // Add events in here
}
