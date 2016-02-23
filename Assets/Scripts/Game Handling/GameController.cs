using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int levelCount = 3;
	public int startLevel = 1;
	int currentLevel;

	LevelController levelController;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);

		currentLevel = startLevel;
		Application.LoadLevel("Level_" + currentLevel);
	}
	
	// Update is called once per frame
	void Update () {
		if (levelController) {
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
		} else {
			try {
				levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
			} catch (System.NullReferenceException ex) {
				//Only ran on levelloader scene. TODO Should probably find better method.
			}
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
