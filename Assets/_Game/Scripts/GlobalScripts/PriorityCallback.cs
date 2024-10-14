using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityCallback
{
    public int Priority;
    public object Callback;
    public PriorityCallback(int priority, object callback)
    {
        Priority = priority; 
        Callback = callback;
    }

}
