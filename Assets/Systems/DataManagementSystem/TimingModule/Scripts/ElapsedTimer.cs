using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

    public class ElapsedTimer : MonoBehaviour {

        #region Properties

        [SerializeField]
        private string id = "elapsed_xy";

        private float startTime = 0.0f;

        #endregion

        // Start is called before the first frame update
        void Start() {
            StartTimer();
        }

        // Update is called once per frame
        void Update() {
            
        }

        public void StartTimer() {
            startTime = Time.time;
        }

        public void StopTimer() {
            startTime = 0.0f;
        }

        public float GetElapsedTime() {
            float elapsedTime = Time.time - startTime;
            Debug.Log("elapsed: " + id + ": " + elapsedTime);
            return elapsedTime;
        }
    }
}