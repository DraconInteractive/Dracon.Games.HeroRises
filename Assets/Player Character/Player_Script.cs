using UnityEngine;
using System.Collections;

public class Player_Script : MonoBehaviour {

	public static GameObject playerObj;
	public GameObject controlObj;
	public Control_Script cScript;
	public float rotationAPS;

	public string playerName;
	public int level, currentExp, maxExp, expAdd;

	void Awake () {
		playerObj = this.gameObject;
	}
	// Use this for initialization
	void Start () {
		controlObj = Control_Script.controlObj;
		cScript = controlObj.GetComponent<Control_Script>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LevelUp(){
		level++;
		currentExp -= maxExp;
		expAdd = Mathf.RoundToInt(expAdd * 1.15f);
		maxExp = Mathf.RoundToInt(maxExp * 1.5f);

		if (level == 5){
			if (cScript != null){
				cScript.SetQuestActive("QU01");
			} else {
				print ("Control Script not properly accessed");
			}
		}
	}

	public void AddExp(int amount){
		currentExp += amount;
		CheckLevelStatus();
	}

	private void CheckLevelStatus(){
		if (currentExp >= maxExp){
			LevelUp();
		}
	}
}
