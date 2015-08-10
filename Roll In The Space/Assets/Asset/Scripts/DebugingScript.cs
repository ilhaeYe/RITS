using UnityEngine;
using System.Collections;

public class DebugingScript : MonoBehaviour {

	public string debugTxt;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{

		debugTxt = GUI.TextArea(new Rect(0, Screen.height - 100, 1000, 100), debugTxt);		 
	}
}
