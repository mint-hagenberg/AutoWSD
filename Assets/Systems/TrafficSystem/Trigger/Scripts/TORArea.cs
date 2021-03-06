﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    public class TORArea : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("TORArea: OnTriggerEnter");

            // fire event
            if (other.gameObject.CompareTag("PlayerCarCollider"))
            {
                // using CreateInstance because it's a scriptable object
                TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
                objVars.automate = false;
                // converts to a json string, so the message only needs one string to pass a large number of diverse parameters
                string jsonVars = JsonUtility.ToJson(objVars);
                // emits message to listeners, passing the json string as a parameter
                EventManager.TriggerEvent("TOR_AREA", jsonVars);
            }
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log("TORArea: OnTriggerExit");

            // fire event
            if (other.gameObject.CompareTag("PlayerCarCollider"))
            {
                // using CreateInstance because it's a scriptable object
                TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
                objVars.automate = true;
                // converts to a json string, so the messge only needs one string to pass a large number of diverse parameters
                string jsonVars = JsonUtility.ToJson(objVars);
                // emits message to listeners, passing the json string as a parameter
                EventManager.TriggerEvent("TOR_AREA", jsonVars);
            }
        }
    }
}