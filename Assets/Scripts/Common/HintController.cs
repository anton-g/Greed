using UnityEngine;
using System.Collections;

public class HintController : MonoBehaviour {
	public GameObject target;

	void LateUpdate () {
		float x = Mathf.Clamp(target.transform.position.x, -33.0f, 33.0f);
		float y = Mathf.Clamp(target.transform.position.y, -18.0f, 18.0f);
		transform.position = new Vector3(x, y, 0.0f);
	}
}
