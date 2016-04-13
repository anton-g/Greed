﻿using UnityEngine;
using System.Collections;

enum GameState {
	Menu,
    Paused,
	Playing
}

[RequireComponent(typeof(Fader))]
public class GameController : MonoBehaviour {
	[Header("Game settings")]
    public int nonLevelScenes = 1;
    
	[Header("GUI")]
	public Fader fader;
    public Canvas pauseMenu;
    
	[Header("Debugging")]
	public int startLevel = 1;

    int levelCount;
	int currentLevel;
	LevelController levelController;
	bool fading = false;
	GameState state;
	
    void Awake() {
        levelCount = Application.levelCount - nonLevelScenes;
    }
    
	void Start () {
		state = GameState.Menu;
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case GameState.Menu:
            break;
        case GameState.Paused:
            CheckForPause();
			break;
		case GameState.Playing:
            CheckInput();
            CheckForPause();
			CheckForLevelCompletion();
			break;
		}
	}
    
    void TransitionToState(GameState toState) {
        //From
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Paused:
                UnpauseGame();
                break;
            case GameState.Playing:
                break;
        }
        
        //To
        switch (toState)
        {
            case GameState.Menu:
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.Playing:
                break;
        }
        
        state = toState;
    }

	public void StartGame() {
		state = GameState.Playing;
		currentLevel = startLevel;
		Application.LoadLevel(currentLevel);
	}

    void CheckInput() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (!fading) {
				fading = true;
				StartCoroutine("RestartCurrentLevel");
			}
        }
    }
    
    void CheckForPause() {
        if (Input.GetKeyDown(KeyCode.P)) {
            TogglePause();
        }
    }
    
    void UnpauseGame() {
        Time.timeScale = 1.0f;
        pauseMenu.enabled = false;
    }
    
    void PauseGame() {
        Time.timeScale = 0.0f;
        pauseMenu.enabled = true;
    }
    
    void TogglePause() {
        TransitionToState(state == GameState.Paused ? GameState.Playing : GameState.Paused);
    }

	void CheckForLevelCompletion() {
		if (levelController) {
			switch (levelController.state) {
			case LevelState.Playing:
                break;
            case LevelState.Secret:
                Application.LoadLevel(0);
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
			Application.LoadLevel(currentLevel);
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
