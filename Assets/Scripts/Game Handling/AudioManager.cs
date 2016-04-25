using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance = null;

    public AudioClip playMusic;
    public AudioClip menuMusic;
    public AudioClip pauseMusic;

    [HideInInspector]
    public float Volume;
    
    float _musicVolume;
    public float MusicVolume {
        get { return _musicVolume; }
        set { 
            _musicVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            source.volume = _musicVolume;
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
        this.Volume = DataManager.Instance.SoundVolume;
		this.MusicVolume = DataManager.Instance.MusicVolume;
	}
    
    public void PlayMenuMusic() {
        source.Stop();
        source.clip = menuMusic;
        source.Play();
    }
    
    public void PlayGameMusic() {
        source.Stop();
        source.clip = playMusic;
        source.Play();
    }
    
    public void PlayPauseMusic() {
        source.Stop();
        source.clip = pauseMusic;
        source.Play();
    }
}
