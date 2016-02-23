using UnityEngine;
using System.Collections;

public class DoorButton : ButtonController {
	public DoorController targetDoor;

	public override void ButtonPushed() {
		targetDoor.Open();
	}

	public override void ButtonReleased() {
		targetDoor.Close();
	}
}
