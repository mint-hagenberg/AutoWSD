using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANR {

    public class WebcamDisplay : MonoBehaviour {

        #region Properties

        [SerializeField]
        private RawImage webCamImage;
        [SerializeField]
        private int deviceIndex = 0;

        private WebCamDevice[] devices;
        private WebCamTexture webcamTexture;

        #endregion

        // Start is called before the first frame update
        void Start() {
            devices = WebCamTexture.devices;
            for (int i=0; i<devices.Length; i++) {
                Debug.Log("Webcam " + i + ": " + devices[i].name);
            }

            webcamTexture = new WebCamTexture();
            if (deviceIndex < devices.Length) {
                webcamTexture.deviceName = devices[deviceIndex].name;
            }
            webCamImage.texture = webcamTexture;
            webCamImage.material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }

        private void OnDestroy() {
            webcamTexture.Stop();
        }
    }
}