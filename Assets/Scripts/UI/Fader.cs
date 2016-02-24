using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {
	[Header("Setup")]
	public Texture2D levelFadeTexture;
	public Texture2D deathFadeTexture;
	public float fadeSpeed = 0.0f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	private Texture2D selectedTexture;

	void Start() {
		selectedTexture = levelFadeTexture;
	}

	void OnGUI() {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), selectedTexture);
	}

	public float BeginFade(int direction, bool death) {
		selectedTexture = death ? deathFadeTexture : levelFadeTexture;
		fadeDir = direction;
		return fadeSpeed;
	}

	void OnLevelWasLoaded() {
		BeginFade(-1, false);
	}
}
