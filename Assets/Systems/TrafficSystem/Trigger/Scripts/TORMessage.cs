using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    // just a scriptable object class for passing json serialized parameters as strings with the EventManager
    public class TORMessage : ScriptableObject
    {
        // should automate
        public bool automate;
        // eg. timestamp, ...
    }
}