using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public Transform camTransform;
    
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    
    float currentShakeDuration;
    bool shaking;
    
    Vector3 origPos;
    
    void Awake() {
        if (camTransform == null) {
            camTransform = gameObject.transform;
        }
        
        currentShakeDuration = shakeDuration;
    }
    
    void OnEnable() {
        origPos = camTransform.localPosition;
    }
    
    void Update() {
        if (shaking && currentShakeDuration > 0) {
            camTransform.localPosition = origPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        } else {
            shaking = false;
            currentShakeDuration = shakeDuration;
            camTransform.localPosition = origPos;
        }
    }
    
    public void Shake(float magnitude, float duration) {
        shaking = true;
        
        currentShakeDuration = Mathf.Max(currentShakeDuration, duration);
    }
}
