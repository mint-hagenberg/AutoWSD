using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    public class LogitechSteeringManager : MonoBehaviour
    {
        #region Properties

        [SerializeField] private CarController carController; // the car controller we want to use

        [SerializeField]
        private bool useKeyboard = false; // use keyboard to steer vehicle instead of Logitech Steering Wheel

        // Logitech steering wheel recording
        private LogitechGSDK.DIJOYSTATE2ENGINES rec;

        #endregion

        private void Awake()
        {
            if (carController == null)
            {
                Debug.Log("You must provide a valid CarController.");
            }
        }

        void Start()
        {
            Debug.Log("Logitech Steering Wheel Init:" + LogitechGSDK.LogiSteeringInitialize(false));
            Debug.Log("Logitech Steering Wheel Connected: " + LogitechGSDK.LogiIsConnected(0));
        }

        void FixedUpdate()
        {
            // horizontal, vertical acceleration and brakes
            float h = 0.0f, v = 0.0f, handbrake = 0.0f, footbrake = 0.0f;
            if (useKeyboard)
            {
                h = Input.GetAxis("Horizontal"); // using keyboard arrows (left, right) or A,D keys or Steering Wheel
                v = Input.GetAxis("Vertical"); // using keyboard arrows (up, down) or W,S keys
                handbrake = Input.GetAxis("Jump"); // using Space key
            }
            else if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                // use Logitech Steering Wheel
                rec = LogitechGSDK.LogiGetStateUnity(0); // 0 = steering wheel

                // pass the input to the car
                h = Input.GetAxis("Steering"); // using Steering Wheel, alternative: logiEngine.lX
                handbrake = Input.GetAxis("Jump"); // using Space key

                // acceleration from gas pedal
                float logiAcc = rec.lY - 32767; // value between -32768 (full speed) and 0 (no speed)
                float logiAccNormalized = Mathf.Clamp(-(logiAcc / 32768.0f), 0.0f, 1.0f);
                v = logiAccNormalized;

                // brake from brake pedal
                float logiBreak = rec.lRz - 32767;
                footbrake = Mathf.Clamp((logiBreak / 32768.0f), -1.0f, 0.0f);

                //Debug.Log("button: " + LogitechGSDK.LogiButtonIsPressed(0, 0));
            }

            //Debug.Log("h: " + h + ", v: " + v + ", footbrake: " + footbrake + ", handbrake: " + handbrake);
            carController.Move(h, v, footbrake, handbrake);
        }

        private void OnDestroy()
        {
            Debug.Log("Logitech Steering Wheel Shutdown:" + LogitechGSDK.LogiSteeringShutdown());
        }
    }
}