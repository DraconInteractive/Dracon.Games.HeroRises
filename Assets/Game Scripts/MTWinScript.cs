using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MTWinScript : MonoBehaviour {
	private Control_Script c;

	public Button toMainButton;

	public Button gOne, gTwo, gThree, gFour;
	public Button gemOne, gemTwo, gemThree, gemFour;
	// Use this for initialization
	void Start () {
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		toMainButton.onClick.AddListener (() => GoToMain ());

		gOne.onClick.AddListener (() => c.BuyGold (1));
		gTwo.onClick.AddListener (() => c.BuyGold (2));
		gThree.onClick.AddListener (() => c.BuyGold (3));
		gFour.onClick.AddListener (() => c.BuyGold (4));

		gemOne.onClick.AddListener (() => c.BuyGems (1));
		gemTwo.onClick.AddListener (() => c.BuyGems (2));
		gemThree.onClick.AddListener (() => c.BuyGems (3));
		gemFour.onClick.AddListener (() => c.BuyGems (4));

		gameObject.SetActive (false);
	}

	private void GoToMain(){
		c.SetActiveWindow (Control_Script.windows.NONE);
		c.PlaySound (Control_Script.audioThing.SELECT);

	}

	public void DisplayGoldValue(GameObject g, int i){
		print ("Displaying value");
	}

	public void StopDisplayGoldValue(GameObject g, int i){
		print ("Displaying value");
	}

	public void DisplayGemValue(GameObject g, int i){
		print ("Displaying value");
	}

	public void StopDisplayGemValue(GameObject g, int i){
		print ("Displaying value");
	}
}
