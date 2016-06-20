using UnityEngine;
using System.Collections;
[CreateAssetMenu(fileName="Quest", menuName = "Quest", order = 2)]
public class QuestObject : ScriptableObject {

	public string questName;

	public enum questStatuses {IDLE, STARTED, FINISHED};

	public questStatuses questStatus;

	public bool [] questRequirements;

	public void SaveQuestStatus(){
		switch (questStatus)
		{
		case questStatuses.IDLE:
			PlayerPrefs.SetInt(questName + "Status", 0);
			break;
		case questStatuses.STARTED:
			PlayerPrefs.SetInt(questName + "Status", 1);
			int a = 0;
			foreach (bool b in questRequirements){
				if (b){
					a += 1;
				}
			}
			PlayerPrefs.SetInt(questName + "REQSolved", a);
			break;
		case questStatuses.FINISHED:
			PlayerPrefs.SetInt(questName + "Status", 2);
			break;
		}
	}

	public void LoadQuestStatus(){
		int i = PlayerPrefs.GetInt(questName + "Status");
		int l = 0;
		switch (i)
		{
		case 0:
			questStatus = questStatuses.IDLE;
			foreach (bool b in questRequirements){
				questRequirements[l] = false;
				l++;
			}
			break;
		case 1:
			questStatus = questStatuses.STARTED;
			int p = PlayerPrefs.GetInt(questName + "REQSolved");
			foreach (bool b in questRequirements){
				if (p > 0){
					questRequirements[l] = true;
					p -= 1;
				}
				l++;
			}
			break;
		case 2:
			questStatus = questStatuses.FINISHED;
			break;
		}
	}

	public void ResetQuestStatus(){
		questStatus = questStatuses.IDLE;
		int l = 0;
		foreach (bool b in questRequirements){
			questRequirements[l] = false;
			l++;
		}
	}
}
