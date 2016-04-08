using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public bool shouldFollow;

	[HideInInspector]
	public GameObject p1;
	[HideInInspector]
	public GameObject p2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!shouldFollow) {
			return;
		}

		float targetX = Mathf.Max (p1.transform.position.x, p2.transform.position.x);

		transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
	}
}
