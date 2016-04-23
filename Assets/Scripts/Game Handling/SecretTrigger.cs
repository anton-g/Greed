using UnityEngine;
using System.Collections;

public class SecretTrigger : MonoBehaviour {

    public LayerMask collisionLayer;
	
    public AudioClip collectionSound;
    
    [HideInInspector]
    public bool collected;
    
    AudioSource source;
    
    void Awake() {
        source = GetComponent<AudioSource>();
    }
    
	// Update is called once per frame
	void Update () {
	    if (Physics2D.OverlapCircle(transform.position, 0.5f, collisionLayer) && !collected) {
            collected = true;
            
            source.PlayOneShot(collectionSound, AudioManager.Instance.Volume);
            
            Invoke("Deactivate", collectionSound.length);
        }
	}
    
    void Deactivate() {
        gameObject.SetActive(false);
    }
}
