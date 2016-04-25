using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public GameController gc;
    public GameObject dataManager;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    public Button continueBtn;
    public Button settingsBtn;
    public Button firstSettingsButton;

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
		mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        
        firstSettingsButton.Select();
	}

	public void OnClickQuit() {
        DataManager.Instance.Save();
		Application.Quit();
	}
    
    public void OnClickMuteMusic() {
        AudioManager.Instance.MusicVolume = AudioManager.Instance.MusicVolume == 0.0f ? 1.0f : 0.0f;
        DataManager.Instance.MusicVolume = AudioManager.Instance.MusicVolume;
    }
    
    public void OnClickMuteSound() {
        AudioManager.Instance.Volume = AudioManager.Instance.Volume == 0.0f ? 1.0f : 0.0f;
        DataManager.Instance.SoundVolume = AudioManager.Instance.Volume;
    }
    
    public void OnClickSettingsBack() {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        
        settingsBtn.Select();
    }
}
