using UnityEngine;
using System.Collections;

public class Bounce : Collectible {

	void Update() {
		UpdateRaycastOrigins ();
		
		CollectibleCollisions collisions = Raycast();
		
		//TODO yeah u know
		Collect(collisions.top);
		Collect(collisions.bottom);
		Collect(collisions.left);
		Collect(collisions.right);
	}
	
	void Collect(GameObject g) {
		if (g != null) {
			Player p = g.GetComponent<Player>();
			p.AddBounce(40.0f);
			Destroy(gameObject);
		}
	}
}
