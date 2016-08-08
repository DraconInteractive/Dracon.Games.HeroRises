using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AgreementButtonScript : MonoBehaviour {

	public void IAgree(){
		SceneManager.LoadScene ("PresentationScene");
	}
}
