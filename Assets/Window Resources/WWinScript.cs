using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WWinScript : MonoBehaviour {
	public Button sabButton, twoHButton, dwButton, mtButton;
	// Use this for initialization
	void Start () {
		GameObject controlObj = Control_Script.controlObj;
		Control_Script c = controlObj.GetComponent<Control_Script>();

		c.sabButton = sabButton;
		c.twoHButton = twoHButton;
		c.dwButton = dwButton;

		sabButton.onClick.AddListener(() => c.SabButton());
		twoHButton.onClick.AddListener(() => c.TwoHButton());
		dwButton.onClick.AddListener(() => c.DWButton());

		mtButton.onClick.AddListener(() => c.MTBUTTON());
	}
		
}
