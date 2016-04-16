using UnityEngine;
using System.Collections;

public class ShrinkKeyController : KeyController {

    public Shrink[] targets;
    public float delay = 0.1f;
	// Use this for initialization
	public override void Collect() {
        StartCoroutine(shrinkTargets());
    }
    
    IEnumerator shrinkTargets() {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].isShrinking = true;
            yield return new WaitForSeconds(delay);
        }
        
        gameObject.SetActive(false);
    }
}
