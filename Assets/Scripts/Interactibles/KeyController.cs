using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour {

    [Header("Key settings")]
    public DoorController targetDoor;
    
    public void UnlockDoor() {
        targetDoor.Open();
        
        gameObject.SetActive(false);
    }
}
