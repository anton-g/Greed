using UnityEngine;
using System.Collections;

public class GoalController : RayCastController {

	Renderer rend;
	Material origMat;

	public override void Start () {
		base.Start ();

		rend = GetComponent<Renderer>();
		origMat = rend.material;
	}

	public bool CheckPlayerInGoal() {
		UpdateRaycastOrigins ();

		GameObject hitObject = null;
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth * 2, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * skinWidth * 2, Color.red);
			
			if (hit) {
				hitObject = hit.transform.gameObject;
			}
		}

		//TODO performance issues
		if (hitObject) {
			rend.material = hitObject.GetComponent<Renderer>().material;
			return true;
		} else {
			rend.material = origMat;
			return false;
		}
	}
}
