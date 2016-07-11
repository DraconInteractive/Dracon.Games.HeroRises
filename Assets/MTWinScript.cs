using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MTWinScript : MonoBehaviour {

	public Button toMainButton;
	// Use this for initialization
	void Start () {
		toMainButton.onClick.AddListener (() => GoToMain ());
	}

	private void GoToMain(){
		Control_Script.controlObj.GetComponent<Control_Script> ().SetActiveWindow (Control_Script.windows.NONE);
	}
}
