using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ANR {

	// Replacement of the basic UnityEvent with a unity event that has a string that will be used to serialize JSON
	[System.Serializable]
	public class CustomUnityEvent : UnityEvent<string> { }

	// This class holds the messages, you need to attach it to one GameObject in the scene
	public class EventManager : MonoBehaviour {

        // events with string as param
		private Dictionary<string, CustomUnityEvent> eventDictionaryCustom;
        // events with no param
        private Dictionary<string, UnityEvent> eventDictionary;

        private static EventManager eventManager;

		// grabs the instance of the event manager
		public static EventManager Instance {
			get {
				if (!eventManager) {
					eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

					if (!eventManager) {
						Debug.Log("There needs to be one active EventManager script on a gameobject in your scene.");
					} else {
						eventManager.Init();

					}
				}
				return eventManager;
			}
		}

		// creates dictionary for the events
		void Init() {
			if (eventDictionaryCustom == null) {
				eventDictionaryCustom = new Dictionary<string, CustomUnityEvent>();
			}
            if (eventDictionary == null) {
                eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        #region CustomUnityEvent

        // function called to insert an event in the dictionary
        public static void StartListening(string eventName, UnityAction<string> listener) {
			CustomUnityEvent thisEvent = null;
			if (Instance.eventDictionaryCustom.TryGetValue(eventName, out thisEvent)) {
				thisEvent.AddListener(listener);
			} else {
				thisEvent = new CustomUnityEvent();
				thisEvent.AddListener(listener);
				Instance.eventDictionaryCustom.Add(eventName, thisEvent);
			}
		}

		// removes an event from the dictionary
		public static void StopListening(string eventName, UnityAction<string> listener) {
			if (eventManager == null) return;
			CustomUnityEvent thisEvent = null;
			if (Instance.eventDictionaryCustom.TryGetValue(eventName, out thisEvent)) {
				thisEvent.RemoveListener(listener);
			}
		}

		// event trigger with a string passed as a parameter
		public static void TriggerEvent(string eventName, string json) {
			CustomUnityEvent thisEvent = null;
			if (Instance.eventDictionaryCustom.TryGetValue(eventName, out thisEvent)) {
				// finally passes the message
				thisEvent.Invoke(json);
			}
		}

        #endregion

        #region UnityEvent

        // function called to insert an event in the dictionary
        public static void StartListening(string eventName, UnityAction listener) {
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        // removes an event from the dictionary
        public static void StopListening(string eventName, UnityAction listener) {
            if (eventManager == null) return;
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        // event trigger with no parameter
        public static void TriggerEvent(string eventName) {
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                // finally passes the message
                thisEvent.Invoke();
            }
        }

        #endregion
    }
}