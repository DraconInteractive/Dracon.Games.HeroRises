using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemMTButtonScript : MonoBehaviour {

	private string initText;
	private Text myText;

	void Start () {
		myText = transform.GetChild (0).gameObject.GetComponent<Text> ();
		initText = myText.text;
	}

	public void DisplayGem(int i){
		myText.text = "Gems: " + Control_Script.controlObj.GetComponent<Control_Script> ().GetGemTierRation (i);
	}

	public void EndDisplayGem(){
		myText.text = initText;
	}
}
