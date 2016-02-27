using UnityEngine;
using System.Collections;

public class DoorButton : ButtonController {
	public DoorController[] targetDoors;

	public override void ButtonPushed() {
		foreach (var dc in targetDoors) {
			dc.Open();
		}
	}

	public override void ButtonReleased() {
		foreach (var dc in targetDoors) {
			dc.Close();
		}
	}

	void OnDrawGizmosSelected() {
		foreach (var item in targetDoors) {
			Gizmos.color = new Color(255.0f, 255.0f, 255.0f, 0.3f);
			Gizmos.DrawLine(transform.position, item.transform.position);
		}
	}
}
