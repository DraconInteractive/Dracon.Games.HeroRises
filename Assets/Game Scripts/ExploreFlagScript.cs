using UnityEngine;
using System.Collections;

public class ExploreFlagScript : MonoBehaviour {

	Material myMaterial;
	Color startingColor;
	GameObject controlObj;
	Control_Script c;
	// Use this for initialization
	void Start () {
		myMaterial = GetComponent<MeshRenderer>().material;
		startingColor = myMaterial.color;
		controlObj = Control_Script.controlObj;
		c = controlObj.GetComponent <Control_Script> ();
	}

	void OnMouseEnter(){
		if (!c.uiActive){
			myMaterial.color = startingColor + new Color(0.25f,0.25f,0.25f);
		}

	}

	public void OnMouseExit(){
		if (!c.uiActive){
			myMaterial.color = startingColor;
		}

	}

	void OnMouseDown(){
		if (!c.uiActive){
			controlObj.GetComponent<Control_Script>().ExecuteAction(Control_Script.actions.EXPLORE);
		}

	}
}
