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
    public CameraShake camShake;
    
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

        if (controller.collisions.pushableCollider) {
            Vector3 pushVel = new Vector3(velocity.x / 2.0f, 0.0f, 0.0f);
            controller.collisions.pushableCollider.Move(pushVel * Time.deltaTime, false);
        }

		if (controller.collisions.above || controller.collisions.below) {
            if (velocity.y < -60.0f)
                camShake.Shake(0.15f, 0.05f);
            
			velocity.y = 0;
		}
        
        MoveEyes();
        SquishAndStretch();
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
