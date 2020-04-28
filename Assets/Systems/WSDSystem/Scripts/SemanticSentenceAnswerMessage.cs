using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    // just a scriptable object class for passing json serialized parameters as strings with the EventManager
    public class SemanticSentenceAnswerMessage : ScriptableObject
    {
        // if answer is correct
        public bool correct;

        // how long it took the user to give an answer
        public float elapsedTime;
    }
}