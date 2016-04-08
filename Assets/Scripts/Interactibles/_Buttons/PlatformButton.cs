public class PlatformButton : ButtonController<PlatformController> {
	public override void ButtonPushed() {
		foreach (var pc in targets) {
			pc.activated = !pc.activated;
		}
	}
	
	public override void ButtonReleased() {
		foreach (var pc in targets) {
			pc.activated = !pc.activated;
		}
	}
}
