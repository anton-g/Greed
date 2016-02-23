using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int levelCount = 3;
	int currentLevel;

	LevelController levelController;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);

		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
		currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (levelController == null) {
			levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
		}

		switch (levelController.state) {
		case LevelState.Playing:
			break;
		case LevelState.Completed:
			LoadNextLevel();
			break;
		case LevelState.Failed:
			RestartCurrentLevel();
			break;
		}
	}

	void LoadNextLevel() {
		if (currentLevel < levelCount) {
			currentLevel++;
			Application.LoadLevel("Level_" + currentLevel);
		}
	}

	void RestartCurrentLevel() {
		Application.LoadLevel(Application.loadedLevelName);
	}
}
