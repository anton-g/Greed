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
	[Header("Button appearance")]
	public Material pushedMaterial;

	[Header("Button settings")]
	public DIRECTION pushableFromDirection;
	public bool triggerOnRelease;

	private bool pushed = false;
	private ButtonState state = ButtonState.Waiting; 
	private Renderer rend;
	private Material origMat;

	public abstract void ButtonPushed();
	public abstract void ButtonReleased();

	public override void Awake() {
		base.Awake();

		rend = GetComponent<Renderer>();
		origMat = rend.material;
	}

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
				TriggerButtonPushed();
			} else if (!triggered) {
				pushed = false;
				if (triggerOnRelease) {
					TriggerButtonReleased();
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

	void TriggerButtonPushed() {
		rend.material = pushedMaterial;

		pushed = true;
		ButtonPushed();
	}

	void TriggerButtonReleased() {
		rend.material = origMat;

		ButtonReleased();
		state = ButtonState.Waiting;
	}

	bool CheckForCollision() {
		switch (pushableFromDirection) {
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
			Vector2 rayOrigin = (pushableFromDirection == DIRECTION.DOWN) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Helper.Vector3FromDIRECTIONS(pushableFromDirection), rayLength, collisionMask);
			
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
			Vector2 rayOrigin = (pushableFromDirection == DIRECTION.LEFT) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Helper.Vector3FromDIRECTIONS(pushableFromDirection), rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
			
			if (hit) {
				return true;
			}
		}
		return false;
	}
}
