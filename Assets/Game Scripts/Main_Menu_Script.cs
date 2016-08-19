using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;

public class Main_Menu_Script : MonoBehaviour {
	private GameObject controlObj;
	private Control_Script cScript;

	public GameObject startWindow, stage1, mainCanvas;

	public Button newGameButton, continueButton, quitButton, beginButton;
	public InputField playerNameInput;
	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		cScript = controlObj.GetComponent<Control_Script>();
		cScript.mainMenu = this.gameObject;

		newGameButton.onClick.AddListener(() => NewGame());
		continueButton.onClick.AddListener(() => ContinueGame());
		quitButton.onClick.AddListener(() => cScript.QuitGame());
		beginButton.onClick.AddListener(() => BeginGame());

		mainCanvas.GetComponent<Canvas> ().enabled = false;
		startWindow.SetActive(false);
	}

	public void NewGame(){
		startWindow.SetActive(true);
		stage1.SetActive (false);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	private void ContinueGame(){
		
		cScript.LoadGameVariables();
		cScript.gameActive = true;
		mainCanvas.GetComponent<Canvas> ().enabled = true;
		GameObject.FindGameObjectWithTag ("BlurCam").SetActive (false);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

		this.gameObject.SetActive(false);
	}

	private void BeginGame(){

		Analytics.CustomEvent("BeginNewGame", new Dictionary<string, object>
			{
				{ "playerName", playerNameInput.text }
			});
		
		Player_Script.playerObj.GetComponent<Player_Script>().playerName = playerNameInput.text;
		cScript.LoadStartingGameVariables();
		cScript.SaveGameVariables();
		cScript.gameActive = true;
		startWindow.SetActive(false);
		mainCanvas.GetComponent<Canvas> ().enabled = true;
		GameObject.FindGameObjectWithTag ("BlurCam").SetActive (false);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

		this.gameObject.SetActive (false);
	}

	void OnEnable () {
		mainCanvas.GetComponent<Canvas> ().enabled = false;
		stage1.SetActive (true);
	}
}
