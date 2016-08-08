using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldMTButtonScript : MonoBehaviour {
	public int cost;
	private string initText;
	private Text myText;
	private Button myButton;
	Control_Script c;

	void Start () {
		myText = transform.GetChild (0).gameObject.GetComponent<Text> ();
		myButton = GetComponent<Button> ();
		initText = myText.text;
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		StartCoroutine ("CheckGems");

	}

	public void DisplayGold(int i){
		myText.text = "Gold: " + Control_Script.controlObj.GetComponent<Control_Script> ().GetGoldTierRation(i).ToString();
	}

	public void EndDisplayGold(){
		myText.text = initText;
	}

	public IEnumerator CheckGems (){
		if (cost > c.gemObj.itemQuantity){
			myButton.interactable = false;
		} else {
			myButton.interactable = true;
		}

		yield return new WaitForSeconds (0.5f);

		StartCoroutine ("CheckGems");
	}
}
