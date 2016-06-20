using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Main_Menu_Script : MonoBehaviour {
	private GameObject controlObj;
	private Control_Script cScript;

	public GameObject startWindow;

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

		startWindow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewGame(){
		startWindow.SetActive(true);
	}

	private void ContinueGame(){
		cScript.LoadGameVariables();
		cScript.gameActive = true;
		this.gameObject.SetActive(false);
	}

	private void BeginGame(){
		Player_Script.playerObj.GetComponent<Player_Script>().playerName = playerNameInput.text;
		cScript.LoadStartingGameVariables();
		cScript.SaveGameVariables();
		cScript.gameActive = true;
		startWindow.SetActive(false);
		this.gameObject.SetActive(false);
	}
}
