using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AWinScript : MonoBehaviour {

	public Button workButton, trainButton, exploreButton, restButton;
	public Slider energySlider;
	public Button toMainButton;

	private Control_Script cScript;

	// Use this for initialization
	void Start () {
		cScript = Control_Script.controlObj.GetComponent<Control_Script>();

		workButton.onClick.AddListener(() => DoWorkAction());
		trainButton.onClick.AddListener(() => DoTrainAction());
		exploreButton.onClick.AddListener(() => DoExploreAction());
		toMainButton.onClick.AddListener (() => GoToMain ());

		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	private void GoToMain(){
		cScript.SetActiveWindow (Control_Script.windows.NONE);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	private void DoWorkAction(){
		cScript.ExecuteAction(Control_Script.actions.WORK);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	private void DoTrainAction(){
		cScript.ExecuteAction(Control_Script.actions.TRAIN);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	private void DoExploreAction(){
		cScript.ExecuteAction(Control_Script.actions.EXPLORE);
		cScript.PlaySound (Control_Script.audioThing.SELECT);

	}

	public void SetEnergy(bool energyLeft){
		if (energyLeft){
			workButton.interactable = true;
			trainButton.interactable = true;
			exploreButton.interactable = true;
		} else {
			workButton.interactable = false;
			trainButton.interactable = false;
			exploreButton.interactable = false;
		}
	}
}
