using UnityEngine;
using System.Collections;

public class PlatformButton : ButtonController {
	public PlatformController[] targetPlatforms;
	
	public override void ButtonPushed() {
		foreach (var pc in targetPlatforms) {
			pc.activated = !pc.activated;
		}
	}
	
	public override void ButtonReleased() {
		foreach (var pc in targetPlatforms) {
			pc.activated = !pc.activated;
		}
	}
}
