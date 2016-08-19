using UnityEngine;
using System.Collections;

public class DojoWinScript : MonoBehaviour {
	private Control_Script c;

	// Use this for initialization
	void Start () {
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		c.dojoWindow = this.gameObject;
		gameObject.SetActive (false);
	}

	public void OpenDojo(){
		c.ExecuteAction (Control_Script.actions.TRAIN);
		c.PlaySound (Control_Script.audioThing.SELECT);

	}

	public void GoToMain(){
		c.SetActiveWindow (Control_Script.windows.NONE);
		c.PlaySound (Control_Script.audioThing.SELECT);

	}
}
