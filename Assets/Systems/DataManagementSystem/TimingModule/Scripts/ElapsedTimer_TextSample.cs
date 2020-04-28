using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ANR
{
    public class ElapsedTimer_TextSample : MonoBehaviour
    {
        #region Properties
        
        [SerializeField]
        private ElapsedTimer elapsedTimer;
        
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<TMP_Text>().text = elapsedTimer.GetElapsedTime().ToString("#.00");
        }
    }
}
