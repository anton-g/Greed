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

	void OnDrawGizmosSelected() {
		foreach (var item in targetPlatforms) {
			Gizmos.color = new Color(255.0f, 255.0f, 255.0f, 0.3f);
			Gizmos.DrawLine(transform.position, item.transform.position);
		}
	}
}
