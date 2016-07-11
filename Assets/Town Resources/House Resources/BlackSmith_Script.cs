using UnityEngine;
using System.Collections;

public class BlackSmith_Script : MonoBehaviour {
	Material myMaterial;
	Color startingColor;
	GameObject controlObj;
	// Use this for initialization
	void Start () {
		myMaterial = GetComponent<MeshRenderer>().material;
		startingColor = myMaterial.color;
		controlObj = Control_Script.controlObj;
	}

	void OnMouseEnter(){
		myMaterial.color = startingColor + new Color(0.25f,0.25f,0.25f);
	}

	public void OnMouseExit(){
		myMaterial.color = startingColor;
	}

	void OnMouseDown(){
		controlObj.GetComponent<Control_Script>().SetActiveWindow(Control_Script.windows.ARMOUR);
	}
}
