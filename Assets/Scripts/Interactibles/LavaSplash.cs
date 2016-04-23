using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class LavaSplash : MonoBehaviour {
	public LayerMask collisionMask;
	public GameObject splashParticles;
	BoxCollider2D col;
	
	Vector2 bottomLeft;
	Vector2 topRight;

	// Use this for initialization
	void Awake () {
		col = GetComponent<BoxCollider2D>();
		
		bottomLeft = new Vector2(col.bounds.center.x - (col.bounds.size.x / 2.0f), col.bounds.center.y - (col.bounds.size.y / 2.0f));
		topRight = new Vector2(col.bounds.center.x + (col.bounds.size.x / 2.0f), col.bounds.center.y + (col.bounds.size.y / 2.0f));
	}
	
	void Update() {
		Collider2D collider = Physics2D.OverlapArea(bottomLeft, topRight, collisionMask);
		if (collider != null) {
			Instantiate(splashParticles, collider.gameObject.transform.position, Quaternion.identity);
		}
	}
}
