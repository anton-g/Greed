using UnityEngine;
using System.Collections;

public class BreakableController : RayCastController {
	[Header("Setup")]
	public GameObject obstacle;
	public ParticleSystem particles;

	[Header("Behaviour")]
	public float speed = 1.0f;


	private bool triggered = false;
	private Vector3 origPos;
	private float origScale;

	public override void Start () {
		base.Start ();

		origPos = obstacle.transform.position;
		origScale = obstacle.transform.localScale.y;
	}

	void Update () {
		UpdateRaycastOrigins ();

		if (!triggered) {
			triggered = Collisions();
			if (triggered) {
				particles.emissionRate = 50.0f * transform.localScale.x;
			}
		} else {
			float newY = Mathf.MoveTowards(obstacle.transform.localScale.y, 0.0f, Time.deltaTime * speed);
			obstacle.transform.localScale -= new Vector3(0f, obstacle.transform.localScale.y - newY, 0.0f);

			float yScale = obstacle.transform.localScale.y;
			obstacle.transform.position = origPos - obstacle.transform.up * (yScale / 2.0f - origScale / 2.0f);

			particles.transform.position = origPos + new Vector3(0.0f, -(yScale - origScale / 2.0f), 0.0f);

			if (obstacle.transform.localScale.y <= 0.0f) {
				obstacle.SetActive(false);
				particles.emissionRate = 0.0f;
				GetComponent<BoxCollider2D>().enabled = false;
			}
		}
	}

	bool Collisions() {
		float rayLength = skinWidth * 2;
		
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.up, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
			
			if (hit) {
				return true;
			}
		}
		return false;
	}
}
