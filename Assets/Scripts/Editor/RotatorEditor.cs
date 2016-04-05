using UnityEngine;
using UnityEditor;

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
                rotator.rotatePoint = EditorGUILayout.Vector3Field("Rotate Point:", rotator.rotatePoint);
            break;
            case RotateBehaviour.RotateAroundObject:
                rotator.rotateTarget = (Transform)EditorGUILayout.ObjectField("Target", rotator.rotateTarget, typeof(Transform), true);
            break;
        }
        
        if (GUI.changed) EditorUtility.SetDirty (target);
    }
}