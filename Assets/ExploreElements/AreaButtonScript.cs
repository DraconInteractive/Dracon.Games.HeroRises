using UnityEngine;
using System.Collections;

public class AreaButtonScript : MonoBehaviour {
	public GameObject areaObject, areaControl;

	public void LoadArea(){
		areaObject.SetActive (true);
		areaControl.SetActive (false);
	}
}
