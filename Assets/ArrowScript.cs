using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	public enum direction {up, down, left, right};
	public direction myDirection;
	public GameObject aUp, aDown, aLeft, aRight;
	private GameObject childArrow;
	// Use this for initialization
	void Start () {
		childArrow = transform.GetChild (0).gameObject;
	}

	public void SetDirection (string d){
		switch (d)
		{
		case "up":
			transform.LookAt (aUp.transform.position);
			childArrow.transform.localRotation = Quaternion.Euler (0, 0, 0);
			break;
		case "down":
			transform.LookAt (aDown.transform.position);
			childArrow.transform.localRotation = Quaternion.Euler (0, 0, 0);
			break;
		case "left":
			transform.LookAt (aLeft.transform.position);
			childArrow.transform.localRotation = Quaternion.Euler (0, 0, 90);
			break;
		case "right":
			transform.LookAt (aRight.transform.position);
			childArrow.transform.localRotation = Quaternion.Euler (0, 0, 90);
			break;
		}
	}
}
