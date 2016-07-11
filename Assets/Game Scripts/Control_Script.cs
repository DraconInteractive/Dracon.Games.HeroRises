using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Control_Script : MonoBehaviour {
	
	//Setup
	public static GameObject controlObj;
	public GameObject mainMenu;
	public GameObject bottomMenuCanvasObject;

	//Game
	public bool gameActive;
	public int day;
	public float dayTime;
	public int dayGoldEarnt;
	public int dayXPEarnt;

	//Windows
	public Bottom_Menu_Script bottomMenu;
	public GameObject characterWindow, actionWindow, menuWindow, weaponsWindow, armourWindow, feedbackWindow, mtWindow;

	public enum windows {CHARACTER, ACTION, MENU, WEAPONS, ARMOUR, FEEDBACK, MICROTRANSACTIONS, NONE};
	public windows activeWindow;

	public Button sabButton, twoHButton, dwButton;
	public Button tOneAB, tTwoAB, tThreeAB;
	public Button mtButtonMenu;

	//Player
	private GameObject playerObj;
	private Player_Script pScript;

	private InventoryObject eWeapon, eArmour;
	private int armourStage;

	//Actions

	public enum actions {WORK, TRAIN, EXPLORE, REST};
	public Slider energySlider;
	public int jobLevel, jobCXP, jobMXP, jobEXPAdd;

	//Inventory

	public InventoryObject[] inventory;
	public InventoryObject goldObj;

	//Quest

	public QuestObject[] allQuests;
	public List<QuestObject> activeQuests;

	//Minigames

	public enum minigames {WORK, TRAIN, EXPLORE, NONE};
	public minigames activeGame;
	private bool minigameActive;
	public GameObject wMiniObject, tMiniObject, eMiniObject;

	public LayerMask mainLayer, wLayer, tLayer, eLayer;

	private enum controlSchemes {WSCHEME, TSCHEME, ESCHEME, NONE};
	private controlSchemes activeScheme;

	private float minigameGoldEarned, minigameXPEarned;

	private Camera mainCam;
	private Vector3 mainCamStartPos;

	//Work
	public GameObject wSphere;
	public GameObject initBlocker, leftBlocker, rightBlocker;
	private float wInitTimer;
	public float wInitTimerMaxStart, wInitTimerMax;
	private float ballFalling;

	//Train
	public GameObject trainArrow;
	public float trainTimerCurrent, trainTimerMax, trainTimerMaxStart;
	public Slider timeSlider;
	//Debug
	public int invIDMax;

	//Feedback
	public Text goldAddText, expAddText, levelCurrentText, expCurrentText, feedbackTitleText;
	public Button redoButton, mainButton;

	#region Basic Functions

	void Awake () {
		controlObj = this.gameObject;
		mainCam = Camera.main;
		mainCamStartPos = mainCam.transform.position;
	}
	// Use this for initialization
	void Start () {
		playerObj = Player_Script.playerObj;
		pScript = playerObj.GetComponent<Player_Script>();
		gameActive = false;

		foreach (InventoryObject i in inventory){
			if (i.itemID > invIDMax){
				invIDMax = i.itemID;
			}
		}

		activeQuests = new List<QuestObject>();

		foreach (QuestObject quest in allQuests){
			if (quest.questStatus == QuestObject.questStatuses.STARTED){
				if (!activeQuests.Contains(quest)){
					activeQuests.Add(quest);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameActive){
			return;
		}

		if (Input.GetKeyDown(KeyCode.F5)){
			SaveGameVariables();
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			if (minigameActive){
				EndMinigame ();
				minigameActive = false;
			}
		}

		if (activeScheme == controlSchemes.WSCHEME){
			WorkInput ();
			WorkUpdate ();
		}

		if (activeScheme == controlSchemes.TSCHEME){
			TrainInput ();
			TrainUpdate ();
		}

	}

	#endregion

	#region ui functions

	public void SetActiveWindow(windows window){
		if (activeWindow == window){
			activeWindow = windows.NONE;
		} else {
			activeWindow = window;
		}
			
		switch (activeWindow)
		{
		case windows.ACTION:

			actionWindow.SetActive(true);

			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			break;
		case windows.CHARACTER:
			
			characterWindow.SetActive(true);

			menuWindow.SetActive(false);
			actionWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			break;
		case windows.MENU:
			
			menuWindow.SetActive(true);

			actionWindow.SetActive(false);
			characterWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			break;
		case windows.WEAPONS:
			weaponsWindow.SetActive(true);

			menuWindow.SetActive(false);
			actionWindow.SetActive(false);
			characterWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			break;
		case windows.ARMOUR:
			armourWindow.SetActive(true);

			actionWindow.SetActive(false);
			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			break;

		case windows.FEEDBACK:
			feedbackWindow.SetActive(true);

			actionWindow.SetActive(false);
			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			mtWindow.SetActive(false);
			break;
		case windows.MICROTRANSACTIONS:
			mtWindow.SetActive(true);

			actionWindow.SetActive(false);
			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			break;
		case windows.NONE:
			mtWindow.SetActive(false);
			actionWindow.SetActive(false);
			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			break;
		}
	}

	public void MTBUTTON(){
		SetActiveWindow(windows.MICROTRANSACTIONS);
	}

	public void UpdateUI(){
		CWinScript c = characterWindow.GetComponent<CWinScript>();
		c.nameText.text = "Name: " + pScript.playerName;
		c.levelText.text = "Level: " + pScript.level.ToString();
		c.expText.text = "EXP: " + pScript.currentExp.ToString() + " / " + pScript.maxExp.ToString();
		c.dayText.text = "Day: " + day.ToString();
		c.jobLevelText.text = "Job Level: " + jobLevel.ToString();
		bottomMenu.goldText.text = "Gold: " + GetInventoryItem("Gold").itemQuantity;

		energySlider.value = dayTime;

		UpdateEquipmentButtonUI();
	}

	public void RedoButton(){
		SetActiveWindow(windows.ACTION);
	}

	public void ReturnToMainMenu(){
		SaveGameVariables();
		mainMenu.SetActive(true);
	}

	public void MainButton(){
		SetActiveWindow(windows.NONE);
	}

	#endregion

	#region DataRetainment
	public void QuitGame(){
		SaveGameVariables();

		Application.Quit();
//		if (Application.isEditor){
//			UnityEditor.EditorApplication.isPlaying = false;
//		}

	}

	public void SaveGameVariables(){
		switch (activeWindow)
		{
		case windows.ACTION:
			PlayerPrefs.SetInt("ActiveWindow", 0);
			break;
		case windows.CHARACTER:
			PlayerPrefs.SetInt("ActiveWindow", 1);
			break;
		case windows.MENU:
			PlayerPrefs.SetInt("ActiveWindow", 2);
			break;
		case windows.WEAPONS:
			PlayerPrefs.SetInt("ActiveWindow", 3);
			break;
		case windows.ARMOUR:
			PlayerPrefs.SetInt("ActiveWindow", 4);
			break;
		case windows.FEEDBACK:
			PlayerPrefs.SetInt("ActiveWindow", 5);
			break;
		case windows.NONE:
			PlayerPrefs.SetInt("ActiveWindow", 6);
			break;
		}

		PlayerPrefs.SetString("PlayerName", pScript.playerName);
		PlayerPrefs.SetInt("PlayerLevel", pScript.level);
		PlayerPrefs.SetInt("PlayerCEXP", pScript.currentExp);
		PlayerPrefs.SetInt("PlayerMEXP", pScript.maxExp);
		PlayerPrefs.SetInt("ExpAdd", pScript.expAdd);
		PlayerPrefs.SetInt("EWIndex", eWeapon.itemID);
		PlayerPrefs.SetInt("EAIndex", eArmour.itemID);
		PlayerPrefs.SetInt("ArmourStage", armourStage);

		PlayerPrefs.SetInt("Day", day);
		PlayerPrefs.SetFloat("DayTime", dayTime);

		PlayerPrefs.SetInt("JobLevel", jobLevel);
		PlayerPrefs.SetInt("JobCXP", jobCXP);
		PlayerPrefs.SetInt("JobMXP", jobMXP);
		PlayerPrefs.SetInt("JobXPAdd", jobEXPAdd);

		foreach (InventoryObject item in inventory){
			item.SaveItemDetails();
		}

		foreach (QuestObject quest in allQuests){
			quest.SaveQuestStatus();
		}

		UpdateUI();
//		UploadStatisticsToAnalytics ();
//		SaveToXML ();
	}

	public void LoadGameVariables(){
		int i = PlayerPrefs.GetInt("ActiveWindow");
		switch (i)
		{
		case 0:
			SetActiveWindow(windows.ACTION);
			break;
		case 1:
			SetActiveWindow(windows.CHARACTER);
			break;
		case 2:
			SetActiveWindow(windows.MENU);
			break;
		case 3:
			SetActiveWindow(windows.WEAPONS);
			break;
		case 4:
			SetActiveWindow(windows.ARMOUR);
			break;
		case 5:
			SetActiveWindow(windows.FEEDBACK);
			break;
		case 6:
			SetActiveWindow(windows.NONE);
			break;
		}

		pScript.playerName = PlayerPrefs.GetString("PlayerName");
		pScript.level = PlayerPrefs.GetInt("PlayerLevel");
		pScript.currentExp = PlayerPrefs.GetInt("PlayerCEXP");
		pScript.maxExp = PlayerPrefs.GetInt("PlayerMEXP");
		pScript.expAdd = PlayerPrefs.GetInt("ExpAdd");
		armourStage = PlayerPrefs.GetInt("ArmourStage");

		int eIndex = PlayerPrefs.GetInt("EWIndex");
		int aIndex = PlayerPrefs.GetInt("EAIndex");
		foreach (InventoryObject item in inventory){

			if (item.itemID == eIndex){
				eWeapon = item;
			}

			if (item.itemID == aIndex){
				eArmour = item;
			}
		}

		day = PlayerPrefs.GetInt("Day");
		dayTime = PlayerPrefs.GetFloat("DayTime");

		jobLevel = PlayerPrefs.GetInt("JobLevel");
		jobCXP = PlayerPrefs.GetInt("JobCXP");
		jobMXP = PlayerPrefs.GetInt("JobMXP");
		jobEXPAdd = PlayerPrefs.GetInt("JobXPAdd");

		foreach (InventoryObject item in inventory){
			item.LoadItemVariables();
		}

		foreach (QuestObject quest in allQuests){
			quest.LoadQuestStatus();
		}

		if (dayTime < 1){
			actionWindow.GetComponent<AWinScript>().SetEnergy(true);
		} else {
			actionWindow.GetComponent<AWinScript>().SetEnergy(false);
		}

		UpdateUI();

	}

	public void LoadStartingGameVariables(){
		SetActiveWindow(windows.NONE);

		pScript.level = 1;
		pScript.currentExp = 0;
		pScript.maxExp = 10;
		pScript.expAdd = 2;
		day = 0;
		dayTime = 0;
		jobLevel = 1;
		jobCXP = 0;
		jobMXP = 10;
		jobEXPAdd = 2;
		armourStage = 1;

		foreach (InventoryObject item in inventory){
			item.ResetItemVariables();
			if (item.type == InventoryObject.itemTypes.TWOH){
				if (item.stage == 0){
					eWeapon = item;
				}
			}

			if (item.type == InventoryObject.itemTypes.AR){
				if (item.stage == 0){
					eArmour = item;
				}
			}
		}

		foreach (QuestObject quest in allQuests){
			quest.ResetQuestStatus();
		}

		UpdateUI();
	}

	private void UploadStatisticsToAnalytics(){
		Player_Script p = playerObj.GetComponent<Player_Script> ();
		Dictionary<string, object> playerStats = new Dictionary<string, object> ();
		playerStats.Add ("PlayerName", p.playerName);
		playerStats.Add ("PlayerLevel", p.level);
		playerStats.Add ("PlayerCurrentEXP", p.currentExp);
		playerStats.Add ("PlayerMaxEXP", p.maxExp);
		playerStats.Add ("Job Level", jobLevel);
		playerStats.Add ("PlayerWeapon", eWeapon.ToString ());
		playerStats.Add ("PlayerArmour", eArmour.ToString ());
		Analytics.SetUserId (SystemInfo.deviceModel);

		Analytics.CustomEvent ("StandardUpload", playerStats);
	}

	private void SaveToXML(){
		string fileName = "";
		fileName = "Save01.xml";

		XmlDocument xml = new XmlDocument ();

		if (File.Exists(Application.persistentDataPath + "/" + fileName)) {
			xml.Load (Application.persistentDataPath + "/" + fileName);
		} else {
			XmlElement root = xml.CreateElement ("GameSessions");
			XmlElement id = xml.CreateElement ("ID");
			id.InnerXml = SystemInfo.deviceUniqueIdentifier;
			root.AppendChild (id);
			xml.AppendChild (root);
		}

		XmlElement session = xml.CreateElement ("Session");

		XmlElement pName = xml.CreateElement ("PlayerName");
		pName.InnerText = playerObj.GetComponent<Player_Script> ().playerName;
		session.AppendChild (pName);

		XmlElement timeStamp = xml.CreateElement ("TimeStamp");
		timeStamp.InnerText = "Hour: " + System.DateTime.Now.Hour.ToString() + "Minute: " + System.DateTime.Now.Minute.ToString() + "Second: " + System.DateTime.Now.Second.ToString();
		session.AppendChild (timeStamp);

		xml.DocumentElement.AppendChild (session);

		xml.Save (Application.persistentDataPath + "/" + fileName);

	}
	#endregion

	#region action functions

	public void ExecuteAction(actions a){
		
		dayTime += 0.25f;
		int expAdd = 0;
		int goldAdd = 0;
		switch (a)
		{
		case actions.EXPLORE:
			expAdd = pScript.expAdd;
			pScript.AddExp (pScript.expAdd);
			goldAdd = Mathf.RoundToInt (pScript.level / Random.Range (0.1f, 2.0f));
			AddGold (goldAdd);
			StartMinigame (minigames.EXPLORE);
			break;
		case actions.REST:
			FinishDay();
			break;
		case actions.TRAIN:
			StartMinigame (minigames.TRAIN);
			break;
		case actions.WORK:
			StartMinigame (minigames.WORK);
			break;
		}

		dayGoldEarnt += goldAdd;
		dayXPEarnt += expAdd;

		if (dayTime < 1){
			actionWindow.GetComponent<AWinScript>().SetEnergy(true);
		} else {
			actionWindow.GetComponent<AWinScript>().SetEnergy(false);
		}


//		SetActiveWindow(windows.FEEDBACK);
//		UpdateFeedbackWindow(a, expAdd, goldAdd);

		UpdateUI();
	}

	public void UpdateFeedbackWindow(actions a, int expAdd, int goldAdd){

		if (a == actions.REST){
			feedbackTitleText.text = a.ToString();
			goldAddText.text = "Day: " + (day - 1).ToString() + " Finished";
			expAddText.text = "";
			expCurrentText.text = "Gold Earnt Today: " + dayGoldEarnt;
			levelCurrentText.text = "Experience Earnt Today: " + dayXPEarnt;
			dayGoldEarnt = 0;
			dayXPEarnt = 0;
		} else {
			feedbackTitleText.text = a.ToString();
			expAddText.text = "Experience Earnt: " + expAdd.ToString();
			goldAddText.text = "Gold Earnt: " + goldAdd.ToString();
			expCurrentText.text = "Current Experience: " + pScript.currentExp.ToString() + " / " + pScript.maxExp.ToString();
			levelCurrentText.text = "Current Level: " + pScript.level.ToString();
		}

	}

	public void FinishDay(){
		day ++;
		dayTime = 0;
		SaveGameVariables();
		UpdateUI();
	}

	#endregion

	#region inventoryManagement

	void AddToInventory(string iName, int amount){
		foreach (InventoryObject i in inventory){
			if (i.itemName == iName){
				i.itemQuantity += amount;
				i.SaveItemDetails();
			}
		}
	}

	void AddGold(int amount){
		goldObj.itemQuantity += amount;
	}

	InventoryObject GetInventoryItem(string iName){
		foreach (InventoryObject i in inventory){
			if (i.itemName == iName){
				return i;
			}
		}

		return null;
	}

	#endregion

	#region equipmentButtons
	public void SabButton () {
		foreach (InventoryObject i in inventory){
			if (i.type == InventoryObject.itemTypes.SAB){
				if (pScript.level > 0 && pScript.level <= 10){
					if (i.stage == 1){
						if (eWeapon != i){
							if (goldObj.itemQuantity > i.cost){
								eWeapon = i;
								AddGold(i.cost * -1);
								UpdateUI();
							} else {
								print ("Not enough Gold");
							}
						} else {
							print ("Weapon Already Equipped");
						}
					} else {
						print ("Stage not initialised");
					}
				} else if (pScript.level > 10 && pScript.level <= 20) {
					if (i.stage == 2){
						if (eWeapon != i){
							if (goldObj.itemQuantity > i.cost){
								eWeapon = i;
								AddGold(i.cost * -1);
								UpdateUI();
							} else {
								print ("Not enough Gold");
							}
						} else {
							print ("Weapon Already Equipped");
						}
					} else {
						print ("Stage not initialised");
					}
				} else {
					print ("Player level out of bounds");
				}
			}
		}
	}

	public void TwoHButton () {
		foreach (InventoryObject i in inventory){
			if (i.type == InventoryObject.itemTypes.TWOH){
				if (pScript.level > 0 && pScript.level <= 10){
					if (i.stage == 1){
						if (eWeapon != i){
							if (goldObj.itemQuantity > i.cost){
								eWeapon = i;
								AddGold(i.cost * -1);
								UpdateUI();
							} else {
								print ("Not enough Gold");
							}
						} else {
							print ("Weapon Already Equipped");
						}
					} else {
						print ("Stage not initialised");
					}
				} else if (pScript.level > 10 && pScript.level <= 20) {
					if (i.stage == 2){
						if (eWeapon != i){
							if (goldObj.itemQuantity > i.cost){
								eWeapon = i;
								AddGold(i.cost * -1);
								UpdateUI();
							} else {
								print ("Not enough Gold");
							}
						} else {
							print ("Weapon Already Equipped");
						}
					} else {
						print ("Stage not initialised");
					}
				} else {
					print ("Player level out of bounds");
				}
			}
		}
	}

	public void DWButton () {
		foreach (InventoryObject i in inventory){
			if (i.type == InventoryObject.itemTypes.DW){
				if (pScript.level > 0 && pScript.level <= 10){
					if (i.stage == 1){
						if (eWeapon != i){
							if (goldObj.itemQuantity > i.cost){
								eWeapon = i;
								AddGold(i.cost * -1);
								UpdateUI();
							} else {
								print ("Not enough Gold");
							}
						} else {
							print ("Weapon Already Equipped");
						}
					} else {
						print ("Stage not initialised");
					}
				} else if (pScript.level > 10 && pScript.level <= 20) {
					if (i.stage == 2){
						if (eWeapon != i){
							if (goldObj.itemQuantity > i.cost){
								eWeapon = i;
								AddGold(i.cost * -1);
								UpdateUI();
							} else {
								print ("Not enough Gold");
							}
						} else {
							print ("Weapon Already Equipped");
						}
					} else {
						print ("Stage not initialised");
					}
				} else {
					print ("Player level out of bounds");
				}
			}
		}
	}

	public void TOneAButton () {
		bool itemBought = false;
		foreach (InventoryObject i in inventory){
			if (!itemBought){
				if (i.type == InventoryObject.itemTypes.AR){
					if (i.stage == (armourStage - 1)){
						if (goldObj.itemQuantity > i.cost){
							if (eArmour != i){
								eArmour = i;
								goldObj.itemQuantity -= i.cost;
								armourStage = i.stage + 1;
								print (eArmour.itemName);
								UpdateUI();
								itemBought = true;
							} else {
								print ("Already Equipped");
							}
						} else {
							print ("Not Enough Gold");
						}
					}
				}
			}

		}
	}

	public void TTwoAButton () {
		bool itemBought = false;
		foreach (InventoryObject i in inventory){

			if (!itemBought){
				if (i.type == InventoryObject.itemTypes.AR){
					if (i.stage == (armourStage)){
						if (goldObj.itemQuantity > i.cost){
							if (eArmour != i){
								eArmour = i;
								goldObj.itemQuantity -= i.cost;
								armourStage = i.stage + 1;
								print (eArmour.itemName);
								UpdateUI();
								itemBought = true;
							} else {
								print ("Already Equipped");
							}
						} else {
							print ("Not enough Gold");
						}
					}
				}
			}
		}
	}

	public void TThreeAButton () {
		bool itemBought = false;
		foreach (InventoryObject i in inventory){

			if (!itemBought){
				if (i.type == InventoryObject.itemTypes.AR){
					if (i.stage == (armourStage + 1)){
						if (goldObj.itemQuantity > i.cost){
							if (eArmour != i){
								eArmour = i;
								goldObj.itemQuantity -= i.cost;
								armourStage = i.stage + 1;
								print (eArmour.itemName);
								UpdateUI();
								itemBought = true;
							} else {
								print ("Already Equipped");
							}
						} else {
							print ("Not enough Gold");
						}
					}
				}
			}
		}
	}

	private void UpdateEquipmentButtonUI () {
		foreach (InventoryObject i in inventory){
			if (i.type == InventoryObject.itemTypes.SAB){
				if (pScript.level > 0 && pScript.level <= 10){
					switch (i.stage)
					{
					case 1:
						sabButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
						break;
					}
				} else if (pScript.level > 10 && pScript.level <= 20){
					switch (i.stage)
					{
					case 2:
						sabButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
						break;
					}
				}
			}

			if (i.type == InventoryObject.itemTypes.TWOH){
				if (pScript.level > 0 && pScript.level <= 10){
					switch (i.stage)
					{
					case 1:
						twoHButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
						break;
					}
				} else if (pScript.level > 10 && pScript.level <= 20){
					switch (i.stage)
					{
					case 2:
						twoHButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
						break;
					}
				}
			}

			if (i.type == InventoryObject.itemTypes.DW){
				if (pScript.level > 0 && pScript.level <= 10){
					switch (i.stage)
					{
					case 1:
						dwButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
						break;
					}
				} else if (pScript.level > 10 && pScript.level <= 20){
					switch (i.stage)
					{
					case 2:
						dwButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
						break;
					}
				}
			}

			if (i.type == InventoryObject.itemTypes.AR){

				if (i.stage == armourStage - 1){
					tOneAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
				}

				if (i.stage == armourStage){
					tTwoAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
				}

				if (i.stage == armourStage + 1){
					tThreeAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = i.itemName;
				}
			}
		}
	}
	#endregion

	#region quest functions

	public void SetQuestActive(string quest){
		foreach (QuestObject q in allQuests){
			if (quest == "QU01"){
				q.questStatus = QuestObject.questStatuses.STARTED;
				print (quest + " started.");
			}
		}

	}

	#endregion

	#region job functions

	private void AddJobEXP(int amount){
		jobCXP += amount;
		if (jobCXP > jobMXP){
			JobLevelUp();
		}
	}

	private void JobLevelUp(){
		jobCXP -= jobMXP;
		jobLevel++;
		jobMXP = Mathf.RoundToInt(jobMXP * 1.5f);
		jobEXPAdd = Mathf.RoundToInt(jobEXPAdd * 1.4f);
	}

	#endregion

	#region minigame functions

	public void StartMinigame(minigames game){

		if (game == activeGame){
			minigameActive = false;
			game = minigames.NONE;
		} else {
			minigameActive = true;
		}

		Camera c = Camera.main;
		switch (game)
		{
		case minigames.WORK:
			activeGame = minigames.WORK;
			c.cullingMask = wLayer;

			wMiniObject.SetActive (true);

			tMiniObject.SetActive (false);
			eMiniObject.SetActive (false);

			activeScheme = controlSchemes.WSCHEME;

			StartWork ();
			break;
		case minigames.TRAIN:
			activeGame = minigames.TRAIN;
			c.cullingMask = tLayer;

			tMiniObject.SetActive (true);

			wMiniObject.SetActive (false);
			eMiniObject.SetActive (false);

			activeScheme = controlSchemes.TSCHEME;


			break;
		case minigames.EXPLORE:
			activeGame = minigames.EXPLORE;
			c.cullingMask = eLayer;

			eMiniObject.SetActive (true);

			wMiniObject.SetActive (false);
			tMiniObject.SetActive (false);

			activeScheme = controlSchemes.ESCHEME;
			break;
		case minigames.NONE:
			minigameActive = false;
			activeGame = minigames.NONE;
			c.cullingMask = mainLayer;

			eMiniObject.SetActive (false);
			tMiniObject.SetActive (false);
			wMiniObject.SetActive (false);
			break;
		}
	}
	#region work
	public void StartWork(){
		mainCam.transform.position += mainCam.transform.forward * 2;
		initBlocker.SetActive (true);
		leftBlocker.SetActive (false);
		rightBlocker.SetActive (false);
		bottomMenuCanvasObject.SetActive (false);

		wInitTimerMax = wInitTimerMaxStart;

		minigameGoldEarned = 0;
		minigameXPEarned = 0;
	}

	public void ResetBall(bool match){

		leftBlocker.SetActive (false);
		rightBlocker.SetActive (false);

		if (!match){
			AddGold ((int)minigameGoldEarned);
			pScript.AddExp ((int)minigameXPEarned);
			EndMinigame ();
			wSphere.GetComponent<WSphereScript> ().gravForce = wSphere.GetComponent<WSphereScript>().initGravForce;
			wInitTimerMax = wInitTimerMaxStart;
		} else {
			wSphere.GetComponent<WSphereScript> ().gravForce += 2;
			minigameGoldEarned += jobLevel;
			minigameXPEarned += pScript.expAdd;
			AddJobEXP (jobEXPAdd);
			print ("Gold: " + minigameGoldEarned + ", XP: " + minigameXPEarned);
		}
	}

	private void WorkUpdate(){
		wInitTimer += Time.deltaTime;
		if (wInitTimer >= wInitTimerMax){
			wInitTimer = 0;
			wInitTimerMax -= 0.05f;
			initBlocker.SetActive (false);
		}
	}


	public void WorkInput(){
		if (Input.GetKeyDown(KeyCode.A)){
			leftBlocker.SetActive (true);
			initBlocker.SetActive (false);
			rightBlocker.SetActive (false);
		}

		if (Input.GetKeyDown(KeyCode.D)){
			rightBlocker.SetActive (true);
			initBlocker.SetActive (false);
			leftBlocker.SetActive (false);
		}

		if (Input.GetKeyDown(KeyCode.S)){
			initBlocker.SetActive (false);
			leftBlocker.SetActive (false);
			rightBlocker.SetActive (false);
		}
	}

	#endregion

	#region train

	private void StartTrain (){
		bottomMenuCanvasObject.SetActive (false);
		ArrowScript a = trainArrow.GetComponent<ArrowScript> ();

		int i = Mathf.RoundToInt(Random.Range (0.0f, 3.0f));
		switch (i)
		{
		case 0:
			a.SetDirection ("up");
			break;
		case 1:
			a.SetDirection ("down");
			break;
		case 2:
			a.SetDirection ("left");
			break;
		case 3:
			a.SetDirection ("right");
			break;
		}

		minigameGoldEarned = 0;
		minigameXPEarned = 0;
		trainTimerMax = trainTimerMaxStart;

		ResetTrainTimer ();
	}

	private void TrainInput(){

		ArrowScript a = trainArrow.GetComponent<ArrowScript> ();
		float b = trainTimerMax * 0.05f;
		if (Input.GetKeyDown(KeyCode.W)){
			CheckTrainInput ("up");
			trainTimerCurrent = 0;
			trainTimerMax -= b;
		}

		if (Input.GetKeyDown(KeyCode.S)){
			CheckTrainInput ("down");
			trainTimerCurrent = 0;
			trainTimerMax -= b;
		}

		if (Input.GetKeyDown(KeyCode.A)){
			CheckTrainInput ("left");
			trainTimerCurrent = 0;
			trainTimerMax -= b;
		}

		if (Input.GetKeyDown(KeyCode.D)){
			CheckTrainInput ("right");
			trainTimerCurrent = 0;
			trainTimerMax -= b;
		}
	}

	private void TrainUpdate(){
		trainTimerCurrent += Time.deltaTime;

		if (trainTimerCurrent >= trainTimerMax){
			trainTimerCurrent = 0;
			TrainFail ();
		}

		timeSlider.value = trainTimerMax - trainTimerCurrent;
//		wInitTimer += Time.deltaTime;
//		if (wInitTimer >= wInitTimerMax){
//			wInitTimer = 0;
//			wInitTimerMax -= 0.05f;
//			initBlocker.SetActive (false);
//		}
	}

	private void CheckTrainInput(string d){
		ArrowScript a = trainArrow.GetComponent<ArrowScript> ();
		switch (d)
		{
		case "up":
			if (a.myDirection == ArrowScript.direction.up){
				TrainSuccess ();
			} else {
				TrainFail ();
			}
			break;
		case "down":
			if (a.myDirection == ArrowScript.direction.down){
				TrainSuccess ();
			} else {
				TrainFail ();
			}
			break;
		case "left":
			if (a.myDirection == ArrowScript.direction.left){
				TrainSuccess ();
			} else {
				TrainFail ();
			}
			break;
		case "right":
			if (a.myDirection == ArrowScript.direction.right){
				TrainSuccess ();
			} else {
				TrainFail ();
			}
			break;
		}
	}

	private void TrainSuccess(){
		minigameXPEarned += pScript.expAdd * 2.5f;
		trainArrow.GetComponent<ArrowScript> ().SetRandomDirection ();
		ResetTrainTimer ();
	}

	private void TrainFail(){
		ResetTrainTimer ();
		trainTimerMax = trainTimerMaxStart;
		AddGold ((int)minigameGoldEarned);
		pScript.AddExp ((int)minigameXPEarned);
		EndMinigame ();
	}

	private void ResetTrainTimer(){
		timeSlider.maxValue = trainTimerMax;
		timeSlider.minValue = 0;
		timeSlider.value = trainTimerMax - trainTimerCurrent;
	}

	#endregion
	public void EndMinigame(){
		mainCam.transform.position = mainCamStartPos;
		bottomMenuCanvasObject.SetActive (true);
		activeScheme = controlSchemes.NONE;
		StartMinigame(minigames.NONE);
		SetActiveWindow(windows.FEEDBACK);
		UpdateFeedbackWindow (actions.WORK, (int)minigameXPEarned, (int)minigameGoldEarned);
		SaveGameVariables ();

	}

	#endregion 

}
