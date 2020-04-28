using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

    public class Countdown : MonoBehaviour {

        #region Properties

        [SerializeField]
        private string id = "countdown_xy";
        [SerializeField]
        private bool repeat = true;
        [SerializeField]
        private float countdownTime = 5.0f;

        private float timeLeft = 0.0f;
        private bool active = true;

        #endregion

        // Start is called before the first frame update
        void Start() {
            StartCountdown();
        }

        // Update is called once per frame
        void Update() {
            if (!active) return;

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                Debug.Log("countdown: " + id);

                // trigger event
                EventManager.TriggerEvent(id);

                // repeat or stop countdown
                if (repeat) {
                    timeLeft = countdownTime;
                } else {
                    StopCountdown();
                }
            }
        }

        public void StartCountdown() {
            active = true;
            timeLeft = countdownTime;
        }

        public void StopCountdown() {
            active = false;
        }

        public float GetCountdownTime() {
            return timeLeft;
        }
    }
}