using UnityEngine;

public class Rotator : MonoBehaviour {
    [Header("Test")]
    public RotateBehaviour behaviour = RotateBehaviour.RotateAroundSelf;
    public RotateDirection direction = RotateDirection.Clockwise;
    public float speed = 20f;
    
    public Transform rotateTarget;
    public Vector3 rotatePoint;

	void Update () {
        switch (behaviour)
        {
            case RotateBehaviour.RotateAroundSelf:
            transform.Rotate(Vector3.forward * (int)direction * speed * Time.deltaTime);
            break;
            case RotateBehaviour.RotateAroundPoint:
            transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward * (int)direction, speed * Time.deltaTime);
            break;
            case RotateBehaviour.RotateAroundObject:
            if (rotateTarget != null) {
                transform.RotateAround(rotateTarget.position, Vector3.forward * (int)direction, speed * Time.deltaTime);
            } else {
                Debug.LogError("Need to set object to rotate around.");
            }
            break;
        }
	}
    
    void OnDrawGizmos() {
        if (behaviour == RotateBehaviour.RotateAroundPoint) {
			Gizmos.color = Color.red;
			float size = .3f;

            Vector3 globalPoint = (Application.isPlaying) ? transform.TransformPoint(rotatePoint) : rotatePoint + transform.position;
            Gizmos.DrawLine(globalPoint - Vector3.up * size, globalPoint + Vector3.up * size);
            Gizmos.DrawLine(globalPoint - Vector3.left * size, globalPoint + Vector3.left * size);
		}
    }
}
public enum RotateDirection {
    Clockwise = -1,
    CounterClockwise = 1
}

public enum RotateBehaviour {
    RotateAroundSelf,
    RotateAroundPoint,
    RotateAroundObject
}