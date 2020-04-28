using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

	// holds a game object as value, with additional parameters
	public class GameObjectValue : TrackedObjectValue {

		public GameObject value;

		public bool trackPosition = false;
		public bool trackLocalPosition = false;
		public bool trackRotation = false;
		public bool trackLocalRotation = false;
	}
}