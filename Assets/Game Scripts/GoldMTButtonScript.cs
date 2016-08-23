using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldMTButtonScript : MonoBehaviour {
	public int cost;
	public int tier;
	private string resultText, costText;
	private Text myText;
	private Button myButton;
	Control_Script c;

	void Start () {
		myText = transform.GetChild (0).gameObject.GetComponent<Text> ();
		myButton = GetComponent<Button> ();
		GetCostGold ();
		GetResultGold ();
		myText.text = resultText;
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		StartCoroutine ("CheckGems");

	}

	public void GetResultGold () {
		resultText = "Gold: " + Control_Script.controlObj.GetComponent<Control_Script> ().GetGoldTierRation(tier).ToString();
	}

	public void GetCostGold () {
		costText = "Cost: " + cost + " Gems";
	}

	public void DisplayResultGold () {
		myText.text = resultText;
	}

	public void DisplayCostGold () {
		myText.text = costText;
	}

//	void OnEnable () {
//		StartCoroutine ("CheckGems");
//	}

//	public void DisplayGold(int i){
////		myText.text = "Gold: " + Control_Script.controlObj.GetComponent<Control_Script> ().GetGoldTierRation(i).ToString();
//		myText.text = costText;
//	}
//
//	public void EndDisplayGold(){
//		myText.text = resultText;
//	}

	public IEnumerator CheckGems (){
		while (true) {
			print ("cg");
			if (cost > c.gemObj.itemQuantity){
				myButton.interactable = false;
			} else {
				myButton.interactable = true;
			}

			yield return new WaitForSeconds (0.5f);
		}


	}
}
