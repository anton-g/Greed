using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickContinue() {
		Application.LoadLevel("Level_0");
	}

	public void OnClickNewGame() {
		Debug.Log("NewGame");
	}

	public void OnClickSettings() {
		Debug.Log("Settings");
	}

	public void OnClickQuit() {
		Debug.Log("Quit");
	}
}
