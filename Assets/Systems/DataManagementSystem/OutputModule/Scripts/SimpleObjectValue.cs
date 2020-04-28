using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

	// value wrapper for bool, float, int; for passing simple values as reference, not as value
	public class SimpleValueWrapper<T> where T : struct {
		public T value { get; set; }
		public SimpleValueWrapper(T value) { this.value = value; }
	}

	// value wrapper for strings; for passing strings as reference, not as value
	public class StringWrapper {
		public string value { get; set; }
		public StringWrapper(string value) { this.value = value; }
	}

	// holds an object as value
	public class SimpleObjectValue : TrackedObjectValue {

		// SimpleValueWrapper<bool>, SimpleValueWrapper<int>, SimpleValueWrapper<float>, StringWrapper
		public object value;

		public SimpleObjectValue(object value) {
			this.value = value;
		}
	}
}