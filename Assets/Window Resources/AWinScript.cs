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

		cScript.energySlider = energySlider;

		energySlider.minValue = 0;
		energySlider.maxValue = 1;
		energySlider.interactable = false;

		workButton.onClick.AddListener(() => DoWorkAction());
		trainButton.onClick.AddListener(() => DoTrainAction());
		exploreButton.onClick.AddListener(() => DoExploreAction());
		restButton.onClick.AddListener(() => DoRestAction());
		toMainButton.onClick.AddListener (() => GoToMain ());

		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	private void GoToMain(){
		cScript.SetActiveWindow (Control_Script.windows.NONE);
	}

	private void DoWorkAction(){
		cScript.ExecuteAction(Control_Script.actions.WORK);
	}

	private void DoTrainAction(){
		cScript.ExecuteAction(Control_Script.actions.TRAIN);
	}

	private void DoExploreAction(){
		cScript.ExecuteAction(Control_Script.actions.EXPLORE);
	}

	private void DoRestAction(){
		cScript.ExecuteAction(Control_Script.actions.REST);
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
