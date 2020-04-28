using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANR
{
    public class TORDisplay : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Canvas warningCanvas;
        [SerializeField] private ElapsedTimer elapsedTimer;

        // event listeners
        private UnityAction<string> eventListenerTOR;
        private UnityAction eventListenerTORSteer;

        #endregion

        private void Awake()
        {
            warningCanvas.enabled = false;

            // listen to events (TORArea, steering)
            eventListenerTOR = new UnityAction<string>(ShowTORText);
            eventListenerTORSteer = new UnityAction(TORSteeringUpdate);
        }

        void OnEnable()
        {
            EventManager.StartListening("TOR_AREA", eventListenerTOR);
            EventManager.StartListening("TOR_steering", eventListenerTORSteer);
        }

        void OnDisable()
        {
            EventManager.StopListening("TOR_AREA", eventListenerTOR);
            EventManager.StopListening("TOR_steering", eventListenerTORSteer);
        }

        #region EventManager

        private void ShowTORText(string jsonVars)
        {
            // use the same scriptable object when sending and receiving the same message since that was serialized
            TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
            // convert JSON string to TORMessage object
            JsonUtility.FromJsonOverwrite(jsonVars, objVars);
            if (!objVars.automate)
            {
                // only show TOR text if manual driving is required
                warningCanvas.enabled = true;

                // start elapsed timer
                elapsedTimer.StartTimer();
            }
            else
            {
                warningCanvas.enabled = false;
            }
        }

        private void TORSteeringUpdate()
        {
            // calculate elpased time from ShowTORMessage to TORSteeringUpdate
            float elapsedTime = elapsedTimer.GetElapsedTime();

            // trigger event
            SendTORSteeringEvent(elapsedTime);
        }

        #endregion

        private void SendTORSteeringEvent(float elapsedTime)
        {
            // using CreateInstance because it's a scriptable object
            TORSteeringMessage objVars = ScriptableObject.CreateInstance<TORSteeringMessage>();
            objVars.elapsedTime = elapsedTime;
            // converts to a json string, so the messge only needs one string to pass a large number of diverse parameters
            string jsonVars = JsonUtility.ToJson(objVars);
            // emits message to listeners, passing the json string as a parameter
            EventManager.TriggerEvent("TOR_steering_message", jsonVars);
        }
    }
}