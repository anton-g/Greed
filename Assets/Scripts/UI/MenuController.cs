using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public GameController gc;
    public GameObject dataManager;

    public Button continueBtn;

    void Awake() {
        if (DataManager.Instance == null)
            Instantiate(dataManager);
    }
    
    void Start() {
        if (DataManager.Instance.reachedLevel > 1) {
            continueBtn.interactable = true;
            continueBtn.Select();
        }
    }

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
