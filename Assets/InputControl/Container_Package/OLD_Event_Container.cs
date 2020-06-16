using System;

//--------------//
// Package by DR
//--------------//
/// This is a container class where you can house all of the events in your project.
/// It's pretty simple to use and makes it super easy for the devs.
/// To Use: Simply copy & paste the example and rename to suit your needs.
//--------------//
/*
    public static event Action EventName;
    public static void FunctionName()
    {
        EventName?.Invoke();
    }
*/
//--------------//

public static class OLD_Event_Container
{
    public static event Action EventName; // <-- This is what the listener adds to.
    public static void FunctionName() // <-- This is what you run to trigger events.
    {
        EventName?.Invoke();
    }

    // Add Events Here.

}
