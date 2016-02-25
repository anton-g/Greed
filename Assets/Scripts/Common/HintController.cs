using UnityEngine;
using System.Collections;

public class HintController : MonoBehaviour {
	public GameObject target;
	public float edgeInset = 1.5f;
	
	void Start() {
		//TODO probably exists better way to do this. Maybe 2 hint-prefabs with materials predefined instead.
		gameObject.GetComponent<Renderer>().material = target.GetComponent<Renderer>().material;
	}

	void LateUpdate () {
		Bounds bounds = Camera.main.OrthographicBounds();
		float maxX = bounds.center.x + bounds.extents.x - edgeInset;
		float minX = bounds.center.x - bounds.extents.x + edgeInset;
		float maxY = bounds.center.y + bounds.extents.y - edgeInset;
		float minY = bounds.center.y - bounds.extents.y + edgeInset;

		float clampedX = Mathf.Clamp(target.transform.position.x, minX, maxX);
		float clampedY = Mathf.Clamp(target.transform.position.y, minY, maxY);
		transform.position = new Vector3(clampedX, clampedY, 0.0f);
	}
}
