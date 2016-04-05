using UnityEngine;
using UnityEditor;

public class Rotator : MonoBehaviour {
    [Header("Test")]
    public RotateBehaviour behaviour = RotateBehaviour.RotateAroundSelf;
    public RotateDirection direction = RotateDirection.Clockwise;
    public float speed = 20f;
    
    public Transform rotateTarget;
    public Vector3 rotatePoint;

	void Start () {
	
	}
	
	void Update () {
        switch (behaviour)
        {
            case RotateBehaviour.RotateAroundSelf:
            transform.Rotate(Vector3.forward * (int)direction * speed * Time.deltaTime);
            break;
            case RotateBehaviour.RotateAroundPoint:
            transform.RotateAround(rotatePoint, Vector3.forward * (int)direction, speed * Time.deltaTime);
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

[CustomEditor(typeof(Rotator))]
[CanEditMultipleObjects]
public class RotatorEditor : Editor {
    public override void OnInspectorGUI() {        
        Rotator rotator = target as Rotator;
        
        rotator.behaviour = (RotateBehaviour)EditorGUILayout.EnumPopup("Behaviour:", rotator.behaviour);
        rotator.direction = (RotateDirection)EditorGUILayout.EnumPopup("Direction:", rotator.direction);
        rotator.speed = EditorGUILayout.FloatField("Speed:", rotator.speed);
        
        EditorGUILayout.Separator();
        
        switch (rotator.behaviour)
        {
            case RotateBehaviour.RotateAroundSelf:
            break;
            case RotateBehaviour.RotateAroundPoint:
                rotator.rotatePoint = EditorGUILayout.Vector3Field("Rotate Point:", Vector3.zero);
            break;
            case RotateBehaviour.RotateAroundObject:
                rotator.rotateTarget = (Transform)EditorGUILayout.ObjectField("Target", rotator.rotateTarget, typeof(Transform), true);
            break;
        }
    }
}