using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANR
{
    public class AutomaticOrManualDrive : MonoBehaviour
    {
        #region Properties

        [SerializeField] private bool isAutomatic = true;
        [SerializeField] private CarAIControl carAIControl;
        [SerializeField] private LogitechSteeringManager logitechSteeringManager;

        // event listeners
        private UnityAction<string> eventListenerTOR;

        #endregion

        private void Awake()
        {
            // listen to events (TORArea)
            eventListenerTOR = new UnityAction<string>(ToggleAutomaticManualDrive);
        }

        // Update is called once per frame
        void Update()
        {
            if (isAutomatic)
            {
                carAIControl.enabled = true;
                logitechSteeringManager.enabled = false;
            }
            else
            {
                carAIControl.enabled = false;
                logitechSteeringManager.enabled = true;
            }
        }

        void OnEnable()
        {
            EventManager.StartListening("TOR_AREA", eventListenerTOR);
        }

        void OnDisable()
        {
            EventManager.StopListening("TOR_AREA", eventListenerTOR);
        }

        #region EventManager

        private void ToggleAutomaticManualDrive(string jsonVars)
        {
            // use the same scriptable object when sending and receiving the same message since that was serialized
            TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
            // convert JSON string to TORMessage object
            JsonUtility.FromJsonOverwrite(jsonVars, objVars);
            isAutomatic = objVars.automate;
        }

        #endregion
    }
}