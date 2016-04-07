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
    public Transform leftEye;
    public Transform rightEye;
    public SpriteRenderer graphic;
    
    [Header("Effects")]
    public GameObject crushParticle;
    
	[Header("Movement")]
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

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
    float currentGhostJumpTime = 0.0f;
	
	Controller2D controller;
	GameObject positionHintObject;
    CameraShake camShake;
    Vector3 leftEyeIdle;
    Vector3 rightEyeIdle;
    Vector3 graphicOrigScale;
    float eyeBlinkTime;

    void Awake() {
        leftEyeIdle = leftEye.localPosition;
        rightEyeIdle = rightEye.localPosition;
    }

	void Start() {      
        camShake = Camera.main.GetComponent<CameraShake>();
		controller = GetComponent<Controller2D> ();
        
        graphicOrigScale = graphic.transform.localScale;
        
        eyeBlinkTime = Random.Range(5f, 20f);
        InvokeRepeating("BlinkEyes", eyeBlinkTime, eyeBlinkTime);
        
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
		}
        
        if (controller.collisions.collidingKey != null) {
            controller.collisions.collidingKey.GetComponent<KeyController>().Collect();
        }

		float moveDir = 0;
		moveDir = Input.GetAxisRaw(HorizontalButtonName);

		Vector2 input = new Vector2 (moveDir, Input.GetAxisRaw ("Vertical"));

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        currentGhostJumpTime += Time.deltaTime; //Update ghost jump time
		if (Input.GetButtonDown (JumpButtonName)) {
			if (controller.collisions.below) {
				Jump();
			} else if (CanGhostJump()) {
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
            }

            currentGhostJumpTime = 0;            
            velocity.y = 0;
        }
	}
    
    void FixedUpdate() {
        MoveEyes();
        SquishAndStretch();
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
    
    #region Cosmetics
    
    void MoveEyes() {
        float leftX = leftEyeIdle.x;
        float leftY = leftEyeIdle.y;
        float rightX = rightEyeIdle.x;
        float rightY = rightEyeIdle.y;
        
        //Vertical
        if (velocity.y > 0) {
            //Up
            leftY = 0.18f * (velocity.y / 40) + leftEyeIdle.y;
            rightY = 0.18f * (velocity.y / 40) + rightEyeIdle.y;
        } else if (velocity.y < 0 && !controller.collisions.below) {
            //Down
            leftY = 0.18f * (velocity.y / 40) + leftEyeIdle.y;
            rightY = 0.18f * (velocity.y / 40) + rightEyeIdle.y;
        }
        
        //Horizontal
        if (velocity.x < -1) {
            //Left
            leftX = 0.12f * (velocity.x / moveSpeed) + leftEyeIdle.x;
            rightX = 0.17f * (velocity.x / moveSpeed) + rightEyeIdle.x;
        } else if (velocity.x > 1) {
            //Right
            leftX = 0.17f * (velocity.x / moveSpeed) + leftEyeIdle.x;
            rightX = 0.12f * (velocity.x / moveSpeed) + rightEyeIdle.x;
        }
        
        Vector3 leftPos = new Vector3(leftX, leftY, -0.1f);
        leftEye.localPosition = Vector3.Lerp(leftEye.localPosition, leftPos, 0.1f);
        
        Vector3 rightPos = new Vector3(rightX, rightY, -0.1f);
        rightEye.localPosition = Vector3.Lerp(rightEye.localPosition, rightPos, 0.1f); 
    }
    
    void BlinkEyes() {
        if (gameObject.activeSelf)
            StartCoroutine(Blink());
    }
    
    void SquishAndStretch() {
        //När y velociy är hög, scala om x mindre och y större
        float maxScaleChange = 0.45f;
        
        float scaleChange = (velocity.y / 40) * maxScaleChange;
        
        Vector3 targetScale = new Vector3(graphicOrigScale.x - scaleChange, graphicOrigScale.y, graphicOrigScale.z);
        graphic.transform.localScale = Vector3.Lerp(graphic.transform.localScale, targetScale, 0.05f);
    }
    
    IEnumerator Blink() {
        rightEye.gameObject.SetActive(false);
        leftEye.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(0.1f);
        
        rightEye.gameObject.SetActive(true);
        leftEye.gameObject.SetActive(true);
    }
    #endregion
    
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
