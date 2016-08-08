using UnityEngine;
using System.Collections;

public class WSphereScript : MonoBehaviour {
	private Control_Script c;
	private Vector3 startPos;
	public enum ColorType {Y, G, B};
	public string myColor;
	public Vector3 grav;
	public float gravForce, initGravForce;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		c = Control_Script.controlObj.GetComponent<Control_Script> ();
		rb = GetComponent<Rigidbody> ();
		startPos = transform.position;
		float colR = Random.Range (0.0f, 3.0f);
		switch ((int)colR)
		{
		case 0:
			myColor = "Y";
			GetComponent<MeshRenderer> ().material.color = new Color (1.0f,0.5f,1.0f);
			break;
		case 1:
			myColor = "G";
			GetComponent<MeshRenderer> ().material.color = Color.green;
			break;
		case 2:
			myColor = "B";
			GetComponent<MeshRenderer> ().material.color = Color.blue;
			break;
		}

		gravForce = initGravForce;
		SetGrav ();

	}

	void Update () {
		rb.AddForce (grav);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "colorCube"){
			bool match = false;
			if (col.gameObject.GetComponent<colorCubeScript>().myColor == myColor){
				match = true;
			}
			ResetSelf (match);
		}
	}

	private void ResetSelf(bool match){
		transform.position = startPos;
		c.ResetBall (match);
		float colR = Random.Range (0.0f, 3.0f);
		switch ((int)colR)
		{
		case 0:
			myColor = "Y";
			GetComponent<MeshRenderer> ().material.color = new Color (1.0f,0.5f,1.0f);
			break;
		case 1:
			myColor = "G";
			GetComponent<MeshRenderer> ().material.color = Color.green;
			break;
		case 2:
			myColor = "B";
			GetComponent<MeshRenderer> ().material.color = Color.blue;
			break;
		}
		SetGrav ();
	}

	private void SetGrav(){
		grav = Vector3.down * gravForce;
	}
}
