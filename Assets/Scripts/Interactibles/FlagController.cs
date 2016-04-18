using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class FlagController : MonoBehaviour {

    public Animator anim;
    public LayerMask targets;
    public bool activated;
	
    BoxCollider2D col;
    
    void Start() {
        col = GetComponent<BoxCollider2D> ();
    }
    
	// Update is called once per frame
	void Update () {
	    if (!activated) {
            Vector2 bottomLeft = new Vector2 (col.bounds.min.x, col.bounds.min.y);
            Vector2 topRight = new Vector2 (col.bounds.max.x, col.bounds.max.y);
            if (Physics2D.OverlapArea(bottomLeft, topRight, targets)) {
                activated = true;
                
                if (anim)
                    anim.SetBool("activated", activated);
            }
        }
	}
}
