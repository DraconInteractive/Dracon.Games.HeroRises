using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ARWinScript : MonoBehaviour {

	private GameObject controlObj;
	private Control_Script c;
	public Button tOneA, tTwoA, tThreeA, mtButton;
	public Button toMainButton;

	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		c = controlObj.GetComponent<Control_Script>();

		c.tOneAB = tOneA;
		c.tTwoAB = tTwoA;
		c.tThreeAB = tThreeA;

		tOneA.onClick.AddListener(() => c.TOneAButton());
		tTwoA.onClick.AddListener(() => c.TTwoAButton());
		tThreeA.onClick.AddListener(() => c.TThreeAButton());
		mtButton.onClick.AddListener(() => c.MTBUTTON());
		toMainButton.onClick.AddListener (() => c.SetActiveWindow (Control_Script.windows.NONE));

		gameObject.SetActive (false);
	}
	

}
