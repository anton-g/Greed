using UnityEngine;
using System.Collections;

public class BeamController : MonoBehaviour {
	public void Toggle() {
		gameObject.SetActive(!gameObject.activeSelf);
	}
}
