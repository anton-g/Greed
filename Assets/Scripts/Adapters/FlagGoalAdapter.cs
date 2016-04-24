using UnityEngine;
using System.Collections;

public class FlagGoalAdapter : MonoBehaviour {

	public FlagController[] flags;
	public GoalController[] goals;
	
	void Update() {
		bool done = true;
		foreach	(FlagController f in flags) {
			done = done && f.activated;
		}
		
		if (done) {
			foreach	(GoalController c in goals) {
				c.overrideActive = true;
			}
		}
	}
}
