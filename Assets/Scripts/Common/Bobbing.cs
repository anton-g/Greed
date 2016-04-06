using UnityEngine;
using System.Collections;

public class Bobbing : MonoBehaviour {

    public float bobDistance = 0.4f;
	
    Vector3 currentTarget;
    Vector3 origPos;
    
    Vector3 velocity;
    float smoothTime = 0.25f;
    float speed = 0.5f;
    
    void Start() {
        origPos = transform.position;
        currentTarget = new Vector3(transform.position.x, transform.position.y + bobDistance, transform.position.z);
    }
    
	void Update () {
	    transform.position = Vector3.SmoothDamp(transform.position, currentTarget, ref velocity, smoothTime, speed);
        
        if (transform.position.y > currentTarget.y - (bobDistance / 10) && transform.position.y < currentTarget.y + (bobDistance / 10)) {
            if (transform.position.y > origPos.y) {
                currentTarget = new Vector3(origPos.x, origPos.y - bobDistance, origPos.z);
            } else {
                currentTarget = new Vector3(origPos.x, origPos.y + bobDistance, origPos.z);
            }
        }
	}
}
