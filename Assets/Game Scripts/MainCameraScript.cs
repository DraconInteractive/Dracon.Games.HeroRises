using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {
	private GameObject controlObj;
	private Control_Script c;

	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		c = controlObj.GetComponent<Control_Script> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
