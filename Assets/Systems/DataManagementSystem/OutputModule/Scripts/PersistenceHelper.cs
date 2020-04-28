using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

	// use this class for entering data to be persisted eg. head position, rotation, flags, counts etc.
	public class PersistenceHelper : MonoBehaviour {

		#region Properties

		// CSVPersistenceManager prefab to instantiate
		[SerializeField]
		private GameObject csvPersistenceManager;

		[SerializeField]
		private float intervalInSeconds = 0.1f;
		[SerializeField]
		private string participantId = "P00";
		[SerializeField]
		private string scenarioId = "S0";

		[SerializeField]
		private GameObject trackedHeadTransform = null;
		
		// bool key
		private bool boolKey { set { boolKeyWrapper.value = value; } get { return boolKeyWrapper.value; }}
		// bool key wrapper
		private SimpleValueWrapper<bool> boolKeyWrapper = new SimpleValueWrapper<bool>(false);

		// int key
		private int intKey { set { intKeyWrapper.value = value; } get { return intKeyWrapper.value; } }
		// int key wrapper
		private SimpleValueWrapper<int> intKeyWrapper = new SimpleValueWrapper<int>(0);

		// float key
		private float floatKey { set { floatKeyWrapper.value = value; } get { return floatKeyWrapper.value; } }
		// float key wrapper
		private SimpleValueWrapper<float> floatKeyWrapper = new SimpleValueWrapper<float>(0);

		// string key
		private string stringKey { set { stringKeyWrapper.value = value; } get { return stringKeyWrapper.value; } }
		// string key wrapper
		private StringWrapper stringKeyWrapper = new StringWrapper("");
	
		#endregion

		void Awake() {
			// set scenario data
			SetupCSVPersistenceManager();
			// set data to be tracked
			SetupTrackedData();
		}

		// Use this for initialization
		void Start() {
			// start data tracking
			CSVPersistenceManager.Instance.StartDataTracking();
		}

		private void SetupCSVPersistenceManager() {
			// checks if a CSVPersistenceManager has already been assigned to static variable CSVPersistenceManager.Instance or if it's still null
			if (CSVPersistenceManager.Instance == null) {
				// instantiates csvPersistenceManager prefab
				Instantiate(csvPersistenceManager);
			}

			// tracking interval
			CSVPersistenceManager.Instance.setTrackingInterval(this.intervalInSeconds);

			// specific scenario data
			CSVPersistenceManager.Instance.SetScenarioData(participantId, scenarioId);
		}

		private void SetupTrackedData() {
			List<TrackedData> trackedDataList = new List<TrackedData>();
			
			// track GameObject
			TrackedData govData = new TrackedData();
			govData.key = "head";
			GameObjectValue gov = new GameObjectValue();
			gov.value = trackedHeadTransform;
			gov.trackPosition = true;
			gov.trackRotation = true;
			govData.value = gov;
			trackedDataList.Add(govData);

			// track bool
			TrackedData sovBoolData = new TrackedData();
			sovBoolData.key = "boolKey";
			SimpleObjectValue sovBool = new SimpleObjectValue(boolKeyWrapper);
			sovBoolData.value = sovBool;
			trackedDataList.Add(sovBoolData);

			// track int
			TrackedData sovIntData = new TrackedData();
			sovIntData.key = "intKey";
			SimpleObjectValue sovInt = new SimpleObjectValue(intKeyWrapper);
			sovIntData.value = sovInt;
			trackedDataList.Add(sovIntData);

			// track float
			TrackedData sovFloatData = new TrackedData();
			sovFloatData.key = "floatKey";
			SimpleObjectValue sovFloat = new SimpleObjectValue(floatKeyWrapper);
			sovFloatData.value = sovFloat;
			trackedDataList.Add(sovFloatData);

			// track string
			TrackedData sovStringData = new TrackedData();
			sovStringData.key = "stringKey";
			SimpleObjectValue sovString = new SimpleObjectValue(stringKeyWrapper);
			sovStringData.value = sovString;
			trackedDataList.Add(sovStringData);

			// set list
			CSVPersistenceManager.Instance.SetTrackedData(trackedDataList);
		}

		void onDestroy() {
			// stop data tracking
			CSVPersistenceManager.Instance.StopDataTracking();
		}

		// just for testing
		private void Update() {
			if (Time.time > 5) {
				boolKey = true;
				intKey = 42;
				floatKey = 123.01452f;
				stringKey = "servus";
			}
		}
	}
}