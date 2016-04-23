using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {
	public float time;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if (time < 0.0f) Destroy(gameObject);
		
		time -= Time.deltaTime;
	}
}
