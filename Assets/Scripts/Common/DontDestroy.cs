using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {
	// Use this for initialization
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
