using UnityEngine;

[RequireComponent(typeof(Bobbing))]
public class KeyController : MonoBehaviour {

    [Header("Key settings")]
    public DoorController targetDoor;
    
    [Header("Appearance")]
    public GameObject graphic;
    public ParticleSystem particle;
    
    public virtual void Collect() {
        targetDoor.Open();
        
        graphic.SetActive(false);
        particle.gameObject.SetActive(true);
        
        Invoke("DisableParticles", 1.0f);
    }
    
    private void DisableParticles() {
        particle.gameObject.SetActive(false);
    }
}
