using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance = null;

    public AudioClip playMusic;
    public AudioClip menuMusic;
    public AudioClip pauseMusic;

    [Range(0.0f, 1.0f)]
    public float intialVolume = 1.0f;

    float _volume;
    public float Volume {
        get { return _volume; }
        set { 
            _volume = Mathf.Clamp(value, 0.0f, 1.0f);
            source.volume = Volume;
        }
    }

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
    
    public void PlayMenuMusic() {
        source.Stop();
        source.PlayOneShot(menuMusic);
    }
    
    public void PlayGameMusic() {
        source.Stop();
        source.PlayOneShot(playMusic);
    }
    
    public void PlayPauseMusic() {
        source.Stop();
        source.PlayOneShot(pauseMusic);
    }
}
