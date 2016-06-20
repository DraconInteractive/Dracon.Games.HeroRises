using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bottom_Menu_Script : MonoBehaviour {

	private GameObject controlObj;
	private Control_Script cScript;

	public Button characterButton, actionButton, weaponsButton, armourButton, menuButton;
	public Text goldText;
	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		cScript = controlObj.GetComponent<Control_Script>();

		cScript.bottomMenu = this.gameObject.GetComponent<Bottom_Menu_Script>();
		characterButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.CHARACTER));
		actionButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.ACTION));
		weaponsButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.WEAPONS));
		armourButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.ARMOUR));
		menuButton.onClick.AddListener(() => cScript.SetActiveWindow(Control_Script.windows.MENU));

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
