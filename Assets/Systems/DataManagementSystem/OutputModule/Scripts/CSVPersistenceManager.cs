using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace ANR {

	public class CSVPersistenceManager : MonoBehaviour {

		// Singleton - CSVPersistenceManager should only be available once per scene
		#region Static Interface

		private static CSVPersistenceManager _instance;

		public static CSVPersistenceManager Instance {
			get {
				return _instance;
			}
		}

		private CSVPersistenceManager() {

		}

		#endregion

		#region Properties

		private StreamWriter writer;
		private char delimiter = ';';
		private int counter = 0;
		private DateTime time;
		private float nextIntervalTime = 0.0f;
		private float interval = 0.1f;
		private string path = ".\\Persistence";
		private string participantId = "P00";
		private string scenarioId = "S0";
		private List<TrackedData> dataList;

		#endregion

		private void Awake() {
			// checks if instance already exists
			if (_instance == null) {
				_instance = this;
			} else if (_instance != this) {
				// enforces singleton pattern -> there can only ever be one instance this class
				Destroy(gameObject);
			}

			// sets this to not be destroyed when reloading scene
			//DontDestroyOnLoad(gameObject);
		}

		// Use this for initialization
		void Start() {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}

		public void setTrackingInterval(float intervalInSeconds) {
			this.interval = intervalInSeconds;
		}

		public void SetScenarioData(string participantId, string scenarioId) {
			this.participantId = participantId;
			this.scenarioId = scenarioId;
		}

		public void SetTrackedData(List<TrackedData> trackedDataList) {
			dataList = trackedDataList;
		}

		public void StartDataTracking() {
			time = DateTime.Now;
			counter = 0;

			string filename = path + "\\" + participantId + "_" + scenarioId + "_" + time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + "-" + time.Hour.ToString("00") + "-" + time.Minute.ToString("00") + "-" + time.Second.ToString("00") + ".csv";
			writer = new StreamWriter(filename, true); 
			WriteHeader();
		}

		public void StopDataTracking() {
			if (writer != null) {
				writer.Close();
			}
		}

		void OnDestroy() {
			if (writer != null) {
				writer.Close();
			}
		}

		// Update is called once per frame
		void Update() {
			// wait for interval
			if (Time.time > nextIntervalTime) {
				nextIntervalTime += interval;
				// execute block of code here
				WriteToCSV();
				counter++;
			}
		}

		private async void WriteToCSV() {
			if (dataList == null) {
				return;
			}
			string csvData = "";
			int i = 0;
			foreach (TrackedData data in dataList) {
				csvData += DetermineValueStringRepresentation(data.value);
				if (i < dataList.Count) {
					csvData += delimiter;
				}
				i++;
			}
			writer.WriteLine(csvData);
			//Debug.Log("csv: " + csvData);
			await Task.Yield();
		}

		private void WriteHeader() {
			string csvHeader = "";
			int i = 0;
			foreach (TrackedData data in dataList) {
				csvHeader += DetermineHeaderStringRepresentation(data.key, data.value);
				if (i < dataList.Count) {
					csvHeader += delimiter;
				}
				i++;
			}
			//string data = "mapActive" + delimiter + "hudActive" + delimiter + "counter" + delimiter + "positionX" + delimiter + "positionY" + delimiter + "positionZ" + delimiter + "rotationX" + delimiter + "rotationY" + delimiter + "rotationZ"
			//	+ delimiter + "quartX" + delimiter + "quartY" + delimiter + "quartZ" + delimiter + "quartW";
			writer.WriteLine(csvHeader);
		}

		private string DetermineHeaderStringRepresentation(string key, object value) {
			if (key == null) {
				return "";
			}
			if (value is SimpleObjectValue) {
				return key;
			} else if (value is GameObjectValue) {
				string multiCsv = "";
				GameObjectValue gov = value as GameObjectValue;
				if (gov.trackPosition) {
					multiCsv += Vector3PositionHeaderToString(key);
				}
				if (gov.trackLocalPosition) {
					multiCsv += Vector3LocalPositionHeaderToString(key);
				}
				if (gov.trackRotation) {
					multiCsv += Vector3RotationHeaderToString(key);
				}
				if (gov.trackLocalRotation) {
					multiCsv += QuartHeaderToString(key);
				}
				return multiCsv;
			}

			return "";
		}

		private string DetermineValueStringRepresentation(object value) {
			if (value == null) {
				return "";
			}
			if (value is SimpleObjectValue) {
				SimpleObjectValue sov = value as SimpleObjectValue;
				if (sov.value is SimpleValueWrapper<bool>) {
					return BoolToString(((SimpleValueWrapper<bool>)sov.value).value);
				} else if (sov.value is SimpleValueWrapper<int>) {
					return IntToString(((SimpleValueWrapper<int>)sov.value).value);
				} else if (sov.value is SimpleValueWrapper<float>) {
					return FloatToString(((SimpleValueWrapper<float>)sov.value).value);
				} else if (sov.value is StringWrapper) {
					return ((StringWrapper)sov.value).value;
				} else {
					return "";
				}
			} else if (value is GameObjectValue) {
				string multiCsv = "";
				GameObjectValue gov = value as GameObjectValue;
				if (gov.trackPosition) {
					multiCsv += Vector3ToString(gov.value.transform.position);
				}
				if (gov.trackLocalPosition) {
					multiCsv += Vector3ToString(gov.value.transform.localPosition);
				}
				if (gov.trackRotation) {
					multiCsv += QuartToString(gov.value.transform.localRotation);
				}
				if (gov.trackLocalRotation) {
					multiCsv += Vector3ToString(gov.value.transform.localEulerAngles);
				}
				return multiCsv;
			}

			return "";
		}

		#region Header ToString

		private string Vector3PositionHeaderToString(string key) {
			return key + "-positionX" + delimiter + key + "-positionY" + delimiter + key + "-positionZ";
		}

		private string Vector3LocalPositionHeaderToString(string key) {
			return key + "-localPositionX" + delimiter + key + "-localPositionY" + delimiter + key + "-localPositionZ";
		}

		private string Vector3RotationHeaderToString(string key) {
			return key + "-rotationX" + delimiter + key + "-rotationY" + delimiter + key + "-rotationZ";
		}

		private string QuartHeaderToString(string key) {
			return key + "-quartX" + delimiter + key + "-quartY" + delimiter + key + "-quartZ" + delimiter + key + "-quartW";
		}

		#endregion

		#region Value ToString

		private string IntToString(int num) {
			return num.ToString("0");
		}

		private string FloatToString(float num) {
			return num.ToString("0.0000");
		}

		private string BoolToString(bool flag) {
			return flag ? "1" : "0";
		}

		private string Vector3ToString(Vector3 vector3) {
			return vector3.x.ToString("0.0000") + delimiter + vector3.y.ToString("0.0000") + delimiter + vector3.z.ToString("0.0000");
		}

		private string QuartToString(Quaternion quart) {
			return quart.x.ToString("0.0000") + delimiter + quart.y.ToString("0.0000") + delimiter + quart.z.ToString("0.0000") + delimiter + quart.w.ToString("0.0000");
		}

		#endregion
	}

}