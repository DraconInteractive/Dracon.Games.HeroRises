using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PresentationScript : MonoBehaviour {
	private float counter;
	// Use this for initialization
	void Start () {
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;

		if (counter >= 5){
			SceneManager.LoadScene ("GameScene01");
		}
	}
}
