using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameController gc;

	public void OnClickContinue() {
		gc.StartGame();
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
