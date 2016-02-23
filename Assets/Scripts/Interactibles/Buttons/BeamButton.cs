using UnityEngine;
using System.Collections;

public class BeamButton : ButtonController {
	public BeamController targetBeam;
	
	public override void ButtonPushed() {
		targetBeam.Toggle();
	}
	
	public override void ButtonReleased() {
		targetBeam.Toggle();
	}
}
