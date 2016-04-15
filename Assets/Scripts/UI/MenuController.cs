using UnityEngine;

public class MenuController : MonoBehaviour {

	public GameController gc;

	public void OnClickContinue() {
		gc.ContinueGame();
	}

	public void OnClickNewGame() {
		gc.StartGame();
	}

	public void OnClickSettings() {
		
	}

	public void OnClickQuit() {
		
	}
}
