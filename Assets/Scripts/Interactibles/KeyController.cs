using UnityEngine;

[RequireComponent(typeof(Bobbing))]
public class KeyController : MonoBehaviour {

    [Header("Key settings")]
    public DoorController targetDoor;
    
    public void Collect() {
        targetDoor.Open();
        
        gameObject.SetActive(false);
    }
}
