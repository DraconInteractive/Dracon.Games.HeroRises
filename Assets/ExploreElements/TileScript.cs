using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
	public bool isStartTile, isPlayerTile, hasEnemy, hasGem, enemyDefeated;
	public GameObject playerObject, areaObject;
	private playerObjScript pScript;

	//if has enemy

	public float enemyCP;
	// Use this for initialization
	void Awake () {
		pScript = playerObject.GetComponent<playerObjScript> ();
	}
	void Start () {
		if (isStartTile) {
			hasEnemy = false;
			pScript.MoveToTile (this.gameObject);
		} else {
			if (Random.Range (0.0f, 100.0f) < 25){
				hasEnemy = true;
			} else {
				hasEnemy = false;
			}
		}

		if (enemyDefeated){
			SetColor (Color.green);
			hasEnemy = false;
		} 

		if (hasEnemy){
			SetColor (Color.red);

			if (Random.Range(0.0f, 100.0f) < 10){
				hasGem = true;
			} else {
				hasGem = false;
			}
		} 

		if (!hasEnemy && !enemyDefeated){
			SetColor (Color.yellow);
		}


	}

	void OnEnable () {
		if (isStartTile) {
			hasEnemy = false;
			pScript.MoveToTile (this.gameObject);
		} else {
			if (Random.Range (0.0f, 100.0f) < 25){
				hasEnemy = true;
			} else {
				hasEnemy = false;
			}
		}

		if (enemyDefeated){
			hasEnemy = false;
			SetColor (Color.green);
		} 

		if (hasEnemy) {

			SetColor (Color.red);

			if (Random.Range(0.0f, 100.0f) < 10){
				hasGem = true;
			} else {
				hasGem = false;
			}
		} 
		if (!hasEnemy && !enemyDefeated){
			SetColor (Color.yellow);
		}
	}

	public void SetColor(Color c){
		GetComponent<MeshRenderer> ().material.color = c;
	}
}
