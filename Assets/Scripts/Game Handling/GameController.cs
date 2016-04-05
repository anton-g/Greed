using UnityEngine;
using System.Collections;

enum GameState {
	GameMenu,
	GamePlaying
}

[RequireComponent(typeof(Fader))]
public class GameController : MonoBehaviour {
	[Header("Game settings")]
	public int levelCount = 3;

	[Header("GUI")]
	public Fader fader;

	[Header("Debugging")]
	public int startLevel = 1;

	int currentLevel;
	LevelController levelController;
	bool fading = false;
	GameState state;
	
	void Start () {
		DontDestroyOnLoad(gameObject);

		state = GameState.GameMenu;
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case GameState.GameMenu:
			break;
		case GameState.GamePlaying:
            CheckInput();
			CheckForLevelCompletion();
			break;
		}
	}

	public void StartGame() {
		state = GameState.GamePlaying;
		currentLevel = startLevel;
		Application.LoadLevel("Level_" + currentLevel);
	}

    void CheckInput() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (!fading) {
				fading = true;
				StartCoroutine("RestartCurrentLevel");
			}
        }
        
        if (Input.GetKeyDown(KeyCode.P)) {
            Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
        }
    }

	void CheckForLevelCompletion() {
		if (levelController) {
			switch (levelController.state) {
			case LevelState.Playing:
				break;
			case LevelState.Completed:
				if (!fading) {
					fading = true;
					StartCoroutine("LoadNextLevel");
				}
				break;
			case LevelState.Failed:
				if (!fading) {
					fading = true;
					StartCoroutine("RestartCurrentLevel");
				}
				break;
			}
		} else {
			try {
				levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
			} catch (System.NullReferenceException ex) {
				Debug.Log(ex.Message);
				//Only runs on levelloader scene. TODO Should probably find better method.
			}
		}
	}

	IEnumerator LoadNextLevel() {
		//TODO should probably disable input
		if (currentLevel < levelCount) {
			float fadeTime = fader.BeginFade(1, false);
			yield return new WaitForSeconds(fadeTime);
			currentLevel++;
			Application.LoadLevel("Level_" + currentLevel);
			fading = false;
		}
	}

	IEnumerator RestartCurrentLevel() {
		float fadeTime = fader.BeginFade(1, true);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevelName);
		fading = false;
	}
}
