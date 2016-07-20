using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldMTButtonScript : MonoBehaviour {
	private string initText;
	private Text myText;

	void Start () {
		myText = transform.GetChild (0).gameObject.GetComponent<Text> ();
		initText = myText.text;
	}

	public void DisplayGold(int i){
		myText.text = "Gold: " + Control_Script.controlObj.GetComponent<Control_Script> ().GetGoldTierRation(i).ToString();
	}

	public void EndDisplayGold(){
		myText.text = initText;
	}
}
