using UnityEngine;
using System.Collections;

public enum LevelState {
	Playing,
	Completed,
	Failed
}

public class LevelController : MonoBehaviour {
	//TODO should really this be done like this?
	//Maybe gamemanager can pass the prefabs to every levelmanager instead.
	[Header("Setup")]
	public GameObject player1Prefab;
	public GameObject player2Prefab;

	[Header("Level setup")]
	public GoalController Goal1;
	public GoalController Goal2;

	public Vector3[] spawnPoints;

	[HideInInspector]
	public LevelState state = LevelState.Playing;

	GameObject player1Object;
	GameObject player2Object;
	
	void Start () {
		player1Object = Instantiate(player1Prefab, spawnPoints[0], Quaternion.identity) as GameObject;
		player2Object = Instantiate(player2Prefab, spawnPoints[1], Quaternion.identity) as GameObject;
	}

	void Update () {
		if (!player1Object.activeSelf || !player2Object.activeSelf) {
			state = LevelState.Failed;
		}

		bool first = Goal1.CheckPlayerInGoal();
		bool second = Goal2.CheckPlayerInGoal();
		if (first && second) {
			state = LevelState.Completed;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		float size = 0.5f;

		for (int i = 0; i < spawnPoints.Length; i++) {
			Gizmos.DrawLine(spawnPoints[i] - Vector3.up * size, spawnPoints[i] + Vector3.up * size);
			Gizmos.DrawLine(spawnPoints[i] - Vector3.left * size, spawnPoints[i] + Vector3.left * size);
		}
	}
}
