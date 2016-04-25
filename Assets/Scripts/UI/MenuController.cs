using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public GameController gc;
    public GameObject dataManager;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    public Button continueBtn;
    public Button settingsBtn;
    public Button musicBtn;
    public Button soundBtn;

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
        
        musicBtn.Select();
	}

	public void OnClickQuit() {
        DataManager.Instance.Save();
		Application.Quit();
	}
    
    public void OnClickMuteMusic() {
        if (AudioManager.Instance.MusicVolume == 0.0f) {
            AudioManager.Instance.MusicVolume = 1.0f;
            musicBtn.GetComponent<Text>().text = "MUTE MUSIC";
        } else {
            AudioManager.Instance.MusicVolume = 0.0f;
            musicBtn.GetComponent<Text>().text = "UNMUTE MUSIC";
        }
        DataManager.Instance.MusicVolume = AudioManager.Instance.MusicVolume;
    }
    
    public void OnClickMuteSound() {
        if (AudioManager.Instance.Volume == 0.0f) {
            AudioManager.Instance.Volume = 1.0f;
            soundBtn.GetComponent<Text>().text = "MUTE SOUND";
        } else {
            AudioManager.Instance.Volume = 0.0f;
            soundBtn.GetComponent<Text>().text = "UNMUTE SOUND";
        }
        DataManager.Instance.SoundVolume = AudioManager.Instance.Volume;
    }
    
    public void OnClickSettingsBack() {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        
        settingsBtn.Select();
    }
}
