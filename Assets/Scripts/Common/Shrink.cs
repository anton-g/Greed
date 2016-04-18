using UnityEngine;

public class Shrink : MonoBehaviour {

    public bool isShrinking;
    public float step = 0.05f;
	
	// Update is called once per frame
	void LateUpdate () {
	    if (isShrinking) {
            gameObject.transform.localScale -= new Vector3(step, step, 0.0f);
            
            if (gameObject.transform.localScale.x < step)
            {
                Destroy(gameObject);
            }
        }
	}
}
