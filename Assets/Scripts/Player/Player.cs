using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {
	[Header("Setup")]
	public string JumpButtonName;
	public string HorizontalButtonName;
	public string VerticalButtonName;

	public GameObject positionIndicator;

	[Header("Appearance")]
	public GameObject crushParticle;

	[Header("Movement")]
	public float moveSpeed = 6;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	[Header("Jumping")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;

	[Header("Interaction settings")]
	public float playerBounceForce = 20.0f;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
	
	Controller2D controller;

	GameObject positionHintObject;

	void Start() {
		controller = GetComponent<Controller2D> ();
		
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);

		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
	}
	
	void Update() {
		CheckCrushed();

		if (controller.collisions.death) {
			Invoke("Die", 0.05f);
		}

		if (controller.collisions.playerCollisionBelow) {
			velocity.y = playerBounceForce;
		}

		float moveDir = 0;
		/*if (Input.GetKey(left)) {
			moveDir = -1;
		}
		if (Input.GetKey(right)) {
			moveDir = 1;
		}*/
		moveDir = Input.GetAxisRaw(HorizontalButtonName);

		Vector2 input = new Vector2 (moveDir, Input.GetAxisRaw ("Vertical"));

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		if (Input.GetButtonDown (JumpButtonName)) {
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
			}
		}
		if (Input.GetButtonUp(JumpButtonName)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
	}

	void CheckCrushed() {
		if (controller.collisions.crushed) {
			Instantiate(crushParticle, transform.position, Quaternion.identity);
			Die ();
		}
	}

	void Die() {
		gameObject.SetActive(false);
	}

	#region PositionHint management

	void OnBecameInvisible() {
		if (gameObject.activeSelf) {
			if (!positionHintObject) {
				positionHintObject = Instantiate(positionIndicator, transform.position, Quaternion.identity) as GameObject;
				positionHintObject.GetComponent<HintController>().target = gameObject;
			} else {
				positionHintObject.SetActive(true);
			}
		}
	}

	void OnBecameVisible() {
		if (gameObject.activeSelf && positionHintObject) {
			positionHintObject.SetActive(false);
		}
	}

	#endregion
}
