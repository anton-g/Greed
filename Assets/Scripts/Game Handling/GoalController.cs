using UnityEngine;
using System.Collections;

public class GoalController : RayCastController {
	[Header("Appearance")]
	public Material inactiveMaterial;

	public bool active = true;
	[HideInInspector]
	public bool playerIsInGoal;

	Renderer rend;
	Material origMat;

	public override void Start () {
		base.Start ();

		rend = GetComponent<Renderer>();
		origMat = rend.material;
	}

	void Update() {
		//TODO improve performance. Dont apply material every frame..
		if (active) {
			GameObject hitPlayer = GetPlayerInGoal();

			if (hitPlayer != null) {
				playerIsInGoal = true;

				rend.material = hitPlayer.GetComponent<Renderer>().material;
			} else {
				playerIsInGoal = false;

				rend.material = origMat;
			}
		} else {
			rend.material = inactiveMaterial;
		}
	}

	GameObject GetPlayerInGoal() {
		UpdateRaycastOrigins ();

		GameObject hitObject = null;
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth * 2, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * skinWidth * 2, Color.red);
			
			if (hit) {
				hitObject = hit.collider.gameObject;
			}
		}

		return hitObject;
	}

	public void Toggle() {
		this.active = !this.active;
	}
}
