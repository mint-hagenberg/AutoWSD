using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ANR
{
    public class Countdown_TextSample : MonoBehaviour
    {
        #region Properties
        
        [SerializeField]
        private Countdown countdownTimer;
        
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<TMP_Text>().text = countdownTimer.GetCountdownTime().ToString("#.00");
        }
    }
}

