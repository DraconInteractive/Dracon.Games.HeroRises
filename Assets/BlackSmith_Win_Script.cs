using UnityEngine;
using System.Collections;

public class BlackSmith_Win_Script : MonoBehaviour {
	private Control_Script c;

	// Use this for initialization
	void Start () {
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		c.blacksmithWindow = this.gameObject;
		gameObject.SetActive (false);
	}

	public void OpenBlacksmith () {
		c.ExecuteAction (Control_Script.actions.WORK);
	}

	public void GoToMain () {
		Control_Script.controlObj.GetComponent<Control_Script> ().SetActiveWindow (Control_Script.windows.NONE);
	}

}
