using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bottom_Menu_Script : MonoBehaviour {

	private GameObject controlObj;
	private Control_Script cScript;

	public GameObject advertisementPanel, tutorialPanelOne, tutorialPanelTwo, tutorialPanelThree;

	public Button characterButton, actionButton, menuButton;
	public Button tutButton1, tutButton2, tutButton3;

	public Text goldText;
	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		cScript = controlObj.GetComponent<Control_Script>();

		cScript.bottomMenu = this.gameObject.GetComponent<Bottom_Menu_Script>();
		characterButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.CHARACTER));
		menuButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.MENU));

		tutButton1.onClick.AddListener (() => TutButtonOne ());
		tutButton2.onClick.AddListener (() => TutButtonTwo ());
		tutButton3.onClick.AddListener (() => TutButtonThree ());

		cScript.adPanel = advertisementPanel;

	}
	
	// Update is called once per frame
	public void TutButtonOne () {
		tutorialPanelOne.SetActive (false);
		tutorialPanelTwo.SetActive (true);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	public void TutButtonTwo () {
		tutorialPanelTwo.SetActive (false);
		tutorialPanelThree.SetActive (true);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	public void TutButtonThree () {
		tutorialPanelThree.SetActive (false);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}
}
