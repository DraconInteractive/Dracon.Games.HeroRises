using UnityEngine;
using System.Collections;

public class playerObjScript : MonoBehaviour {
	public GameObject myTile;
	public int cp;
	Control_Script c;
	ParticleSystem ps;
	public bool canMove;

	void Awake () {
		ps = GetComponent<ParticleSystem> ();
	}

	void Start () {
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		c.epScript = this.gameObject.GetComponent<playerObjScript> ();
		CalculatePlayerCP ();

	}

	public void CalculatePlayerCP(){
		if (c.eWeapon != null && c.eArmour != null){
			cp = c.eWeapon.dps + c.eWeapon.armour + c.eArmour.dps + c.eArmour.armour;
		} else {
			print ("Need to set weapon &&|| armour");
		}
	}

	public void MoveInDirection(string s){
		CalculatePlayerCP ();
		if (!canMove){
			return;
		}
		switch (s)
		{
		case "up":
			RaycastHit hit;
			if (Physics.Raycast(myTile.transform.position, Vector3.up, out hit, 3)){
				if (hit.collider.gameObject.GetComponent<TileScript>()){
					MoveToTile(hit.collider.gameObject);
				}
			}
			break;
		case "down":
			if (Physics.Raycast(myTile.transform.position, Vector3.down, out hit, 3)){
				if (hit.collider.gameObject.GetComponent<TileScript>()){
					MoveToTile(hit.collider.gameObject);
				}
			}
			break;
		case "left":
			if (Physics.Raycast(myTile.transform.position, Vector3.left, out hit, 3)){
				if (hit.collider.gameObject.GetComponent<TileScript>()){
					MoveToTile(hit.collider.gameObject);
				}
			}
			break;
		case "right":
			if (Physics.Raycast(myTile.transform.position, Vector3.right, out hit, 3)){
				if (hit.collider.gameObject.GetComponent<TileScript>()){
					MoveToTile(hit.collider.gameObject);
				}
			}
			break;
		}
	}

	public void MoveToTile(GameObject t){
		if (myTile != null){
			myTile.GetComponent<TileScript> ().isPlayerTile = false;
		}
		t.GetComponent<TileScript> ().isPlayerTile = true;
		transform.position = t.transform.position - t.transform.forward;
		myTile = t;
		TileCombat (t);
	}

	public void TileCombat(GameObject t){
		TileScript ts = t.GetComponent<TileScript> ();
		if (ts.hasEnemy) {
			CalculatePlayerCP ();
			print ("Enemy CP - " + ts.enemyCP + " | Player CP - " + cp + " | Difference: " + (ts.enemyCP - cp).ToString());
			if (ts.enemyCP >= cp){
				LoseCombat ();
				c.TileStory ("Lose");
			} else {
				WinCombat ();
				c.TileStory ("Win");
			}
		} else {
			NoCombat ();
			if (c != null){
				c.TileStory ("None");
			} else {
				c = Control_Script.controlObj.GetComponent<Control_Script> ();
				print ("Retrieved c");
				c.TileStory ("None");
			}

		}

	}

	private bool AreaComplete(){
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tile");
		bool complete = true;
		foreach (GameObject t in tiles){
			if (t.GetComponent<TileScript>().hasEnemy){
				if (t.GetComponent<TileScript>().enemyDefeated){
					
				} else {
					complete = false;
				}
			}
		}
		if (complete == true){
			return true;
		} else {
			return false;
		}
	}

	public void WinCombat(){
		c.PlaySound (Control_Script.audioThing.WIN);
		c.TileReward (myTile);
		myTile.GetComponent<TileScript> ().enemyDefeated = true;
		myTile.GetComponent<TileScript> ().SetColor (Color.green);
		if (myTile.GetComponent<TileScript>().hasGem){
			c.AddGem (1);
			print ("Won Combat - GEM");

			if (AreaComplete()){
				EmitParticles (Color.yellow);
				int areaNum = 0;
				int areaAlreadyComplete = 0;
				foreach (GameObject g in c.areaObjects){
					if (g.activeSelf){
						areaAlreadyComplete = g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().completed;
						g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().completed = 1;
						areaNum = g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().aNum;
						c.SaveGameVariables ();
						break;
					}
				}
				if (areaAlreadyComplete != 1){
					c.CompleteExploreArea (areaNum);
				}

				canMove = false;
				Invoke ("EndExploration", 3);
			} else {
				EmitParticles (Color.cyan);
			}
		} else {
			print ("Won Combat - Normal");
			if (AreaComplete()){
				EmitParticles (Color.yellow);
				int areaNum = 0;
				int areaAlreadyComplete = 0;
				foreach (GameObject g in c.areaObjects){
					if (g.activeSelf){
						areaAlreadyComplete = g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().completed;
						g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().completed = 1;
						areaNum = g.transform.GetChild (0).gameObject.GetComponent<AreaObjectScript> ().aNum;
						c.SaveGameVariables ();
						break;
					}
				}
				if (areaAlreadyComplete != 1){
					c.CompleteExploreArea (areaNum);
				}

				canMove = false;
				Invoke ("EndExploration", 3);
			} else {
				EmitParticles (Color.green);
			}
		}




	}

	public void LoseCombat(){
		c.PlaySound (Control_Script.audioThing.LOSE);
		EmitParticles (Color.red);
		canMove = false;
		Invoke ("EndExploration", 2);
		print ("Lost Combat");
	}

	private void EndExploration(){
		c.EndExplore ();
	}

	public void NoCombat(){
		print ("No Combat");
	}

	private void EmitParticles(Color c){
		if (ps.IsAlive()){
			ps.Clear ();
		}
		ps.startColor = c;
		ps.Emit (50);
	}
}
