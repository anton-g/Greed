using UnityEngine;
using System.Collections;

public class GoalButton : ButtonController {
	public GoalController[] targetGoals;
	
	public override void ButtonPushed() {
		foreach (var dc in targetGoals) {
			dc.Toggle();
		}
	}
	
	public override void ButtonReleased() {
		foreach (var dc in targetGoals) {
			dc.Toggle();
		}
	}

	void OnDrawGizmosSelected() {
		foreach (var item in targetGoals) {
			Gizmos.color = new Color(255.0f, 255.0f, 255.0f, 0.3f);
			Gizmos.DrawLine(transform.position, item.transform.position);
		}
	}
}
