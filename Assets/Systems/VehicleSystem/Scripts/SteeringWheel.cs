using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANR
{
    public class SteeringWheel : MonoBehaviour
    {
        #region Properties

        // maximum steer angle in degrees
        [SerializeField] private float maxAngle = 45;

        // flag if vehicle is automated or manually driven
        [SerializeField] private bool isAutomatic = true;

        // flag if keyboard keys should be used for determining the steering wheel angle
        [SerializeField] private bool useKeyboard = false;

        // wheel collider as reference for the steering angle
        [SerializeField] public WheelCollider wheelCollider;

        // current steering wheel angle in degrees
        private float currentAngle = 0;

        // automatic driving using wheel collider as reference
        float targetAngle = 0.0f;

        // makes the interpolation faster when the input is pressed down
        private float interpolationSpeed = 0.05f;

        // event listeners
        private UnityAction<string> eventListenerTOR;

        // flag if steer info should be sent via EventManager
        private bool shouldSendSteer = false;

        private int currentForce = 0;

        #endregion

        private void Awake()
        {
            // listen to events (from TORArea)
            eventListenerTOR = new UnityAction<string>(StartAutomatedDrive);
        }

        void Start()
        {
            Debug.Log("Logitech Steering Wheel Init:" + LogitechGSDK.LogiSteeringInitialize(false));
            Debug.Log("Logitech Steering Wheel Connected: " + LogitechGSDK.LogiIsConnected(0));
            LogitechGSDK.LogiPlayConstantForce(0, 0);
        }

        void Update()
        {
            // check if vehicle is automated or manually driven
            if (isAutomatic)
            {
                // automatic driving using wheel collider as reference
                targetAngle = wheelCollider.steerAngle * 1.0f;
                // makes the interpolation faster when the input is pressed down
                interpolationSpeed = 0.05f;
                // smoothly sets the current angle based on the input
                currentAngle = Mathf.Lerp(currentAngle, targetAngle, interpolationSpeed);
                // sets the steering wheel angle
                transform.localEulerAngles = Vector3.down * Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

                if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
                {
                    if (Math.Abs(currentAngle - targetAngle) < 0.001f)
                    {
                        LogitechGSDK.LogiStopConstantForce(0);
                    }
                    else
                    {
                        // rotate steering wheel (< 0: rotate cw, > 0: rotate ccw)
                        // approx. force necessary to turn the steering wheel
                        int force = (int) ((targetAngle * 2 - currentAngle * 2) * -2.0f) + (targetAngle < 0 ? 6 : -6);
                        //Debug.Log("force: " + force);
                        LogitechGSDK.LogiPlayConstantForce(0, force);
                        currentForce = force;
                    }
                }
            }
            else
            {
                // manual driving with keyboard keys / logitech steering manager
                // wheel effects
                LogitechGSDK.LogiPlayBumpyRoadEffect(0, 10);
                //LogitechGSDK.LogiPlayDirtRoadEffect(0, 25);

                targetAngle = wheelCollider.steerAngle * 1.0f;
                interpolationSpeed = useKeyboard ? 0.15f : 0.10f;
                currentAngle = Mathf.Lerp(currentAngle, targetAngle, interpolationSpeed);
                float manualInput = (useKeyboard ? Input.GetAxis("Horizontal") : Input.GetAxis("Steering")) * 100;
                transform.localEulerAngles = Vector3.down * Mathf.Clamp(manualInput, -maxAngle, maxAngle);
                //Debug.Log("steering: " + manualInput + " shouldSendSteer: " + shouldSendSteer);

                // send message that steering wheel was used
                if (shouldSendSteer && Mathf.Abs(manualInput) > 1.0f)
                {
                    EventManager.TriggerEvent("TOR_steering");
                    shouldSendSteer = false;
                }
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

        public void setAutomatic(bool automated)
        {
            this.isAutomatic = automated;
        }

        #region EventManager

        private void StartAutomatedDrive(string jsonVars)
        {
            // use the same scriptable object when sending and receiving the same message since that was serialized
            TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
            // convert JSON string to RecognizedKeyword object
            JsonUtility.FromJsonOverwrite(jsonVars, objVars);
            setAutomatic(objVars.automate);
            shouldSendSteer = !objVars.automate;
            Debug.Log("automated: " + objVars.automate);
        }

        #endregion
    }
}