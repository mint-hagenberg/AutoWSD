using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace ANR {

	public class UDPManager : MonoBehaviour {

		// Singleton - UDPManager should only be available once per scene
		#region Static Interface

		private static UDPManager _instance;

		public static UDPManager Instance {
			get {
				return _instance;
			}
		}

		private UDPManager() {

		}

		#endregion

		#region Properties

		private string ipAddress = "127.0.0.1";
		private int port = 8051;
		private int portOut = 5007;
		private IPEndPoint anyIP = null;
		private IPEndPoint remoteEndPoint = null;
		private UdpClient client = null;

		private string filename = "UdpLog.txt";
		private bool connectedToConsole = false;
		private StreamWriter streamWriter;

		[SerializeField]
		private ScenarioSettingsManager scenarioSettingsManager;

		#endregion

		void Awake() {
			// checks if instance already exists
			if (_instance == null) {
				_instance = this;
			} else if (_instance != this) {
				// enforces singleton pattern -> there can only ever be one instance this class
				Destroy(gameObject);
			}

			InitScenarioSettings();
		}

		private void InitScenarioSettings() {
			// checks if a ScenarioSettingsManager has already been assigned to static variable ScenarioSettingsManager.Instance or if it's still null
			if (ScenarioSettingsManager.Instance == null) {
				// instantiates scenarioSettingsManager prefab
				Instantiate(scenarioSettingsManager);
			}
		}

		// Use this for initialization
		void Start() {
			streamWriter = new StreamWriter(filename, true);

			WriteToLog("UDPManager - Start");

			InitClient();

			if (client != null) {
				WriteToLog("Connection successful.");
			} else {
				WriteToLog("Connection unsuccessful.");
			}

			remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), portOut);
		}

		private void InitClient() {
			WriteToLog("Starting Unity UDP Client ...");

			client = new UdpClient(port);
			anyIP = new IPEndPoint(IPAddress.Any, 0);
			client.Client.Blocking = false;

			WriteToLog("Setup Unity Client ... done");
		}

		// Update is called once per frame
		void Update() {
			if (!connectedToConsole) {
				SendRemoteMessage("unity_app_started");
			}
			ReceiveData();
		}

		private void ReceiveData() {
			string[] recvData = new string[2];
			try {
				// receive bytes
				byte[] data = client.Receive(ref anyIP);
				if (data != null) {
					// convert bytes with UTF8-Coding into text
					string text = Encoding.UTF8.GetString(data);
					WriteToLog(text);
					recvData = text.Split(':'); //[0] - topic, [1] - message

					// check if recvData has exactly 2 components - topic and message
					if (recvData.Length != 2) {
						SendRemoteMessage("err_format_invalid");
						return;
					}
					string topic = recvData[0];
					string message = recvData[1];

					// check if recvData components are valid
					if (topic == null || message == null || topic.Length == 0) {
						SendRemoteMessage("err_topic_or_message_empty");
						return;
					}

					// handle topic and message
					bool recvDataInvalid = false;

					if (topic.Equals("connection")) {
						if (message.Equals("start")) {
							Debug.Log("connection:start");
							connectedToConsole = true;
							SendRemoteMessage("unity_app_connected");
							return;
						} else if (message.Equals("disconnected")) {
							Debug.Log("connection:disconnected");
							connectedToConsole = false;
						} else if (message.Equals("close")) {
							Debug.Log("connection:close");
							Application.Quit();
						} else {
							recvDataInvalid = true;
						}
					} else if (topic.Equals("participant")) {
						Debug.Log("participant:" + message);
						ScenarioSettingsManager.Instance.SetParticipant(message); // int.Parse(message)
					} else if (topic.Equals("condition")) {
						Debug.Log("condition:" + message);
						ScenarioSettingsManager.Instance.SetCondition(message); // int.Parse(message)
					} else if (topic.Equals("startStop")) {
						if (message.Equals("stop")) {
							Debug.Log("startStop:stop");
							WriteToLog("StopScenario");
							ScenarioSettingsManager.Instance.StopScenario();
						} else if (message.Equals("start")) {
							Debug.Log("startStop:start");
							WriteToLog("StartScenario");
							ScenarioSettingsManager.Instance.StartScenario();
						} else if (message.Equals("pause")) {
							Debug.Log("startStop:pause");
							WriteToLog("PauseScenario");
							ScenarioSettingsManager.Instance.PauseScenario();
						} else if (message.Equals("resume")) {
							Debug.Log("startStop:resume");
							WriteToLog("ResumeScenario");
							ScenarioSettingsManager.Instance.ResumeScenario();
						} else {
							recvDataInvalid = true;
						}
					} else if (topic.Equals("cameraPositionY")) {
						if (message.Equals("+")) {
							ScenarioSettingsManager.Instance.UpdateCameraPosition(increase: true);
						} else if (message.Equals("-")) {
							ScenarioSettingsManager.Instance.UpdateCameraPosition(increase: false);
						} else if (message.Equals("reset")) {
							ScenarioSettingsManager.Instance.ResetCameraPosition();
						} else {
							recvDataInvalid = true;
						}
					} else {
						recvDataInvalid = true;
					}

					// send message back to server to inform server if the received command was ok
					if (recvDataInvalid) {
						SendRemoteMessage("err_topic_or_message_invalid");
					} else {
						SendRemoteMessage("OK");
					}
				}
			} catch (Exception err) {
				//print(err.Message);
			}
		}

		private void OnDestroy() {
			SendRemoteMessage("unity_app_closed");

			WriteToLog("Stopping Unity UDP Client ...");
			WriteToLog("================================================");

			// close client
			if (client != null) {
				client.Close();
			}

			// close streamwriter for logging
			if (streamWriter != null) {
				streamWriter.Close();
			}
		}

		public void ScenarioFinished() {
			SendRemoteMessage("condition_finished");
		}

		private void SendRemoteMessage(string msg) {
			try {
				byte[] data = Encoding.UTF8.GetBytes(msg);

				// send text to remote client
				client.Send(data, data.Length, remoteEndPoint);
			} catch (Exception e) {
				print(e.Message.ToString());
			}
		}

		private void WriteToLog(string text) {
			streamWriter.WriteLine(text);
		}
	}
}