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
}
