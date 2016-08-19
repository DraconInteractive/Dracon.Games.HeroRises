using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MWinScript : MonoBehaviour {

	public Button quitButton, mainMenuButton, newGameButton, mtButtonMenu;
	public Button toMainButton;

	public GameObject confirmPanel;
	public Button confirmButton, noButton;

	private Control_Script cScript;
	// Use this for initialization
	void Start () {
		cScript = Control_Script.controlObj.GetComponent<Control_Script>();
		quitButton.onClick.AddListener(() => cScript.QuitGame());
		mainMenuButton.onClick.AddListener(() => cScript.ReturnToMainMenu());
		newGameButton.onClick.AddListener(() => ActivateConfirmationWindow());
		confirmButton.onClick.AddListener (() => NewGame ());
		noButton.onClick.AddListener (() => GoToMain ());
		mtButtonMenu.onClick.AddListener(() => cScript.MTBUTTON());
		toMainButton.onClick.AddListener (() => GoToMain ());
		gameObject.SetActive(false);
	}

	private void NewGame(){
		confirmPanel.SetActive (false);
		cScript.ReturnToMainMenu();
		cScript.mainMenu.GetComponent<Main_Menu_Script>().NewGame();
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	private void ActivateConfirmationWindow () {
		confirmPanel.SetActive (true);
	}

	private void GoToMain(){
		cScript.SetActiveWindow (Control_Script.windows.NONE);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}
}
