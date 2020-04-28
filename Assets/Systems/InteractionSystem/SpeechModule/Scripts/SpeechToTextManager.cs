using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace ANR
{
    public class SpeechToTextManager : MonoBehaviour
    {
        // Singleton - SpeechToTextManager should only be available once per scene

        #region Static Interface

        private static SpeechToTextManager _instance;

        public static SpeechToTextManager Instance
        {
            get { return _instance; }
        }

        private SpeechToTextManager()
        {
        }

        #endregion

        #region Properties

        // phrase recognizer
        private PhraseRecognizer recognizer;

        // currently recognized word
        private string word = "";

        #endregion

        private void Awake()
        {
            // checks if instance already exists
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                // enforces singleton pattern -> there can only ever be one instance this class
                Destroy(gameObject);
            }
        }

        public void Init(string[] keywords, ConfidenceLevel confidence)
        {
            if (keywords != null)
            {
                recognizer = new KeywordRecognizer(keywords, confidence);
                recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
                recognizer.Start();
            }
        }

        private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            word = args.text;
            Debug.Log("recognized word: " + word);

            // using CreateInstance instead of new RecognizedPhrase because it's a scriptable object
            RecognizedKeyword objVars = ScriptableObject.CreateInstance<RecognizedKeyword>();
            objVars.word = word;

            // converts to a json string, so the messge only needs one string to pass a large number of diverse parameters
            string jsonVars = JsonUtility.ToJson(objVars);
            // emits message to listeners, passing the json string as a parameter
            EventManager.TriggerEvent("OnKeywordRecognized", jsonVars);
        }

        private void OnDestroy()
        {
            if (recognizer != null && recognizer.IsRunning)
            {
                recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
                recognizer.Stop();
            }
        }
    }
}