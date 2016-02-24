using UnityEngine;
using System.Collections;

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
				//Only ran on levelloader scene. TODO Should probably find better method.
			}
		}
	}

	IEnumerator LoadNextLevel() {
		//TODO should probably disable input
		if (currentLevel < levelCount) {
			float fadeTime = fader.BeginFade(1);
			yield return new WaitForSeconds(fadeTime);
			currentLevel++;
			Application.LoadLevel("Level_" + currentLevel);
			fading = false;
		}
	}

	IEnumerator RestartCurrentLevel() {
		float fadeTime = fader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevelName);
		fading = false;
	}
}
