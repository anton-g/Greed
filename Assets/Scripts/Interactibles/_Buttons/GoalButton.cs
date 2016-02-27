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
}
