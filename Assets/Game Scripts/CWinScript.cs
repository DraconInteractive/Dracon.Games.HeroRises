using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CWinScript : MonoBehaviour {

	public Text nameText, levelText, expText, dayText, jobLevelText, weaponText, armourText;
	public Button toMainButton;
	public Image weaponImage;
	public Sprite[] weaponSprites;

	// Use this for initialization
	void Start () {
		toMainButton.onClick.AddListener (() => GoToMain ());
		gameObject.SetActive(false);

	}

	private void GoToMain(){
		Control_Script.controlObj.GetComponent<Control_Script> ().SetActiveWindow (Control_Script.windows.NONE);
		Control_Script.controlObj.GetComponent<Control_Script>().PlaySound (Control_Script.audioThing.SELECT);

	}

}
