using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    public class FinishArea : MonoBehaviour
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
            //Debug.Log("FinishArea: OnTriggerEnter");

            // fire event
            if (other.gameObject.CompareTag("PlayerCarCollider"))
            {
                // using CreateInstance because it's a scriptable object
                FinishMessage objVars = ScriptableObject.CreateInstance<FinishMessage>();
                objVars.isStart = true;
                // converts to a json string, so the messge only needs one string to pass a large number of diverse parameters
                string jsonVars = JsonUtility.ToJson(objVars);
                // emits message to listeners, passing the json string as a parameter
                EventManager.TriggerEvent("FINISH_AREA", jsonVars);
            }
        }

        // currently not used
        /*
        void OnTriggerExit(Collider other) {
            Debug.Log("FinishArea: OnTriggerExit");

            // fire event
            if (other.gameObject.CompareTag("PlayerCarCollider")) {
                // using CreateInstance because it's a scriptable object
                FinishMessage objVars = ScriptableObject.CreateInstance<FinishMessage>();
                objVars.isStart = false;
                // converts to a json string, so the messge only needs one string to pass a large number of diverse parameters
                string jsonVars = JsonUtility.ToJson(objVars);
                // emits message to listeners, passing the json string as a parameter
                EventManager.TriggerEvent("FINISH_AREA", jsonVars);
            }
        }
        */
    }
}