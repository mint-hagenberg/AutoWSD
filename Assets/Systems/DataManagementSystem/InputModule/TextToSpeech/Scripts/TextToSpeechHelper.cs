using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANR {

	public class TextToSpeechHelper : MonoBehaviour {

		#region Properties

		[SerializeField]
		private TextToSpeechManager textToSpeechManager;

		[SerializeField]
		private TextAsset welcomeMessageTextFile;

		[SerializeField]
		private TextAsset TORMessageTextFile;

		[SerializeField]
		private TextAsset finishMessageTextFile;

        // example text of how to use the Microsoft Speech API using xml
        //private string exampleText = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice xml:lang='de-DE' gender='female'>Willkommen beim hochautomatisierten Fahrzeug. Vielen Dank für Ihre Teilnahme an der Studie.</voice></speak>"; //"Willkommen beim hochautomatisierten Fahrzeug. Vielen Dank für Ihre Teilnahme an der Studie.";

        // event listeners
        private UnityAction<string> eventListenerStart;
        private UnityAction<string> eventListenerFinish;
        private UnityAction<string> eventListenerTOR;

        #endregion

        private void Awake() {
			// checks if a TextToSpeechManager has already been assigned to static variable TextToSpeechManager.Instance or if it's still null
			if (TextToSpeechManager.Instance == null) {
				// instantiates textToSpeechManager prefab
				Instantiate(textToSpeechManager);
			}

            // listen to events (from StartArea, FinishArea)
            eventListenerStart = new UnityAction<string>(SayWelcomeTextTrigger);
            eventListenerFinish = new UnityAction<string>(SayFinishTextTrigger);
            eventListenerTOR = new UnityAction<string>(SayTORTextTrigger);
        }

		// Use this for initialization
		void Start() {
			/*string text = welcomeMessageTextFile.text;  //this is the content as string
			List<string> lines = new List<string>(text.Split('\n'));

			for (int i=0; i<lines.Count; i++) {
				//Debug.Log("text: " + lines[i]);
				string speak = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice xml:lang='de-DE' gender='female'>" + lines[i] + "</voice></speak>";
				textToSpeechManager.Say(speak);
			}*/
		}

		private void SayParameterizedText(string text) {
			string speak = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice xml:lang='de-DE' gender='female'>" + text + "</voice></speak>";
			textToSpeechManager.Say(speak);
		}

        // Add more methods for extra speeches

		public void SayWelcomeText() {
			SayParameterizedText(welcomeMessageTextFile.text);
		}

        public void SayTORText() {
            SayParameterizedText(TORMessageTextFile.text);
        }

        public void SayFinishText() {
            SayParameterizedText(finishMessageTextFile.text);
        }

        void OnEnable() {
            EventManager.StartListening("START_AREA", eventListenerStart);
            EventManager.StartListening("FINISH_AREA", eventListenerFinish);
            EventManager.StartListening("TOR_AREA", eventListenerTOR);
        }

        void OnDisable() {
            EventManager.StopListening("START_AREA", eventListenerStart);
            EventManager.StopListening("FINISH_AREA", eventListenerFinish);
            EventManager.StopListening("TOR_AREA", eventListenerTOR);
        }

        #region EventManager

        private void SayWelcomeTextTrigger(string jsonVars) {
            SayParameterizedText(welcomeMessageTextFile.text);
        }

        private void SayFinishTextTrigger(string jsonVars) {
            SayParameterizedText(finishMessageTextFile.text);
        }

        private void SayTORTextTrigger(string jsonVars) {
            // use the same scriptable object when sending and receiving the same message since that was serialized
            TORMessage objVars = ScriptableObject.CreateInstance<TORMessage>();
            // convert JSON string to RecognizedKeyword object
            JsonUtility.FromJsonOverwrite(jsonVars, objVars);
            if (!objVars.automate) {
                // only say TOR text if manual driving is required
                SayParameterizedText(TORMessageTextFile.text);
            }
        }

        #endregion
    }
}