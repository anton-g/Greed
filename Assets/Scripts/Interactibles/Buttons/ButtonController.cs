using UnityEngine;
using System.Collections;

public enum ButtonState {
	Waiting,
	Pushed,
	Used
}

/// <summary>
/// Inherit from this class to create a button that controlls a specific component
/// </summary>
public abstract class ButtonController : RayCastController {
	[Header("Button settings")]
	public DIRECTION PushableFromDirection;
	public bool triggerOnRelease;

	bool pushed = false;
	ButtonState state = ButtonState.Waiting;

	public abstract void ButtonPushed();
	public abstract void ButtonReleased();

	void Update () {
		UpdateRaycastOrigins ();
		
		bool triggered = CheckForCollision();
		
		switch (state) {
		case ButtonState.Waiting:
			if (triggered) {
				state = ButtonState.Pushed;
			}
			break;
		case ButtonState.Pushed:
			if (!pushed) {
				pushed = true;
				ButtonPushed();
			} else if (!triggered) {
				pushed = false;

				if (triggerOnRelease) {
					ButtonReleased();
					state = ButtonState.Waiting;
				} else {
					state = ButtonState.Used;
				}
			}
			break;
		case ButtonState.Used:
			//TODO change color?
			break;
		}
	}

	bool CheckForCollision() {
		switch (PushableFromDirection) {
			case DIRECTION.DOWN:
			case DIRECTION.UP:
				return VerticalCollisions();
			case DIRECTION.LEFT:
			case DIRECTION.RIGHT:
				return HorizontalCollisions();
		}

		return false;
	}

	bool VerticalCollisions() {
		float rayLength = skinWidth * 2;
		
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (PushableFromDirection == DIRECTION.DOWN) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Helper.Vector3FromDIRECTIONS(PushableFromDirection), rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
			
			if (hit) {
				return true;
			}
		}
		return false;
	}

	bool HorizontalCollisions() {
		float rayLength = skinWidth * 2;
		
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (PushableFromDirection == DIRECTION.LEFT) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Helper.Vector3FromDIRECTIONS(PushableFromDirection), rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
			
			if (hit) {
				return true;
			}
		}
		return false;
	}
}
