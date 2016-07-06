using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Control_Script))]
public class ControlScriptEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		Control_Script c = (Control_Script)target;

		if (GUILayout.Button("Go to Work")){
			c.StartMinigame (Control_Script.minigames.WORK);
		}
		if (GUILayout.Button("Go Explore")){
			c.StartMinigame (Control_Script.minigames.EXPLORE);
		}
		if (GUILayout.Button("Go Train")){
			c.StartMinigame (Control_Script.minigames.TRAIN);
		}
	}






}
