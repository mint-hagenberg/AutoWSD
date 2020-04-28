using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    // just a scriptable object class for passing json serialized parameters as strings with the EventManager
    public class StartMessage : ScriptableObject
    {
        // flag if the start area is being started or finished
        public bool isStart;
        // eg. timestamp, ...
    }
}