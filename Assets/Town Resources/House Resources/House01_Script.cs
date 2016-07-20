using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class House01_Script : MonoBehaviour {
	Material[] myMaterials;
	Color[] startingColor;
	GameObject controlObj;
	// Use this for initialization
	void Start () {
		myMaterials = GetComponent<MeshRenderer>().materials;
		startingColor = new Color[myMaterials.Length];
		foreach (Color c in startingColor){
			startingColor[System.Array.IndexOf(startingColor, c)] = myMaterials [System.Array.IndexOf (startingColor, c)].color;
		}
		controlObj = Control_Script.controlObj;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter(){
		foreach (Material m in myMaterials){
			m.color = startingColor[System.Array.IndexOf(myMaterials, m)] + new Color(0.25f,0.25f,0.25f);
		}

	}

	public void OnMouseExit(){
		foreach (Material m in myMaterials){
			m.color = startingColor[System.Array.IndexOf(myMaterials, m)];
		}
	}

	void OnMouseDown(){
		controlObj.GetComponent<Control_Script>().SetActiveWindow(Control_Script.windows.WEAPONS);
	}
}
