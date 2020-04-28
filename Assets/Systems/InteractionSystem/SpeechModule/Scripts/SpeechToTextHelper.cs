using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.Events;

namespace ANR
{
    public class SpeechToTextHelper : MonoBehaviour
    {
        #region Properties

        [SerializeField] private SpeechToTextManager speechToTextManager;

        // event listener
        private UnityAction<string> eventListener;
        [SerializeField] private string[] keywords = new string[] {"up", "down", "left", "right"};
        [SerializeField] private ConfidenceLevel confidence = ConfidenceLevel.Medium;
        [SerializeField] private float speed = 1;
        [SerializeField] private Text resultsText;

        #endregion

        void Awake()
        {
            // checks if a SpeechToTextManager has already been assigned to static variable SpeechToTextManager.Instance or if it's still null
            if (SpeechToTextManager.Instance == null)
            {
                // instantiates speechToTextManager prefab
                Instantiate(speechToTextManager);
            }

            SpeechToTextManager.Instance.Init(keywords, confidence);

            // listen to events (from SpeechToTextManager)
            eventListener = new UnityAction<string>(OnKeywordRecognized);
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnKeywordRecognized", eventListener);
        }

        private void OnDisable()
        {
            EventManager.StopListening("OnKeywordRecognized", eventListener);
        }

        #region EventManager

        // the actual event that will receive the "message"
        void OnKeywordRecognized(string jsonVars)
        {
            // use the same scriptable object when sending and receiving the same message since that was serialized
            RecognizedKeyword objVars = ScriptableObject.CreateInstance<RecognizedKeyword>();
            // convert JSON string to RecognizedKeyword object
            JsonUtility.FromJsonOverwrite(jsonVars, objVars);

            // set text
            Debug.Log("OnKeywordRecognized: " + jsonVars + "; " + objVars.word);
            if (resultsText != null)
            {
                resultsText.text = "You said: <b>" + objVars.word + "</b>";
            }

            // check recognized words
            switch (objVars.word)
            {
                case "yes":
                    EventManager.TriggerEvent("speech_yes");
                    break;
                case "no":
                    EventManager.TriggerEvent("speech_no");
                    break;
                default:
                    return;
            }
        }

        #endregion
    }
}