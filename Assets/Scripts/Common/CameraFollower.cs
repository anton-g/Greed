using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;

	public Vector2 focusAreaMinSize;

	FocusArea focusArea;

	void Start() {
		focusArea = new FocusArea(player1.GetComponent<Controller2D>().col.bounds, player2.GetComponent<Controller2D>().col.bounds, focusAreaMinSize);
	}

	void LateUpdate() {
		//Move camera
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1,0,0,0.5f);
		Gizmos.DrawCube(focusArea.center, focusAreaMinSize);
	}

	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;

		float left, right;
		float top, bottom;

		public FocusArea(Bounds target1Bounds, Bounds target2Bounds, Vector2 size) {
			left = Mathf.Min(target1Bounds.center.x, target2Bounds.center.x);
			right = Mathf.Max(target1Bounds.center.x, target2Bounds.center.x);
			bottom = Mathf.Min(target1Bounds.min.y, target2Bounds.min.y);
			top = Mathf.Max (target1Bounds.min.y, target2Bounds.min.y);
			/*left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;*/
			
			center = new Vector2((left+right)/2, (top+bottom)/2);
			
			velocity = Vector2.zero;
		}
	}
}
