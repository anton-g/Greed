using UnityEngine;
using System.Collections;

public class Bounce : Collectible {

	void Update() {
		Collect(SphereCast());
	}
	
	void Collect(GameObject g) {
		if (g != null) {
			Player p = g.GetComponent<Player>();
			p.AddBounce(40.0f);
			Destroy(gameObject);
		}
	}
}
