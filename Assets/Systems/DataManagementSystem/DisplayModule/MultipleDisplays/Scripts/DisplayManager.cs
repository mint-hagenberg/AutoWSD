using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

    public class DisplayManager : MonoBehaviour {

	    // Singleton - DisplayManager should only be available once per scene
	    #region Static Interface

	    private static DisplayManager _instance;

	    public static DisplayManager Instance {
		    get {
			    return _instance;
		    }
	    }

	    private DisplayManager() {

	    }

	    #endregion

	    #region Properties

	    [SerializeField]
	    private Camera[] cameras;

	    void Awake() {
		    // checks if instance already exists
		    if (_instance == null) {
			    _instance = this;
		    } else if (_instance != this) {
			    // enforces singleton pattern -> there can only ever be one instance this class
			    Destroy(gameObject);
		    }
	    }

	    // Start is called before the first frame update
	    void Start() {

	    }

	    #endregion

	    // Update is called once per frame
	    void Update() {
		    Debug.Log("Displays connected: " + Display.displays.Length);
		    // Display.displays[0] is the primary, default display and is always ON.
		    for (int i = 1; i < cameras.Length; i++) {
			    //cameras[i].targetDisplay = i; // set the display in which to render the camera to
			    Display.displays[i].Activate();
		    }
	    }
    }
}