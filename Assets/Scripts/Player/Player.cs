﻿using UnityEngine;

[RequireComponent(typeof(Controller2D), typeof(PlayerCosmetics), typeof(AudioSource))]
public class Player : MonoBehaviour {
	[Header("Setup")]
	public string JumpButtonName;
	public string HorizontalButtonName;
	public string VerticalButtonName;
	public GameObject positionIndicator;

    [Header("Effects")]
    public GameObject crushParticle;
	
	[Header("Sound")]
	public AudioClip bounceSound;
	public AudioClip highFallSound;
    
	[Header("Movement")]
	public bool inputEnabled = true;
	public float moveSpeed = 6;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	[Header("Jumping")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
    public float ghostJumpTime = 0.05f;
    [Range(0.0f, 2.0f)]
    public float gravityModifier = 1.0f;

	[Header("Interaction settings")]
	public float playerBounceForce = 20.0f;
	float bounceVelocity = 0.0f;
	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
    float currentGhostJumpTime = 0.0f;
	
	Controller2D controller;
	GameObject positionHintObject;
    CameraShake camShake;
	AudioSource source;

    [HideInInspector]
    public PlayerCosmetics cosmetics;

	void Start() {
        cosmetics = GetComponent<PlayerCosmetics>();
              
        camShake = Camera.main.GetComponent<CameraShake>();
		controller = GetComponent<Controller2D> ();
        
		source = GetComponent<AudioSource>();
		
        CalculateJumpVariables();
	}
    
    void CalculateJumpVariables() {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        
        gravity *= gravityModifier;
        timeToJumpApex /= gravityModifier;
        
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
			
			if (!source.isPlaying)
				source.PlayOneShot(bounceSound, AudioManager.Instance.Volume);
		}
        
        if (controller.collisions.collidingKey != null) {
            controller.collisions.collidingKey.GetComponent<KeyController>().Collect();
        }
		
		if (bounceVelocity != 0.0f) {
			velocity.y = bounceVelocity;
			bounceVelocity = 0.0f;
		}

		float moveDir = 0;
		moveDir = Input.GetAxisRaw(HorizontalButtonName);

		Vector2 input = new Vector2 (moveDir, Input.GetAxisRaw ("Vertical"));
		
		if (!inputEnabled) input = Vector2.zero;
		
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        currentGhostJumpTime += Time.deltaTime;
		if (Input.GetButtonDown (JumpButtonName) && inputEnabled) {
			if (controller.collisions.below || CanGhostJump()) {
				Jump();
			}
		}
		if (Input.GetButtonUp(JumpButtonName)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input);

		if (controller.collisions.above) {
			velocity.y = 0;
		}
        if (controller.collisions.below) {
            if (velocity.y < -60.0f) {
                camShake.Shake(0.2f, 0.1f);
				source.PlayOneShot(highFallSound, AudioManager.Instance.Volume);
            } else if (velocity.y < -20.0f) {
				
				if (!controller.collisions.playerCollisionBelow)
					cosmetics.Squint();
			}

            currentGhostJumpTime = 0;            
            velocity.y = 0;
        }
	}
    
    void FixedUpdate() {
        cosmetics.Run(velocity, moveSpeed, controller.collisions.below);
    }
    
    bool CanGhostJump() {
        return currentGhostJumpTime < ghostJumpTime && velocity.y < -1.0f;
    }

	void CheckCrushed() {
		if (controller.collisions.crushed) {
			Instantiate(crushParticle, transform.position, Quaternion.identity);
			Die ();
		}
	}

	void Die() {
		gameObject.SetActive(false);
        camShake.Shake(0.4f, 0.3f);
	}
    
    void Jump() {
        velocity.y = maxJumpVelocity;
        currentGhostJumpTime += ghostJumpTime; //Make sure to not allow double jumps
    }
    
    void SetGravityModifier(float newGravityModifer) {
        gravityModifier = newGravityModifer;
        CalculateJumpVariables();
    }
	
	public void AddBounce(float strength) {
		bounceVelocity = strength;
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
