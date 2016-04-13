using UnityEngine;
using System.Collections;

public class SecretTrigger : MonoBehaviour {

    public LayerMask collisionLayer;
	
    [HideInInspector]
    public bool collected;
    
	// Update is called once per frame
	void Update () {
	    if (Physics2D.OverlapCircle(transform.position, 0.5f, collisionLayer)) {
            collected = true;
            gameObject.SetActive(false);
        }
	}
}
