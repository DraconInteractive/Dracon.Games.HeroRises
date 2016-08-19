using UnityEngine;
using System.Collections;

public class AreaObjectScript : MonoBehaviour {

	public int aNum;
	public int areaLevel;
	public int completed;

	public void LoadArea (){
		completed = PlayerPrefs.GetInt ("ACI" + aNum);
	}

	public void SaveArea () {
		PlayerPrefs.SetInt ("ACI" + aNum, completed);
	}

	public void LoadDefaultArea () {
		PlayerPrefs.SetInt ("ACI" + aNum, 0);
		print ("Loaded Default Area");
	}
}
