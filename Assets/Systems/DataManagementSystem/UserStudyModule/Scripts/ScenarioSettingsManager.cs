using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

	public class ScenarioSettingsManager : MonoBehaviour {

		// Singleton - ScenarioSettingsManager should only be available once per scene
		#region Static Interface

		private static ScenarioSettingsManager _instance;

		public static ScenarioSettingsManager Instance {
			get {
				return _instance;
			}
		}

		private ScenarioSettingsManager() {

		}

		#endregion

		#region Properties

		// Screens
		[SerializeField]
		private GameObject loadingScreen;
		[SerializeField]
		private GameObject pleaseAnswerQuestionsScreen;

		// EgoVehicle - Seating
		[SerializeField]
		private float cameraPositionY = 0f;
		[SerializeField]
		private GameObject currentRig = null;
		[SerializeField]
		private float originPositionYCameraRig = 0f;

		// WSD

		// GENERAL
		// flag if data should be persisted
		[SerializeField]
		private bool persistData = true;
		// scenario/condition id
		[SerializeField]
		private string conditionId = "S0";
		// participant/subject id
		[SerializeField]
		private string participantId = "P00";
		// flag if preview should be shown
		[SerializeField]
		private bool showTutorial = false;

		#endregion

		void Awake() {
			// checks if instance already exists
			if (_instance == null) {
				_instance = this;
			} else if (_instance != this) {
				// enforces singleton pattern -> there can only ever be one instance this class
				Destroy(gameObject);
			}
		}

		// Use this for initialization
		void Start() {
			ResetVariables();
		}

		// Update is called once per frame
		void Update() {

		}

		public void ResetCameraPosition() {
			cameraPositionY = originPositionYCameraRig;
			currentRig.transform.position = new Vector3(currentRig.transform.position.x, cameraPositionY, currentRig.transform.position.z);
		}

		public void UpdateCameraPosition(bool increase) {
			if (increase) {
				cameraPositionY += 0.02f;
			} else {
				cameraPositionY -= 0.02f;
			}
			currentRig.transform.position = new Vector3(currentRig.transform.position.x, cameraPositionY, currentRig.transform.position.z);
		}

		#region Scenario

		public void SetParticipant(string participantId) {
			// check for a new participant
			if (!this.participantId.Equals(participantId)) {
				ResetVariables();
			}
			this.participantId = participantId;
		}

		public void SetCondition(string conditionId) {
			this.conditionId = conditionId;
		}

		public void StartScenario() {
			SetupScenario();
			loadingScreen.SetActive(false);
			pleaseAnswerQuestionsScreen.SetActive(false);
		}

		public void StopScenario() {
			pleaseAnswerQuestionsScreen.SetActive(true);
		}

		public void PauseScenario() {
			Time.timeScale = 0.0f;
		}

		public void ResumeScenario() {
			Time.timeScale = 1.0f;
		}

		private void SetupScenario() {
			
		}

		private void ResetVariables() {
			ResetCameraPosition();
			loadingScreen.gameObject.SetActive(true);
			pleaseAnswerQuestionsScreen.gameObject.SetActive(false);
		}

		#endregion
	}
}