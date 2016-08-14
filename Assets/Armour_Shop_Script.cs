using UnityEngine;
using System.Collections;

public class Armour_Shop_Script : MonoBehaviour {

	Material[] myMaterials;
	Color[] startingColor;
	GameObject controlObj;
	Control_Script cScript;
	// Use this for initialization
	void Start () {
		myMaterials = GetComponent<MeshRenderer>().materials;
		startingColor = new Color[myMaterials.Length];
		foreach (Color c in startingColor){
			startingColor[System.Array.IndexOf(startingColor, c)] = myMaterials [System.Array.IndexOf (startingColor, c)].color;
		}
		controlObj = Control_Script.controlObj;
		cScript = controlObj.GetComponent<Control_Script> ();
	}


	void OnMouseEnter(){
		if (!cScript.uiActive){
			foreach (Material m in myMaterials){
				m.color = startingColor[System.Array.IndexOf(myMaterials, m)] + new Color(0.25f,0.25f,0.25f);
			}
		}
	}

	public void OnMouseExit(){

		foreach (Material m in myMaterials){
			m.color = startingColor[System.Array.IndexOf(myMaterials, m)];
		}

	}

	void OnMouseDown(){
		if (!cScript.uiActive){
			controlObj.GetComponent<Control_Script>().SetActiveWindow(Control_Script.windows.ARMOUR);
		}
	}
}
