using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    // just a scriptable object class for passing json serialized parameters as strings with the EventManager
    public class TORSteeringMessage : ScriptableObject
    {
        // how long it took the user to use the steering wheel
        public float elapsedTime;
    }
}