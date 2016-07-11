using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MWinScript : MonoBehaviour {

	public Button quitButton, mainMenuButton, newGameButton, mtButtonMenu;
	public Button toMainButton;

	private Control_Script cScript;
	// Use this for initialization
	void Start () {
		cScript = Control_Script.controlObj.GetComponent<Control_Script>();
		quitButton.onClick.AddListener(() => cScript.QuitGame());
		mainMenuButton.onClick.AddListener(() => cScript.ReturnToMainMenu());
		newGameButton.onClick.AddListener(() => NewGame());
		mtButtonMenu.onClick.AddListener(() => cScript.MTBUTTON());
		toMainButton.onClick.AddListener (() => GoToMain ());
		gameObject.SetActive(false);
	}

	private void NewGame(){
		cScript.ReturnToMainMenu();
		cScript.mainMenu.GetComponent<Main_Menu_Script>().NewGame();
	}

	private void GoToMain(){
		cScript.SetActiveWindow (Control_Script.windows.NONE);
	}
}
