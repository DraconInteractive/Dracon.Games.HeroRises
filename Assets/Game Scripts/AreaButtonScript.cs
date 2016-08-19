using UnityEngine;
using System.Collections;

public class AreaButtonScript : MonoBehaviour {
	public GameObject areaObject, areaControl;
	public int areaNum;

	public void LoadArea(){
		areaObject.SetActive (true);
		areaControl.SetActive (false);
		Control_Script.controlObj.GetComponent<Control_Script> ().activeArea = areaNum;
	}
}
