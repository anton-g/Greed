public class GoalButton : ButtonController<GoalController> {
	public override void ButtonPushed() {
		foreach (var gc in targets) {
			gc.Toggle();
		}
	}
	
	public override void ButtonReleased() {
		foreach (var gc in targets) {
			gc.Toggle();
		}
	}
}
