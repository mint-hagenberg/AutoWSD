using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

    public class WebcamDisplayPlane : MonoBehaviour {

        #region Properties

        [SerializeField]
        private int deviceIndex = 0;

        private WebCamDevice[] devices;
        private WebCamTexture webcamTexture;

        #endregion

        // Start is called before the first frame update
        void Start() {
            devices = WebCamTexture.devices;
            for (int i = 0; i < devices.Length; i++) {
                Debug.Log("Webcam " + i + ": " + devices[i].name);
            }

            webcamTexture = new WebCamTexture();
            if (deviceIndex < devices.Length) {
                webcamTexture.deviceName = devices[deviceIndex].name;
            }
            this.GetComponent<MeshRenderer>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }

        private void OnDestroy() {
            webcamTexture.Stop();
        }
    }
}