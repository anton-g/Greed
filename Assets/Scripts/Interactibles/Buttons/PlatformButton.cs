using UnityEngine;
using System.Collections;

public class PlatformButton : ButtonController {
	public PlatformController targetPlatform;
	
	public override void ButtonPushed() {
		targetPlatform.activated = !targetPlatform.activated;
	}
	
	public override void ButtonReleased() {
		targetPlatform.activated = !targetPlatform.activated;
	}
}
