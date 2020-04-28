using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    // just a scriptable object class for passing json serialized parameters as strings with the EventManager
    public class RecognizedKeyword : ScriptableObject
    {
        // recognized word as string
        public string word;
        // eg. confidence, duration, ...
    }
}