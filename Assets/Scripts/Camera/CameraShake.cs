using UnityEngine;

public class CameraShake : MonoBehaviour {
	public Transform camTransform;
    
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    
    float currentShakeDuration = 0.0f;
    bool shaking = false;
    
    Vector3 origPos;
    
    void Awake() {        
        if (camTransform == null) {
            camTransform = gameObject.transform;
        }
    }
    
    void OnEnable() {
        origPos = camTransform.localPosition;
    }
    
    void LateUpdate() {
        if (shaking && currentShakeDuration > 0) {
            camTransform.localPosition = origPos + Random.insideUnitSphere * shakeAmount;
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
        } else {
            shaking = false;
            camTransform.localPosition = origPos;
        }
    }
    
    public void Shake(float magnitude, float duration) {
        shaking = true;
        currentShakeDuration = Mathf.Max(currentShakeDuration, duration);
        shakeAmount = magnitude;
    }
}
