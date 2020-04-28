using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ANR
{
    public class VRCamera : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Camera mCamera;
        [SerializeField] private Vector3 tolerance = new Vector3(1.005f, 1.005f, 1.005f);
        
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //UnityEngine.XR.InputTracking.disablePositionalTracking = true;
            //UnityEngine.XR.InputTracking.Recenter();
        }

        // Update is called once per frame
        void Update()
        {
            //transform.position = -InputTracking.GetLocalPosition(XRNode.CenterEye);
            //transform.rotation = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.CenterEye));
            //if (Input.GetKeyDown(KeyCode.C)) {
            //Debug.Log("Reset");
            //transform.localPosition = new Vector3(-camera.transform.localPosition.x, -camera.transform.localPosition.y, -camera.transform.localPosition.z);
            //}

            if (mCamera != null)
            {
                Vector3 camLocalPos = mCamera.transform.localPosition;
                float newPosX = -camLocalPos.x * tolerance.x; //Mathf.Clamp(-camLocalPos.x, -0.05f, 0.05f);
                float newPosY = -camLocalPos.y * tolerance.y; //Mathf.Clamp(-camLocalPos.y, -0.05f, 0.05f);
                float newPosZ = -camLocalPos.z * tolerance.z; //Mathf.Clamp(-camLocalPos.z, -0.05f, 0.05f);
                transform.localPosition = new Vector3(newPosX, newPosY, newPosZ);
            }
        }
    }
}