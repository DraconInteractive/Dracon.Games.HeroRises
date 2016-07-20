using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CWinScript : MonoBehaviour {

	public Text nameText, levelText, expText, dayText, jobLevelText, weaponText, armourText;
	public Button toMainButton;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
		toMainButton.onClick.AddListener (() => GoToMain ());
	}

	private void GoToMain(){
		Control_Script.controlObj.GetComponent<Control_Script> ().SetActiveWindow (Control_Script.windows.NONE);
	}

}
