public class BeamButton : ButtonController<BeamController> {
	public override void ButtonPushed() {
		foreach (var bc in targets) {
			bc.Toggle();
		}
	}
	
	public override void ButtonReleased() {
		foreach (var bc in targets) {
			bc.Toggle();
		}
	}
}
