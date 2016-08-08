using UnityEngine;
using System.Collections;

public class BlackSmith_Camera_Script : MonoBehaviour {

	public RenderTexture rt;

	void Start () {
		StartCoroutine ("ReleaseRT");
	}

	public IEnumerator ReleaseRT (){
		while (true){
			rt.DiscardContents ();
			yield return new WaitForSeconds (1);
		}
	}
}
