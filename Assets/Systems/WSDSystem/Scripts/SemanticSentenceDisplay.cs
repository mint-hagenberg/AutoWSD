using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ANR
{
    public class SemanticSentenceDisplay : MonoBehaviour
    {
        #region Properties

        [SerializeField] private SemanticSentenceLoader semanticSentenceLoader;
        [SerializeField] private Text semanticSentenceText;
        [SerializeField] private Countdown countdown;
        [SerializeField] private ElapsedTimer elapsedTimer;

        private SemanticSentence[] semanticSentences;
        private int index = -1;

        // event listeners
        private UnityAction eventListenerCountdown;
        private UnityAction eventListenerSpeechYes;
        private UnityAction eventListenerSpeechNo;

        #endregion

        private void Awake()
        {
            // listen to events (countdown)
            eventListenerCountdown = new UnityAction(UpdateDisplay);
            eventListenerSpeechYes = new UnityAction(VerifyAffirmativeAnswer);
            eventListenerSpeechNo = new UnityAction(VerifyNegativeAnswer);
        }

        // Start is called before the first frame update
        void Start()
        {
            semanticSentences = semanticSentenceLoader.GetSemanticSentences();
        }

        void OnEnable()
        {
            EventManager.StartListening("countdown_text", eventListenerCountdown);
            EventManager.StartListening("speech_yes", eventListenerSpeechYes);
            EventManager.StartListening("speech_no", eventListenerSpeechNo);
        }

        void OnDisable()
        {
            EventManager.StopListening("countdown_text", eventListenerCountdown);
            EventManager.StopListening("speech_yes", eventListenerSpeechYes);
            EventManager.StopListening("speech_no", eventListenerSpeechNo);
        }

        #region EventManager

        public void VerifyAffirmativeAnswer()
        {
            VerifyAnswer(true);
        }

        public void VerifyNegativeAnswer()
        {
            VerifyAnswer(false);
        }

        public void UpdateDisplay()
        {
            // update index
            if (index < semanticSentences.Length - 1)
            {
                index++;
            }

            // no answer was given -> send false event with elapsed time being the countdown time
            if (index > 0)
            {
                SendSemanticAnswerEvent(false, countdown.GetCountdownTime());
            }

            // start elapsed timer
            elapsedTimer.StartTimer();

            // display text
            semanticSentenceText.text = semanticSentences[index].sentence;
        }

        #endregion

        public void VerifyAnswer(bool answer)
        {
            bool correct = semanticSentences[index].correct == answer;
            float elapsedTime = elapsedTimer.GetElapsedTime();

            //Debug.Log("correct: " + correct + ", duration: " + elapsedTime);
            // trigger event
            SendSemanticAnswerEvent(correct, elapsedTime);

            // play sound
            SoundManager.Instance.PlayInfo();

            // update display text
            UpdateDisplay();

            // reset countdown
            countdown.StartCountdown();
        }

        private void SendSemanticAnswerEvent(bool correct, float elapsedTime)
        {
            // using CreateInstance because it's a scriptable object
            SemanticSentenceAnswerMessage objVars = ScriptableObject.CreateInstance<SemanticSentenceAnswerMessage>();
            objVars.correct = correct;
            objVars.elapsedTime = elapsedTime;
            // converts to a json string, so the messge only needs one string to pass a large number of diverse parameters
            string jsonVars = JsonUtility.ToJson(objVars);
            // emits message to listeners, passing the json string as a parameter
            EventManager.TriggerEvent("semantic_sentence_message", jsonVars);
        }
    }
}