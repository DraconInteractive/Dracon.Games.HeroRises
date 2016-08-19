using UnityEngine;
using System.Collections;

public class Window_Script : MonoBehaviour {

	private GameObject controlObj;
	private Control_Script cScript;
	public enum windowType {CHARACTER, ACTION, MENU, WEAPONS, ARMOUR, MICROTRANSACTIONS, FEEDBACK};
	public windowType myWindowType;

	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		cScript = controlObj.GetComponent<Control_Script>();
		switch (myWindowType)
		{
		case windowType.CHARACTER:
			cScript.characterWindow = this.gameObject;
			break;
		case windowType.MENU:
			cScript.menuWindow = this.gameObject;
			break;
		case windowType.WEAPONS:
			cScript.weaponsWindow = this.gameObject;
			break;
		case windowType.ARMOUR:
			cScript.armourWindow = this.gameObject;
			break;
		case windowType.FEEDBACK:
			cScript.feedbackWindow = this.gameObject;
			break;
		case windowType.MICROTRANSACTIONS:
			cScript.mtWindow = this.gameObject;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
