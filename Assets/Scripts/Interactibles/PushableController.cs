using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PushableController : MonoBehaviour {

    Controller2D controller;

    float gravity = -114.2857f;
    Vector3 velocity;
    
	// Use this for initialization
	void Start () {
	   controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
	   velocity.y += gravity * Time.deltaTime;
	   controller.Move (velocity * Time.deltaTime, false);
       
       if (controller.collisions.above || controller.collisions.below) {
	       velocity.y = 0;
	   }
	}
}
