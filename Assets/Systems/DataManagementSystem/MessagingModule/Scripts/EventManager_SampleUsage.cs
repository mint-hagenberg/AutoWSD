using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANR {

    public class EventManager_SampleUsage : MonoBehaviour {

        #region Properties

        [SerializeField]
        private Canvas warningCanvas;

        // event listeners
        private UnityAction<string> eventListenerTOR;

        #endregion

        private void Awake() {
            warningCanvas.enabled = false;

            // listen to events (TORArea)
            eventListenerTOR = new UnityAction<string>(ShowTORText);
        }

        void OnEnable() {
            EventManager.StartListening("TOR_AREA", eventListenerTOR);
        }

        void OnDisable() {
            EventManager.StopListening("TOR_AREA", eventListenerTOR);
        }

        #region EventManager

        private void ShowTORText(string jsonVars) {
            // use the same scriptable object when sending and receiving the same message since that was serialized
            TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
            // convert JSON string to TORMessage object
            JsonUtility.FromJsonOverwrite(jsonVars, objVars);
            if (!objVars.automate) {
                // only show TOR text if manual driving is required
                warningCanvas.enabled = true;
            } else {
                warningCanvas.enabled = false;
            }
        }

        #endregion
    }
}
