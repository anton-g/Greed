using UnityEngine;
using System.Collections;

public class BeamButton : ButtonController {
	public BeamController[] targetBeams;
	
	public override void ButtonPushed() {
		foreach (var bc in targetBeams) {
			bc.Toggle();
		}
	}
	
	public override void ButtonReleased() {
		foreach (var bc in targetBeams) {
			bc.Toggle();
		}
	}

	void OnDrawGizmosSelected() {
		foreach (var item in targetBeams) {
			Gizmos.color = new Color(255.0f, 255.0f, 255.0f, 0.3f);
			Gizmos.DrawLine(transform.position, item.transform.position);
		}
	}
}
