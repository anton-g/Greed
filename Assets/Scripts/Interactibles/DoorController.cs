using UnityEngine;
using System.Collections.Generic;

public class DoorController : RayCastController {
	
	public LayerMask passengerMask;
	
	public DIRECTION doorMoveDirection;
	public float length = 10;
	Vector3 direction;

	[Range(0, 10)]
	public float easeAmount;

	[HideInInspector]
	public List<bool> triggers = new List<bool>();
	[Range(1, 5)]
	public int requiredTriggers = 1;

	Vector3 targetPos;
	Vector3 origPos;
	Vector3 velocity;

	List<PassengerMovement> passengerMovement;
	Dictionary<Transform, Controller2D>	passengerDictionary = new Dictionary<Transform, Controller2D>();

	public override void Start () {
		base.Start();

		direction = Helper.Vector3FromDIRECTIONS(doorMoveDirection);

		origPos = transform.position;
		targetPos = origPos + (direction * length);
	}

	public void Open() {
		triggers.Add(true);
	}

	public void Close() {
		if (triggers.Count > 0) {
			triggers.RemoveAt(0);
		}
	}
	
	void Update () {
		UpdateRaycastOrigins ();

		Vector3 velocity = CalculatePlatformMovement();
		
		CalculatePassengerMovement(velocity);
		
		MovePassengers(true);
		transform.Translate (velocity);
		MovePassengers(false);
	}
	
	float Ease(float x) {
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}
	
	Vector3 CalculatePlatformMovement() {
		Vector3 to = origPos;
		if (triggers.Count > 0 && triggers.Count >= requiredTriggers) {
			to = targetPos;
		}

		Vector3 newPosition = Vector3.SmoothDamp(transform.position, to, ref velocity, easeAmount);
		
		return newPosition - transform.position;
	}
	
	void MovePassengers(bool beforeMovePlatform) {
		foreach (PassengerMovement passenger in passengerMovement) {
			if (!passengerDictionary.ContainsKey(passenger.transform)) {
				passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
			}
			if (passenger.moveBeforePlatform == beforeMovePlatform) {
				passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}
	
	void CalculatePassengerMovement(Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovement = new List<PassengerMovement>();
		
		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);
		
		// Vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + skinWidth;
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength * 2.0f, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						
						float pushX = (directionY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
						
						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
					}
				}
			}
		}
		
		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + skinWidth;
			
			for (int i = 0; i < horizontalRayCount; i ++) {
				Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);
				
				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = -skinWidth; //Fix error jumping while pushed
						
						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
					}
				}
			}
		}
		
		// Passenger on top of horizontally or downward moving platform
		if (directionY == -1 || (velocity.y == 0 && velocity.x != 0)) {
			float rayLength = skinWidth * 2;
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, false));
					}
				}
			}
		}
	}
	
	struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;
		
		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		float size = .5f;
	
		Vector3 globalTargetPos = (Application.isPlaying) ? targetPos : targetPos + transform.position;
		Gizmos.DrawLine(globalTargetPos - Vector3.up * size, globalTargetPos + Vector3.up * size);
		Gizmos.DrawLine(globalTargetPos - Vector3.left * size, globalTargetPos + Vector3.left * size);
	}
}