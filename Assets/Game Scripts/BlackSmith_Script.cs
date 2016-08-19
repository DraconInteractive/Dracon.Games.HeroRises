using UnityEngine;
using System.Collections;

public class BlackSmith_Script : MonoBehaviour {
	public Material[] myMats;
	private Color[] myStartingColor;
	GameObject controlObj;
	Control_Script c;
	// Use this for initialization
	void Start () {
		myStartingColor = new Color[myMats.Length];
		for (int i = 0; i < myMats.Length; i++){
			myStartingColor[i] = myMats[i].color;
		}
			
		controlObj = Control_Script.controlObj;
		c = controlObj.GetComponent <Control_Script> ();
	}

	void OnMouseEnter(){
		if (!c.uiActive){
			for (int i = 0; i < myMats.Length; i++){
				myMats[i].color = myStartingColor[i] + new Color(0.25f,0.25f,0.25f);
			}
		}

	}

	public void OnMouseExit(){
		for (int i = 0; i < myMats.Length; i++){
			myMats[i].color = myStartingColor[i];
		}
	}

	void OnMouseDown(){
		if (!c.uiActive){
			c.SetActiveWindow(Control_Script.windows.BLACKSMITH);
			c.PlaySound (Control_Script.audioThing.SELECT);

		}

	}
}
