using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

	// key-value pair for data to be tracked by CSVPersistenceManager
	public class TrackedData {
		// key
		public string key;
		// value
		public TrackedObjectValue value;

		public TrackedData() {
			
		}

		public TrackedData(string key, TrackedObjectValue value) {
			this.key = key;
			this.value = value;
		}
	}
}
