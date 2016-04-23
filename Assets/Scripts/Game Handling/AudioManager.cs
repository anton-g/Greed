using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance = null;

	AudioSource source;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }    
        DontDestroyOnLoad(gameObject);
		
		source = GetComponent<AudioSource>();
    }
	
	void Start() {
		
	}
}
