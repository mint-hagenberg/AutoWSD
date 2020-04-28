using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    public class LogitechSteeringButtonManager : MonoBehaviour
    {
        #region Properties

        // Logitech steering wheel recording
        private LogitechGSDK.DIJOYSTATE2ENGINES rec;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Logitech Steering Wheel Init:" + LogitechGSDK.LogiSteeringInitialize(false));
            Debug.Log("Logitech Steering Wheel Connected: " + LogitechGSDK.LogiIsConnected(0));
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                // use Logitech Steering Wheel
                rec = LogitechGSDK.LogiGetStateUnity(0); // 0 = steering wheel

                // upper left directional pad
                switch (rec.rgdwPOV[0])
                {
                    case (0): // up
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_Up");
                        break;
                    case (4500): // up-right
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_UpRight");
                        break;
                    case (9000): // right
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_Right");
                        break;
                    case (13500): // down-right
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_DownRight");
                        break;
                    case (18000): // down
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_Down");
                        break;
                    case (22500): // down-left
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_DownLeft");
                        break;
                    case (27000): // left
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_Left");
                        break;
                    case (31500): // up-left
                        EventManager.TriggerEvent("SteeringWheel_Button_DPad_UpLeft");
                        break;
                }
            }

            // upper right action buttons
            if (LogitechGSDK.LogiButtonIsPressed(0, 0))
            {
                // right keypad: down ("X")
                EventManager.TriggerEvent("SteeringWheel_Button_Action_Down");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 1))
            {
                // right keypad: left ("square")
                EventManager.TriggerEvent("SteeringWheel_Button_Action_Left");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 2))
            {
                // right keypad: right ("O")
                EventManager.TriggerEvent("SteeringWheel_Button_Action_Right");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 3))
            {
                // right keypad: up ("triangle")
                EventManager.TriggerEvent("SteeringWheel_Button_Action_Up");
            }

            // turn signals behind steering wheel
            if (LogitechGSDK.LogiButtonIsPressed(0, 4))
            {
                // right behind steering wheel: turn signal right
                EventManager.TriggerEvent("SteeringWheel_Button_Signal_Right");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 5))
            {
                // left behind steering wheel: turn signal left
                EventManager.TriggerEvent("SteeringWheel_Button_Signal_Left");
            }

            // R buttons
            if (LogitechGSDK.LogiButtonIsPressed(0, 6))
            {
                // right R2 button
                EventManager.TriggerEvent("SteeringWheel_Button_R2_Right");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 7))
            {
                // left L2 button
                EventManager.TriggerEvent("SteeringWheel_Button_L2_Left");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 10))
            {
                // right R3 button
                EventManager.TriggerEvent("SteeringWheel_Button_R3_Right");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 11))
            {
                // left L3 button
                EventManager.TriggerEvent("SteeringWheel_Button_R3_Left");
            }

            // middle buttons
            if (LogitechGSDK.LogiButtonIsPressed(0, 8))
            {
                // middle "share" button
                EventManager.TriggerEvent("SteeringWheel_Button_Share");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 9))
            {
                // middle "option" button
                EventManager.TriggerEvent("SteeringWheel_Button_Option");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 24))
            {
                // middle "Playstation Logo" button
                EventManager.TriggerEvent("SteeringWheel_Button_Logo");
            }

            // Plus/Minus buttons
            if (LogitechGSDK.LogiButtonIsPressed(0, 19))
            {
                // left Plus button ("+")
                EventManager.TriggerEvent("SteeringWheel_Button_Plus");
            }
            else if (LogitechGSDK.LogiButtonIsPressed(0, 20))
            {
                // left Minus button ("-")
                EventManager.TriggerEvent("SteeringWheel_Button_Minus");
            }
        }
    }
}