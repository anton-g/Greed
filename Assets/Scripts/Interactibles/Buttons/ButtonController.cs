using UnityEngine;
using System.Collections;

public enum ButtonState {
	Waiting,
	Pushed
}

/// <summary>
/// Inherit from this class to create a button that controlls a specific component
/// </summary>
public abstract class ButtonController : RayCastController {
	public bool triggerOnRelease;

	public DIRECTION PushableFromDirection;
	
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
				state = ButtonState.Waiting;

				if (triggerOnRelease) {
					ButtonReleased();
				}
			}
			break;
		}
	}

	bool CheckForCollision() {
		switch (PushableFromDirection) {
			case DIRECTION.DOWN:
			case DIRECTION.UP:
				return VerticalCollisions();
				break;
			case DIRECTION.LEFT:
			case DIRECTION.RIGHT:
				return HorizontalCollisions();
				break;
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
