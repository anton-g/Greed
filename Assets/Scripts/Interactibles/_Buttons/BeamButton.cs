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
}
