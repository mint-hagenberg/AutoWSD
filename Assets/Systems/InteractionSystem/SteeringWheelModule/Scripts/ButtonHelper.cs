using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANR
{
    public class ButtonHelper : MonoBehaviour
    {
        #region Properties

        // selected object
        [SerializeField] private Transform selectedObject;

        // event listeners
        private UnityAction eventListenerSteeringWheelButtonDPadRight;
        private UnityAction eventListenerSteeringWheelButtonDPadLeft;
        private UnityAction eventListenerSteeringWheelButtonDPadUp;
        private UnityAction eventListenerSteeringWheelButtonDPadDown;
        private UnityAction eventListenerSteeringWheelButtonActionUp;
        private UnityAction eventListenerSteeringWheelButtonActionDown;
        private UnityAction eventListenerSteeringWheelButtonPlus;
        private UnityAction eventListenerSteeringWheelButtonMinus;

        #endregion

        private void Awake()
        {
            // listen to events
            eventListenerSteeringWheelButtonDPadRight = new UnityAction(MoveRight);
            eventListenerSteeringWheelButtonDPadLeft = new UnityAction(MoveLeft);
            eventListenerSteeringWheelButtonDPadUp = new UnityAction(MoveUp);
            eventListenerSteeringWheelButtonDPadDown = new UnityAction(MoveDown);
            eventListenerSteeringWheelButtonActionUp = new UnityAction(MoveForward);
            eventListenerSteeringWheelButtonActionDown = new UnityAction(MoveBackward);
            eventListenerSteeringWheelButtonPlus = new UnityAction(ScaleUp);
            eventListenerSteeringWheelButtonMinus = new UnityAction(ScaleDown);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnEnable()
        {
            EventManager.StartListening("SteeringWheel_Button_DPad_Right", eventListenerSteeringWheelButtonDPadRight);
            EventManager.StartListening("SteeringWheel_Button_DPad_Left", eventListenerSteeringWheelButtonDPadLeft);
            EventManager.StartListening("SteeringWheel_Button_DPad_Up", eventListenerSteeringWheelButtonDPadUp);
            EventManager.StartListening("SteeringWheel_Button_DPad_Down", eventListenerSteeringWheelButtonDPadDown);
            EventManager.StartListening("SteeringWheel_Button_Action_Up", eventListenerSteeringWheelButtonActionUp);
            EventManager.StartListening("SteeringWheel_Button_Action_Down", eventListenerSteeringWheelButtonActionDown);
            EventManager.StartListening("SteeringWheel_Button_Plus", eventListenerSteeringWheelButtonPlus);
            EventManager.StartListening("SteeringWheel_Button_Minus", eventListenerSteeringWheelButtonMinus);
        }

        void OnDisable()
        {
            EventManager.StopListening("SteeringWheel_Button_DPad_Right", eventListenerSteeringWheelButtonDPadRight);
            EventManager.StopListening("SteeringWheel_Button_DPad_Left", eventListenerSteeringWheelButtonDPadLeft);
            EventManager.StopListening("SteeringWheel_Button_DPad_Up", eventListenerSteeringWheelButtonDPadUp);
            EventManager.StopListening("SteeringWheel_Button_DPad_Down", eventListenerSteeringWheelButtonDPadDown);
            EventManager.StopListening("SteeringWheel_Button_Action_Up", eventListenerSteeringWheelButtonActionUp);
            EventManager.StopListening("SteeringWheel_Button_Action_Down", eventListenerSteeringWheelButtonDPadLeft);
            EventManager.StopListening("SteeringWheel_Button_Plus", eventListenerSteeringWheelButtonPlus);
            EventManager.StopListening("SteeringWheel_Button_Minus", eventListenerSteeringWheelButtonMinus);
        }

        #region EventManager

        public void MoveRight()
        {
            selectedObject.Translate(Vector3.right * Time.deltaTime);
        }

        public void MoveLeft()
        {
            selectedObject.Translate(Vector3.left * Time.deltaTime);
        }

        public void MoveUp()
        {
            selectedObject.Translate(Vector3.up * Time.deltaTime);
        }

        public void MoveDown()
        {
            selectedObject.Translate(Vector3.down * Time.deltaTime);
        }

        public void MoveForward()
        {
            selectedObject.Translate(Vector3.forward * Time.deltaTime);
        }

        public void MoveBackward()
        {
            selectedObject.Translate(Vector3.back * Time.deltaTime);
        }

        public void ScaleUp()
        {
            selectedObject.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
        }

        public void ScaleDown()
        {
            selectedObject.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
        }

        #endregion
    }
}