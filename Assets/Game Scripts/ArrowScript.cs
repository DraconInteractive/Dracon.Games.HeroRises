using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	public enum direction {up, down, left, right};
	public direction myDirection;
	public GameObject aUp, aDown, aLeft, aRight;
	//private GameObject childArrow;
	private Vector3 initPos;
	// Use this for initialization
	void Start () {
		//childArrow = transform.GetChild (0).gameObject;
		initPos = transform.position;
		SetRandomDirection ();

	}

	public void SetRandomDirection(){
		float f = Random.Range (0.0f, 3.0f);
		switch ((int)f)
		{
		case 0:
			SetDirection ("up");
			break;
		case 1:
			SetDirection ("down");
			break;
		case 2:
			SetDirection ("left");
			break;
		case 3:
			SetDirection ("right");
			break;
		}
	}

	public void SetDirection (string d){
		switch (d)
		{
		case "up":
			transform.rotation = Quaternion.Euler (-90, 0, 0);
			transform.position = initPos + Vector3.down;
			myDirection = direction.up;
			break;
		case "down":
			transform.rotation = Quaternion.Euler (90, 0, 0);
			transform.position = initPos + Vector3.up;
			myDirection = direction.down;
			break;
		case "left":
			transform.rotation = Quaternion.Euler (0, -90, 90);
			transform.position = initPos + Vector3.right;
			myDirection = direction.left;
			break;
		case "right":
			transform.rotation = Quaternion.Euler (0, 90, 90);
			transform.position = initPos + Vector3.left;
			myDirection = direction.right;
			break;
		}
	}
}
