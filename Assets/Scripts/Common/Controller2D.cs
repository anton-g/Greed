using UnityEngine;

public class Controller2D : RayCastController {
	float maxClimbAngle = 80;
	float maxDescendAngle = 80;
	[HideInInspector]
	public Vector2 playerInput;
	
	public CollisionInfo collisions;

	public override void Start() {
		base.Start ();
		collisions.faceDir = 1;
	}

	public void Move(Vector3 velocity, bool standingOnPlatform) {
		Move (velocity, Vector2.zero, standingOnPlatform);
	}

	public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false) {
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;
		playerInput = input;

		if (velocity.x != 0) {
			collisions.faceDir = (int)Mathf.Sign(velocity.x);
		}

		if (velocity.y < 0) {
			DescendSlope(ref velocity);
		}
		HorizontalCollisions (ref velocity);
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		if (standingOnPlatform) {
			collisions.below = true;
		}

		if (collisions.below) {
			CrushingCollision(velocity);
		}
		
		transform.Translate (velocity);
	}
	
	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		if (Mathf.Abs(velocity.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);
			
			if (hit) {
				if (hit.distance == 0) {
					continue;
				}

				if (hit.collider.tag == "Death") {
					collisions.death = true;
					continue;
				}
                
                if (hit.collider.tag == "Key") {
                    collisions.collidingKey = hit.collider.gameObject;
                    continue;
                }
				
				if (hit.collider.tag == "NoCollision") {
					continue;
				}

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				
				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance-skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}
				
				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;
					
					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}
					
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}
	
	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;
		
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);
			
            if (hit) {
                HandleVerticalHit(hit, directionY, ref velocity);
            }
            
            //If going up, also raycast down but only for player
            if (directionY == 1) {
                Vector2 rayOriginDown = raycastOrigins.bottomLeft + (Vector2.up * velocity.y);
                rayOriginDown += Vector2.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit2D hitDown = Physics2D.Raycast(rayOriginDown, Vector2.up * -1, rayLength, collisionMask);
                
                Debug.DrawRay(rayOriginDown, Vector2.up * -1 * rayLength,Color.red);
                
                if (hitDown) {
                    HandleVerticalHit(hitDown, -1, ref velocity, true);
                }
            }
		}
		
		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,Vector2.right * directionX,rayLength,collisionMask);
			
			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal,Vector2.up);
				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}
    
    void HandleVerticalHit(RaycastHit2D hit, float directionY, ref Vector3 velocity, bool onlyPlayer = false) {
        if (onlyPlayer && (hit.collider.tag == "Player1" || hit.collider.tag == "Player2")) {
            collisions.playerCollisionBelow = true;
            return;
        }
        
        bool shouldCollide = !onlyPlayer;
        
        if (hit.collider.tag == "Through") {
            if (directionY == 1 || collisions.fallingThrough || hit.distance == 0) {
                shouldCollide = false;
            }
            if (playerInput.y == -1) {
                collisions.fallingThrough = true; 
                Invoke("ResetFallingThroughPlatform", .5f);
                shouldCollide = false;
            }
        }
        
        if (hit.collider.tag == "Death") {
            collisions.death = true;
            shouldCollide = false;
        }
        
        if (hit.collider.tag == "Key") {
            collisions.collidingKey = hit.collider.gameObject;
            shouldCollide = false;
        }
        
        if (shouldCollide) {
            if ((hit.collider.tag == "Player1" || hit.collider.tag == "Player2") && directionY == -1) {
                collisions.playerCollisionBelow = true;
            }

            velocity.y = (hit.distance - skinWidth) * directionY;
            //rayLength = hit.distance;
            
            if (collisions.climbingSlope) {
                velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
            }

            collisions.below = (directionY == -1);
            collisions.above = (directionY != -1);
        }
    }

	void CrushingCollision(Vector3 velocity) {
		float rayLength = skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 orig = raycastOrigins.topLeft;
			orig += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D crushCheckHit = Physics2D.Raycast(orig, Vector2.up, rayLength, collisionMask);
			
			Debug.DrawRay(orig, Vector2.up * rayLength, Color.yellow);
			
			if (crushCheckHit && crushCheckHit.collider.gameObject.tag != "Player1" && crushCheckHit.collider.gameObject.tag != "Player2") {
				collisions.crushed = true;
			}
		}
	}
	
	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		
		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}
	
	void DescendSlope(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
		
		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign(hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;
						
						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}
	
	void ResetFallingThroughPlatform() {
		collisions.fallingThrough = false;
	}
	
	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;
		
		public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
		public int faceDir;
		public bool fallingThrough; 
		public bool crushed;
		public bool death;
		public bool playerCollisionBelow;
        public GameObject collidingKey;
		
		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
			crushed = false;
			death = false;
			playerCollisionBelow = false;
			
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
            
            collidingKey = null;
		}
	}
}