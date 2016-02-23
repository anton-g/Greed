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
	
	[Range(-1, 1)]
	public int direction = 1;
	
	bool pushed = false;
	ButtonState state = ButtonState.Waiting;
	
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
		float rayLength = skinWidth * 2;
		
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (direction == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
			
			if (hit) {
				return true;
			}
		}
		return false;
	}

	public abstract void ButtonPushed();
	public abstract void ButtonReleased();
}
