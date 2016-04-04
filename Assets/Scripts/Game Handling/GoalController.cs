using UnityEngine;
using System.Collections;

public class GoalController : RayCastController {
	[Header("Appearance")]
	public Material inactiveMaterial;
    public ParticleSystem particles;

	public bool active = true;
	[HideInInspector]
	public bool playerIsInGoal;

	Renderer rend;
    Material origMaterial;
    Color origColor;
    float origEmissionRate;
    Renderer particleRenderer;

	public override void Start () {
		base.Start ();

		rend = GetComponent<Renderer>();
        origMaterial = rend.material;
        origColor = rend.material.color;
        
        origEmissionRate = particles.emissionRate; 
        particles.emissionRate = active ? origEmissionRate : 0.0f;
        particleRenderer = particles.GetComponent<Renderer>();
        
        rend.material = active ? origMaterial : inactiveMaterial;
	}

	void Update() {
		//TODO improve performance. Dont apply material every frame..
		if (active) {
			GameObject hitPlayer = GetPlayerInGoal();

            playerIsInGoal = hitPlayer != null;
			if (playerIsInGoal) {
				rend.material.color = hitPlayer.GetComponent<Player>().graphic.color;
                particleRenderer.material.color = rend.material.color;
			} else {
				rend.material.color = origColor;
                particleRenderer.material.color = origColor;
			}
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
        particles.emissionRate = active ? origEmissionRate : 0.0f;
        rend.material = active ? origMaterial : inactiveMaterial;
	}
}
