using UnityEngine;

public class CameraFollowOld : MonoBehaviour {
	public Controller2D target;
	public float verticalOffset;
	public float lookAheadDistanceX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirectionX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	bool lookAheadStopped;

	void Start() {
		focusArea = new FocusArea(target.col.bounds, focusAreaSize);
	}

	void LateUpdate() {
		focusArea.Update(target.col.bounds);
		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

		if (focusArea.velocity.x != 0) {
			lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
			if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
				targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
				lookAheadStopped = false;
			}
			else {
				if (!lookAheadStopped) {
					targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX)/4f;
					lookAheadStopped = true;
				}

			}
		}
		currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX); 

		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;
		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1,0,0,0.5f);
		Gizmos.DrawCube(focusArea.center, focusAreaSize);
	}

	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;

		float left, right;
		float top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			center = new Vector2((left+right)/2, (top+bottom)/2);

			velocity = Vector2.zero;
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			}
			else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			}
			else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			bottom += shiftY;
			top += shiftY;

			center = new Vector2((left+right)/2, (top+bottom)/2);

			velocity = new Vector2(shiftX, shiftY);
 		}
	}
}
