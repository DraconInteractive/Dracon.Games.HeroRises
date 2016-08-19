using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Control_Script : MonoBehaviour {
	
	//Setup
	public static GameObject controlObj;
	public GameObject blurCam;
	public GameObject mainMenu;
	public GameObject bottomMenuCanvasObject;

	//Game
	public bool gameActive;
	public int day;
	public float dayTime;
	public int dayGoldEarned;
	public int dayXPEarned;

	public GameObject adPanel;
	public Button adButton;

	public bool uiActive;

	//Audio
	public GameObject backgroundAudio;
	AudioSource camAS;
	AudioSource bgAS;
	public AudioClip ambientAC, minigameAC, winAC, loseAC, selectAC, workAC;

	public enum audioThing {AMBIENT, MINIGAME, WIN, LOSE, SELECT, WORK};

	//Windows
	public Bottom_Menu_Script bottomMenu;
	public GameObject characterWindow, menuWindow, weaponsWindow, armourWindow, feedbackWindow, mtWindow;
	public GameObject dojoWindow, blacksmithWindow;

	public enum windows {CHARACTER, MENU, WEAPONS, ARMOUR, FEEDBACK, MICROTRANSACTIONS, BLACKSMITH, DOJO, NONE};
	public windows activeWindow;

	public Button sabButton, twoHButton, dwButton;
	public Button tOneAB, tTwoAB, tThreeAB;
	public Button mtButtonMenu;

	//Player
	private GameObject playerObj;
	private Player_Script pScript;

	public InventoryObject eWeapon, eArmour;
	public int armourStage, maxArmourStage;
	public int weaponStage, maxWeaponStage;

	//Actions

	public enum actions {WORK, TRAIN, EXPLORE, ADVERTISEMENT};
	public int jobLevel, jobCXP, jobMXP, jobEXPAdd;

	//Inventory

	public InventoryObject[] inventory;
	public InventoryObject goldObj;
	public InventoryObject gemObj;

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

	private int mRandomCounter;

	//Work
	public GameObject wSphere;
	public GameObject initBlocker, leftBlocker, rightBlocker;
	public Text wHelpText, wProgressText, wGProgressText, wEProgressText;
	private float wInitTimer;
	public float wInitTimerMaxStart, wInitTimerMax;
	private float ballFalling;
	private int wNumRight;

	//Train
	public GameObject trainArrow;
	public float trainTimerCurrent, trainTimerMax, trainTimerMaxStart;
	public Slider timeSlider;

	//Explore
	public playerObjScript epScript;
	public GameObject areaControl;
	public GameObject[] areaObjects;
	public Text areaOneInfoText, areaTwoInfoText;
	public Text areaOneNarrativeText;
	public int activeArea;

	public string[] enemies;
	public string[] winNarrative;
	public string[] loseNarrative;
	public string[] neutralNarrative;

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
		camAS = mainCam.GetComponent<AudioSource> ();
		bgAS = backgroundAudio.GetComponent<AudioSource> ();
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

		Analytics.CustomEvent("GameStarted", new Dictionary<string, object>
			{
				{ "Time", System.DateTime.Now },
			});

		mRandomCounter = 0;

		PlaySound (audioThing.AMBIENT);

		StartCoroutine ("TestIteration");
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameActive){
			return;
		}

		if (Input.GetKeyDown(KeyCode.F5)){
			SaveGameVariables();
		}

		if (Input.GetKeyDown(KeyCode.F7)){
			AddGold (10000);
			UpdateUI ();
		}

		if (Input.GetKeyDown(KeyCode.F8)){
			AddGem (1);
			UpdateUI ();
		}

		if (Input.GetKeyDown(KeyCode.F9)){
			UpdateUI ();
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			if (minigameActive){
				switch (activeGame)
				{
				case minigames.EXPLORE:
					EndMinigame (actions.EXPLORE);
					break;
				case minigames.WORK:
					EndMinigame (actions.WORK);
					break;
				case minigames.TRAIN:
					EndMinigame (actions.TRAIN);
					break;
				case minigames.NONE:
					EndMinigame (actions.WORK);
					break;
				}

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

		if (activeScheme == controlSchemes.ESCHEME){
			ExploreUpdate ();
			ExploreInput();
		}

	}

	public void AddGold (int i) {
		goldObj.itemQuantity += i;
		goldObj.SaveItemDetails ();
	}

	public void AddEXP (int i) {
		pScript.AddExp (i);
	}

	public void AddGem(int amount) {
		gemObj.itemQuantity += amount;
		goldObj.SaveItemDetails ();
	}


	#endregion

	#region Audio Functions

	public void PlaySound(audioThing clip) {
		print (clip.ToString ());
		switch (clip)
		{
		case audioThing.AMBIENT:
			bgAS.Stop ();
			bgAS.clip = ambientAC;
			bgAS.Play ();
			break;
		case audioThing.MINIGAME:
			bgAS.Stop ();
			bgAS.clip = minigameAC;
			bgAS.Play ();
			break;
		case audioThing.LOSE:
			camAS.PlayOneShot (loseAC);
			break;
		case audioThing.WIN:
			camAS.PlayOneShot (winAC);
			break;
		case audioThing.SELECT:
			camAS.PlayOneShot (selectAC);
			break;
		case audioThing.WORK:
			camAS.PlayOneShot (workAC);
			break;
		}
	}

	#endregion

	#region ui functions

	public void SetActiveWindow(windows window){
		if (activeWindow == window){
			if (window == windows.NONE){
				activeWindow = windows.MENU;
			} else {
				activeWindow = windows.NONE;
			}
		} else {
			activeWindow = window;
			Analytics.CustomEvent("Opening Window", new Dictionary<string, object>
				{
					{ "Window", window.ToString() }
				});
		}

		switch (activeWindow)
		{

		case windows.CHARACTER:
			
			characterWindow.SetActive(true);

			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		case windows.MENU:
			
			menuWindow.SetActive(true);

			characterWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		case windows.WEAPONS:
			weaponsWindow.SetActive(true);

			menuWindow.SetActive(false);
			characterWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		case windows.ARMOUR:
			armourWindow.SetActive(true);

			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			mtWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;

		case windows.FEEDBACK:
			feedbackWindow.SetActive(true);

			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			mtWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		case windows.MICROTRANSACTIONS:
			mtWindow.SetActive(true);

			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		case windows.BLACKSMITH:

			blacksmithWindow.SetActive (true);

			mtWindow.SetActive (false);
			characterWindow.SetActive (false);
			menuWindow.SetActive (false);
			weaponsWindow.SetActive (false);
			armourWindow.SetActive (false);
			feedbackWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		case windows.DOJO:

			dojoWindow.SetActive (true);

			blacksmithWindow.SetActive (false);
			mtWindow.SetActive (false);
			characterWindow.SetActive (false);
			menuWindow.SetActive (false);
			weaponsWindow.SetActive (false);
			armourWindow.SetActive (false);
			feedbackWindow.SetActive (false);
			break;
		case windows.NONE:
			mtWindow.SetActive(false);
			characterWindow.SetActive(false);
			menuWindow.SetActive(false);
			weaponsWindow.SetActive(false);
			armourWindow.SetActive(false);
			feedbackWindow.SetActive(false);
			blacksmithWindow.SetActive (false);
			dojoWindow.SetActive (false);
			break;
		}

		if (activeWindow == windows.NONE){
			uiActive = false;
		} else {
			uiActive = true;
		}
	}

	public void MTBUTTON(){
		SetActiveWindow(windows.MICROTRANSACTIONS);
		PlaySound (Control_Script.audioThing.SELECT);

	}

	public void UpdateUI(){
		pScript.CheckLevelStatus ();
		CWinScript c = characterWindow.GetComponent<CWinScript>();
		c.nameText.text = "Name: " + pScript.playerName;
		c.levelText.text = "Level: " + pScript.level.ToString();
		c.expText.text = "EXP: " + pScript.currentExp.ToString() + " / " + pScript.maxExp.ToString();
		c.dayText.text = "Day: " + day.ToString();
		c.jobLevelText.text = "Job Level: " + jobLevel.ToString();
		c.weaponText.text = "Weapon: " + eWeapon.itemName;
		c.armourText.text = "Armour: " + eArmour.itemName;

		switch (eWeapon.type)
		{
		case InventoryObject.itemTypes.SAB:
			c.weaponImage.sprite = c.weaponSprites [0];
			break;
		case InventoryObject.itemTypes.TWOH:
			c.weaponImage.sprite = c.weaponSprites [1];
			break;
		case InventoryObject.itemTypes.DW:
			c.weaponImage.sprite = c.weaponSprites [2];
			break;
		}

//		c.weaponImage.SetNativeSize ();


		bottomMenu.goldText.text = "Gold: " + goldObj.itemQuantity.ToString() + "   |   Gems: " + gemObj.itemQuantity.ToString();

		UpdateEquipmentButtonUI();
	}

	public void ReturnToMainMenu(){
		SetActiveWindow (Control_Script.windows.NONE);
		SaveGameVariables();
		uiActive = true;
		blurCam.SetActive (true);
		mainMenu.SetActive(true);

	}

	public void MainButton(){
		SetActiveWindow(windows.NONE);
		PlaySound (Control_Script.audioThing.SELECT);
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

//		SetActiveWindow (windows.NONE);

		PlayerPrefs.SetString("PlayerName", pScript.playerName);
		PlayerPrefs.SetInt("PlayerLevel", pScript.level);
		PlayerPrefs.SetInt("PlayerCEXP", pScript.currentExp);
		PlayerPrefs.SetInt("PlayerMEXP", pScript.maxExp);
		PlayerPrefs.SetInt("ExpAdd", pScript.expAdd);
		PlayerPrefs.SetInt("EWIndex", eWeapon.itemID);
		PlayerPrefs.SetInt("EAIndex", eArmour.itemID);
		PlayerPrefs.SetInt("ArmourStage", armourStage);
		PlayerPrefs.SetInt ("WeaponStage", weaponStage);

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

		foreach (GameObject g in areaObjects){
			g.transform.GetChild(0).gameObject.GetComponent<AreaObjectScript> ().SaveArea ();
		}

		UpdateUI();

	}

	public void LoadGameVariables(){
		SetActiveWindow (windows.NONE);

		pScript.playerName = PlayerPrefs.GetString("PlayerName");
		pScript.level = PlayerPrefs.GetInt("PlayerLevel");
		pScript.currentExp = PlayerPrefs.GetInt("PlayerCEXP");
		pScript.maxExp = PlayerPrefs.GetInt("PlayerMEXP");
		pScript.expAdd = PlayerPrefs.GetInt("ExpAdd");
		armourStage = PlayerPrefs.GetInt("ArmourStage", 1);
		weaponStage = PlayerPrefs.GetInt ("WeaponStage", 1);

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

		foreach (GameObject g in areaObjects){
			g.transform.GetChild(0).gameObject.GetComponent<AreaObjectScript> ().LoadArea ();
		}

		UpdateUI();

	}

	public void LoadStartingGameVariables(){
		Analytics.CustomEvent("Starting New Game", new Dictionary<string, object>
			{
				{ "Time", System.DateTime.Now }
			});
		
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
		weaponStage = 1;
		maxArmourStage = 5;
		maxWeaponStage = 2;

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

		bottomMenu.GetComponent<Bottom_Menu_Script> ().tutorialPanelOne.SetActive (true);


		foreach (GameObject g in areaObjects){
			g.transform.GetChild(0).gameObject.GetComponent<AreaObjectScript> ().LoadDefaultArea ();
		}

		UpdateUI();
	}
		
	#endregion

	#region action functions

	public void ExecuteAction(actions a){

		switch (a)
		{
		case actions.EXPLORE:
			Analytics.CustomEvent("StartExplore", new Dictionary<string, object>
				{
					{ "startGold", goldObj.itemQuantity },
					{ "startXP", pScript.currentExp },
					{ "activeWeapon", eWeapon.ToString()},
					{ "activeArmour", eArmour.ToString()},
					{ "playerCP", epScript.cp},
					{ "playerLevel", pScript.level}
				});
			StartMinigame (minigames.EXPLORE);

			break;
		case actions.TRAIN:
			Analytics.CustomEvent("StartTrain", new Dictionary<string, object>
				{
					{ "startXP", pScript.currentExp },
					{ "currentLevel", pScript.level }
				});
			StartMinigame (minigames.TRAIN);
			break;
		case actions.WORK:
			Analytics.CustomEvent("StartWork", new Dictionary<string, object>
				{
					{ "startGold", goldObj.itemQuantity },
					{ "startXP", pScript.currentExp },
					{ "activeWeapon", eWeapon.ToString()},
					{ "activeArmour", eArmour.ToString()},
					{ "playerCP", epScript.cp},
					{ "playerLevel", pScript.level}
				});
			StartMinigame (minigames.WORK);
			break;
		}
		UpdateUI();
	}

	public void UpdateFeedbackWindow(actions a, int expAdd, int goldAdd){

		if (a == actions.ADVERTISEMENT){
			feedbackTitleText.text = a.ToString();
			expAddText.text = "Experience Earned: " + expAdd.ToString();
			goldAddText.text = "Gold Earned: " + goldAdd.ToString() + " (+150%)";
			expCurrentText.text = "Current Experience: " + pScript.currentExp.ToString() + " / " + pScript.maxExp.ToString();
			levelCurrentText.text = "Current Level: " + pScript.level.ToString();
		} else {
			feedbackTitleText.text = a.ToString();
			expAddText.text = "Experience Earned: " + expAdd.ToString();
			goldAddText.text = "Gold Earned: " + goldAdd.ToString();
			expCurrentText.text = "Current Experience: " + pScript.currentExp.ToString() + " / " + pScript.maxExp.ToString();
			levelCurrentText.text = "Current Level: " + pScript.level.ToString();
		}

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

		Analytics.CustomEvent("AddingItemToInventory", new Dictionary<string, object>
			{
				{ "ItemAdded", iName },
				{ "playerLevel", pScript.level },
				{ "playerGold", goldObj.itemQuantity}
			});
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
		bool itemBought = false;
		foreach (InventoryObject i in inventory){
			if (!itemBought){
				if (i.type == InventoryObject.itemTypes.SAB && i.stage == weaponStage && eWeapon != i && goldObj.itemQuantity > i.cost){
					PlaySound (Control_Script.audioThing.SELECT);

					eWeapon = i;
					AddGold (i.cost * -1);

					if (weaponStage != maxWeaponStage){
						weaponStage++;
					}
					itemBought = true;

					UpdateUI ();
				}
			}


		}
	}

	public void TwoHButton () {
		bool itemBought = false;
		foreach (InventoryObject i in inventory){
			if (!itemBought){
				if (i.type == InventoryObject.itemTypes.TWOH && i.stage == weaponStage && eWeapon != i && goldObj.itemQuantity > i.cost){
					PlaySound (Control_Script.audioThing.SELECT);

					eWeapon = i;
					AddGold (i.cost * -1);

					if (weaponStage != maxWeaponStage){
						weaponStage++;
					}

					itemBought = true;

					UpdateUI ();
				}

			}

		}
	}

	public void DWButton () {
		bool itemBought = false;
		foreach (InventoryObject i in inventory){
			if (!itemBought){
				if (i.type == InventoryObject.itemTypes.DW && i.stage == weaponStage && eWeapon != i && goldObj.itemQuantity > i.cost){
					PlaySound (Control_Script.audioThing.SELECT);

					eWeapon = i;
					AddGold (i.cost * -1);

					if (weaponStage != maxWeaponStage){
						weaponStage++;
					}

					itemBought = true;

					UpdateUI ();
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
								PlaySound (Control_Script.audioThing.SELECT);

								eArmour = i;
								AddGold (i.cost * -1);
								if (i.stage + 1 <= maxArmourStage){
									armourStage = i.stage + 1;
								}
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
								PlaySound (Control_Script.audioThing.SELECT);

								eArmour = i;
								AddGold (i.cost * -1);
								if (i.stage + 1 != maxArmourStage){
									armourStage = i.stage + 1;
								}
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
								PlaySound (Control_Script.audioThing.SELECT);

								eArmour = i;
								AddGold (i.cost * -1);
								if (i.stage + 1 != maxArmourStage){
									armourStage = i.stage + 1;
								}

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
		InventoryObject aOne = null;
		InventoryObject aTwo = null;
		InventoryObject aThree = null;
		InventoryObject wOne = null;
		InventoryObject wTwo = null;
		InventoryObject wThree = null;

		foreach (InventoryObject i in inventory){
			if (i.type == InventoryObject.itemTypes.SAB) {
				if (i.stage == weaponStage){
					wOne = i;
				}
			} else if (i.type == InventoryObject.itemTypes.TWOH) {
				if (i.stage == weaponStage){
					wTwo = i;
				}
			} else if (i.type == InventoryObject.itemTypes.DW) {
				if (i.stage == weaponStage){
					wThree = i;
				}
			}

			if (i.type == InventoryObject.itemTypes.AR){

				if (i.stage == armourStage - 1){
					aOne = i;
				}

				if (i.stage == armourStage){
					aTwo = i;
				}

				if (i.stage == armourStage + 1){
					aThree = i;
				}


			}
		}

		if (wOne != null){
			sabButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = wOne.itemName;
			if (wOne.cost < goldObj.itemQuantity){
				sabButton.interactable = true;
			} else {
				sabButton.interactable = false;
			}
		} else {
			print ("SAB not available");
			sabButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Unavailable";
			sabButton.interactable = false;
		}

		if (wTwo != null){
			twoHButton.transform.GetChild (0).gameObject.GetComponent<Text> ().text = wTwo.itemName;
			if (wTwo.cost < goldObj.itemQuantity){
				twoHButton.interactable = true;
			} else {
				twoHButton.interactable = false;
			}
		} else {
			print ("2H not available");
			twoHButton.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Unavailable";
			twoHButton.interactable = false;
		}

		if (wThree != null){
			dwButton.transform.GetChild (0).gameObject.GetComponent<Text> ().text = wThree.itemName;
			if (wThree.cost < goldObj.itemQuantity){
				dwButton.interactable = true;
			} else {
				dwButton.interactable = false;
			}
		} else {
			print ("DW not available");
			dwButton.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Unavailable";
			dwButton.interactable = false;
		}

		if (aOne != null){
			if (eArmour == aOne){
				tOneAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = aOne.itemName + " (Equipped)";
				if (aOne.cost < goldObj.itemQuantity){
					tOneAB.interactable = true;
				} else {
					tOneAB.interactable = false;
				}
			}/* else {
				tOneAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = aOne.itemName;
			}*/

		} else {
			print ("aOne not available");
			tOneAB.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Unavailable";
			tOneAB.interactable = false;
		}

		if (aTwo != null){
			tTwoAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = aTwo.itemName;
			if (aTwo.cost < goldObj.itemQuantity){
				tTwoAB.interactable = true;
			} else {
				tTwoAB.interactable = false;
			}
		} else {
			tTwoAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Unavailable";
			print ("aTwo not available");
			tTwoAB.interactable = false;
		}

		if (aThree != null){
			tThreeAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = aThree.itemName;
			if (aThree.cost < goldObj.itemQuantity) {
				tThreeAB.interactable = true;
			} else {
				tThreeAB.interactable = false;
			}
		} else {
			tThreeAB.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Unavailable";
			print ("aThree not available");
			tThreeAB.interactable = false;
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
		jobMXP = Mathf.RoundToInt(jobMXP * 1.1f);
		jobEXPAdd = Mathf.RoundToInt(jobEXPAdd * 1.05f);
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

			StartTrain ();
			break;
		case minigames.EXPLORE:
			activeGame = minigames.EXPLORE;
			c.cullingMask = eLayer;

			eMiniObject.SetActive (true);

			wMiniObject.SetActive (false);
			tMiniObject.SetActive (false);

			activeScheme = controlSchemes.ESCHEME;

			StartExplore ();
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

		SetActiveWindow (windows.NONE);
	}
	#region work
	public void StartWork(){
		PlaySound (audioThing.MINIGAME);
		mainCam.transform.position += mainCam.transform.forward * 2;
		initBlocker.SetActive (true);
		leftBlocker.SetActive (false);
		rightBlocker.SetActive (false);
		bottomMenuCanvasObject.SetActive (false);
		wHelpText.gameObject.SetActive (true);
		wInitTimerMax = wInitTimerMaxStart;

		minigameGoldEarned = 0;
		minigameXPEarned = 0;
		wNumRight = 0;
	}

	public void ResetBall(bool match){

		leftBlocker.SetActive (false);
		rightBlocker.SetActive (false);

		if (!match){
			PlaySound (audioThing.LOSE);
			Analytics.CustomEvent("EndWork", new Dictionary<string, object>
				{
					{ "goldEarned", minigameGoldEarned },
					{ "xpEarned", minigameXPEarned }
				});
			AddGold ((int)minigameGoldEarned);
			pScript.AddExp ((int)minigameXPEarned);
			dayGoldEarned += (int)minigameGoldEarned;
			dayXPEarned += (int)minigameXPEarned;
			EndMinigame (actions.WORK);
			wSphere.GetComponent<WSphereScript> ().gravForce = wSphere.GetComponent<WSphereScript>().initGravForce;
			wInitTimerMax = wInitTimerMaxStart;
		} else {
			PlaySound (audioThing.WORK);
			wHelpText.gameObject.SetActive (false);
			wSphere.GetComponent<WSphereScript> ().gravForce += 10;
			minigameGoldEarned += jobLevel;
			minigameXPEarned += pScript.expAdd;
			AddJobEXP (jobEXPAdd);
			wNumRight++;
			wProgressText.text = wNumRight.ToString ();
			wGProgressText.text = minigameGoldEarned.ToString ();
			wEProgressText.text = minigameXPEarned.ToString ();
//			print ("Gold: " + minigameGoldEarned + ", XP: " + minigameXPEarned);
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
			PlaySound (audioThing.SELECT);
			leftBlocker.SetActive (true);
			initBlocker.SetActive (false);
			rightBlocker.SetActive (false);
		}

		if (Input.GetKeyDown(KeyCode.D)){
			PlaySound (audioThing.SELECT);
			rightBlocker.SetActive (true);
			initBlocker.SetActive (false);
			leftBlocker.SetActive (false);
		}

		if (Input.GetKeyDown(KeyCode.S)){
			PlaySound (audioThing.SELECT);
			initBlocker.SetActive (false);
			leftBlocker.SetActive (false);
			rightBlocker.SetActive (false);
		}
	}

	#endregion

	#region train

	private void StartTrain (){
		bottomMenuCanvasObject.SetActive (false);
//		ArrowScript a = trainArrow.GetComponent<ArrowScript> ();
		PlaySound (audioThing.MINIGAME);
		minigameGoldEarned = 0;
		minigameXPEarned = 0;
		trainTimerMax = trainTimerMaxStart;

		ResetTrainTimer ();
	}

	private void TrainInput(){

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
		PlaySound (audioThing.WORK);
		minigameXPEarned += pScript.expAdd * 2.5f;
		trainArrow.GetComponent<ArrowScript> ().SetRandomDirection ();
		ResetTrainTimer ();
	}

	private void TrainFail(){
		PlaySound (audioThing.LOSE);
		Analytics.CustomEvent("EndTrain", new Dictionary<string, object>
			{
				{ "xpEarned", minigameXPEarned }
			});
		ResetTrainTimer ();
		trainTimerMax = trainTimerMaxStart;
		AddGold ((int)minigameGoldEarned);
		pScript.AddExp ((int)minigameXPEarned);
		dayGoldEarned += (int)minigameGoldEarned;
		dayXPEarned += (int)minigameXPEarned;
		EndMinigame (actions.TRAIN);
	}

	private void ResetTrainTimer(){
		timeSlider.maxValue = trainTimerMax;
		timeSlider.minValue = 0;
		timeSlider.value = trainTimerMax - trainTimerCurrent;
	}

	#endregion

	#region explore

	private void StartExplore() {
		bottomMenuCanvasObject.SetActive (false);
		PlaySound (audioThing.MINIGAME);
		minigameGoldEarned = 0;
		minigameXPEarned = 0;

		areaControl.SetActive (true);

		foreach (GameObject g in areaObjects){
			g.SetActive (false);
		}

		epScript.canMove = true;
		epScript.gameObject.transform.position += Vector3.up * 500;

		if (enemies.Length == 0){
			enemies = new string[5];
			enemies [0] = "Ogre";
			enemies [1] = "Giant";
			enemies [2] = "Troll";
			enemies [3] = "Brett";
			enemies [4] = "MemeLord";
		}

		if (winNarrative.Length == 0){
			winNarrative = new string[5];
			winNarrative [0] = pScript.playerName + " enters a sunlit grove. But there sits an " + enemies[(int)Random.Range(0.0f, 4.0f)] + "! With style, fashion and a pinch the epic, " + pScript.playerName + " fells the fearsome beast.";
			winNarrative [1] = "Striding through dense forest, " + enemies[(int)Random.Range(0.0f, 4.0f)] + "s converge on " + pScript.playerName + ", shrouded in darkness. Suddenly, the sun shines through! With quick strikes the player defeats them, Praise The Sun!";
			winNarrative [2] = "Tripping over their own feet, " + pScript.playerName + " stumbles upon a hidden trove of treasure!";
			winNarrative [3] = "They said that any warrior to face the " + enemies[(int)Random.Range(0.0f, 4.0f)] + " would die in seconds. " + pScript.playerName + " proved them wrong!";
			winNarrative [4] = "Arrows sliced through the air as " + pScript.playerName + " is ambushed by " + enemies[(int)Random.Range(0.0f, 4.0f)] + "s! But using his trusty " + eWeapon.itemName + ", " + pScript.playerName + " fought them back";
		}

		if (loseNarrative.Length == 0){
			loseNarrative = new string[5];
			loseNarrative [0] = "AHHHHH AN OGRE OH KNOW SHIIIIIIIIT HOLY GOD ITS SO BIG HOW THE HELL NO KEEP BACK NO NO NOOOOO AHHHH ... ... ... *gurgle*";
			loseNarrative [1] = "With a single flick of its fingers, the " + enemies[(int)Random.Range(0.0f, 4.0f)] + " sent " + pScript.playerName + " flying through the air. Into a rock. A sharp rock. Ow";
			loseNarrative [2] = "A Justin Bieber has challenged you to battle! Bieber uses 'Sing'!" + pScript.playerName + "dies excruciatingly!";
			loseNarrative [3] = "With great flourish, and pazzaz, " + pScript.playerName + " dies to a group of " + enemies[(int)Random.Range(0.0f, 4.0f)] + "s.";
			loseNarrative [4] = pScript.playerName + " realises suddenly that this is just a game and gives up";
		}

		if (neutralNarrative.Length == 0){
			neutralNarrative = new string[5];
			neutralNarrative [0] = pScript.playerName + " enters a clearing. Nothing happens";
			neutralNarrative [1] = "The life of an adventurer is awesome. Just not right now";
			neutralNarrative [2] = "Congratulations on your movement sire";
			neutralNarrative [3] = "These tiles blend perfectly";
			neutralNarrative [4] = "You made it! ... ... ... ... Now what?";
		}

		SetActiveWindow (windows.NONE);
	}

	public void EndExplore(){
		Analytics.CustomEvent("EndExplore", new Dictionary<string, object>
			{
				{ "goldEarned", minigameGoldEarned },
				{ "xpEarned", minigameXPEarned }
			});
		AddGold ((int)minigameGoldEarned);
		pScript.AddExp ((int)minigameXPEarned);
		dayXPEarned += (int)minigameXPEarned;
		dayGoldEarned = (int)minigameGoldEarned;


		EndMinigame (actions.EXPLORE);
	}

	private void ExploreUpdate(){
		bool complete = false;
		foreach (GameObject g in areaObjects){
			if (g.activeSelf){
				int a = g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().completed;
				if (a == 0){
					complete = false;
				} else {
					complete = true;
				}
			}
		}
		areaOneInfoText.text = "Player Combat Power: " + epScript.cp + "  |  Area Completed Before: " + complete.ToString();
		areaTwoInfoText.text = "Player Combat Power: " + epScript.cp + "  |  Area Completed Before: " + complete.ToString();
	}

	private void ExploreInput(){
		if (Input.GetKeyDown(KeyCode.W)){
			epScript.MoveInDirection ("up");
		}

		if (Input.GetKeyDown(KeyCode.S)){
			epScript.MoveInDirection ("down");
		}

		if (Input.GetKeyDown(KeyCode.A)){
			epScript.MoveInDirection ("left");
		}

		if (Input.GetKeyDown(KeyCode.D)){
			epScript.MoveInDirection ("right");
		}
	}

	public void TileReward(GameObject t){
		minigameGoldEarned += t.GetComponent<TileScript> ().enemyCP / 2;
		minigameXPEarned += t.GetComponent<TileScript> ().enemyCP;
	}

	public void TileStory(string s){
		switch (s)
		{
		case "Win":
			switch (activeArea)
			{
			case 1:
				areaOneNarrativeText.text = winNarrative [(int)Random.Range (0.0f, 4.0f)];
				break;
			case 2:
				break;
			case 3:
				break;
			}
			break;
		case "Lose":
			switch (activeArea)
			{
			case 1:
				areaOneNarrativeText.text = loseNarrative [(int)Random.Range (0.0f, 4.0f)];
				break;
			case 2:
				break;
			case 3:
				break;
			}
			break;
		case "None":
			switch (activeArea)
			{
			case 1:
				areaOneNarrativeText.text = neutralNarrative [(int)Random.Range (0.0f, 4.0f)];
				break;
			case 2:
				break;
			case 3:
				break;
			}
			break;
		}
	}

	public void CompleteExploreArea(int areaNum) {
		switch (areaNum)
		{
		case 0:
			print ("Invalid Area Completion Number (CompleteExploreArea on c, called from playerObjScript");
			break;
		case 1:
			minigameGoldEarned += 500;
			minigameXPEarned += 250;
			break;
		case 2:
			minigameGoldEarned += 1500;
			minigameXPEarned += 750;
			break;
		case 3:
			minigameGoldEarned += 5000;
			minigameXPEarned += 1500;
			break;
		}
	}


	#endregion
	public void EndMinigame(actions a){
		print ("Ended Minigame");
		float r = Random.Range (0.0f, 100.0f);
		print ("Ad Random Result - " + r + "  |  Random Counter Result - " + mRandomCounter);
		switch (mRandomCounter)
		{
		case 0:
			if (r < 25){
				adButton.gameObject.SetActive (true);
				mRandomCounter = 0;
			} else {
				mRandomCounter++;
				adButton.gameObject.SetActive (false);
			}
			break;
		case 1:
			if (r < 50){
				adButton.gameObject.SetActive (true);
				mRandomCounter = 0;
			} else {
				adButton.gameObject.SetActive (false);
				mRandomCounter++;
			}
			break;
		case 2:
			if (r < 75){
				adButton.gameObject.SetActive (true);
				mRandomCounter = 0;
			} else {
				adButton.gameObject.SetActive (false);
				mRandomCounter++;
			}
			break;
		case 3:
			adButton.gameObject.SetActive (true);
			mRandomCounter = 0;
			break;
		}


		PlaySound (audioThing.AMBIENT);
		mainCam.transform.position = mainCamStartPos;
		bottomMenuCanvasObject.SetActive (true);
		activeScheme = controlSchemes.NONE;
		StartMinigame(minigames.NONE);
		UpdateFeedbackWindow (a, (int)minigameXPEarned, (int)minigameGoldEarned);
		SetActiveWindow(windows.FEEDBACK);

		SaveGameVariables ();

	}

	#endregion 

	#region advertisements
	public void StartAdvertisement(){
		Analytics.CustomEvent("StartAdvertisement", new Dictionary<string, object>
			{
				{ "Time", System.DateTime.Now }
			});
		PlaySound (audioThing.SELECT);
		adPanel.SetActive (true);
		adButton.gameObject.SetActive (false);
		AddGold ((int)(minigameGoldEarned * 0.5f));
		dayGoldEarned += (int)(minigameGoldEarned * 0.5f);
		minigameGoldEarned *= 1.5f;
		Invoke ("EndAdvertisement", 5);
	}

	public void EndAdvertisement(){
		adPanel.SetActive (false);
		UpdateFeedbackWindow (actions.ADVERTISEMENT, (int)minigameXPEarned, (int)minigameGoldEarned);
	}

	#endregion

	#region microtransactions

	public float GetGoldTierRation(int tier){
		int i = 0;
		switch (tier)
		{
		case 1:
			i = pScript.level * 10;

			break;
		case 2:
			i = pScript.level * 25;
			break;
		case 3:
			i = pScript.level * 75;
			break;
		case 4:
			i = pScript.level * 300;
			break;
		default:
			print ("Inactive tier input");
			i = 0;
			break;
		}
		return i;
	}

	public float GetGemTierRation(int tier){
		return 0;
	}

	public void BuyGold(int tier){
		Analytics.CustomEvent("BuyGold", new Dictionary<string, object>
			{
				{ "tier", tier }
			});
		PlaySound (audioThing.SELECT);
		switch (tier)
			{
			case 1:
				if (gemObj.itemQuantity > 5){
					AddGem (-5);
					AddGold (pScript.level * 10);
					UpdateUI ();
					print ("added: " + pScript.level * 10 + " Gold");
				}
				break;
			case 2:
				if (gemObj.itemQuantity > 10){
					AddGem (-10);
					AddGold (pScript.level * 25);
					UpdateUI ();
					print ("added: " + pScript.level * 25 + " Gold");
				}
				break;
			case 3:
				if (gemObj.itemQuantity > 25){
					AddGem (-25);
					AddGold (pScript.level * 75);
					UpdateUI ();
					print ("added: " + pScript.level * 75 + " Gold");
				}
				break;
			case 4:
				if (gemObj.itemQuantity > 75){
					AddGem (-75);
					AddGold (pScript.level * 300);
					UpdateUI ();
					print ("added: " + pScript.level * 300 + " Gold");
				}
				break;
			default:
				print ("Inactive tier input");
				break;
			}

	}

	public void BuyGems(int tier){
		Analytics.CustomEvent("BuyGem", new Dictionary<string, object>
			{
				{ "tier", tier }
			});
		PlaySound (audioThing.SELECT);
		switch (tier)
		{
		case 1:
			Analytics.Transaction ("GemTOne", (decimal)4.95, "AUD");
			break;
		case 2:
			Analytics.Transaction ("GemTTwo", (decimal)9.95f, "AUD");
			break;
		case 3:
			Analytics.Transaction ("GemTThree", (decimal)24.95f, "AUD");
			break;
		case 4:
			Analytics.Transaction ("GemTFour", (decimal)49.95f, "AUD");
			break;
		}
	}






	#endregion

	public IEnumerator TestIteration () {
		yield return new WaitForSeconds (2);
		while (true){
			if (EventSystem.current.currentSelectedGameObject != null){
				print (EventSystem.current.currentSelectedGameObject.name);
			}

			yield return new WaitForSeconds (0.5f);
		}
	}

}
