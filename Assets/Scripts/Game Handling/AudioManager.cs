using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance = null;

    float _volume;
    public float Volume {
        get { return _volume; }
        set { 
            _volume = Mathf.Clamp(value, 0.0f, 1.0f);
            source.volume = Volume;
        }
    }
    [Range(0.0f, 1.0f)]
    public float intialVolume = 1.0f;

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
		this.Volume = intialVolume;
	}
}
