using UnityEngine;
using System.Collections;

public class DoorButton : ButtonController<DoorController> {
	public override void ButtonPushed() {
		foreach (var dc in targets) {
			dc.Open();
		}
	}

	public override void ButtonReleased() {
		foreach (var dc in targets) {
			dc.Close();
		}
	}
}
