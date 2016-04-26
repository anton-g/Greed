using UnityEngine;
using System.Collections;

public class PlayerCosmetics : MonoBehaviour {

    [Header("Appearance")]
    public Transform leftEye;
    public Transform rightEye;
    public SpriteRenderer graphic;
    
    public float minBlinkInterval = 5f;
    public float maxBlinkInterval = 20f;
    
    [Header("Squish and Stretch")]
    public float maxScaleChange = 0.45f;
	
    Vector3 leftEyeIdle;
    Vector3 rightEyeIdle;
    Vector3 eyesOrigScale;
    Vector3 graphicOrigScale;
    float eyeBlinkTime;
    
    void Awake() {
        leftEyeIdle = leftEye.localPosition;
        rightEyeIdle = rightEye.localPosition;
        eyesOrigScale = leftEye.localScale;
    }
    
    void Start() {
        graphicOrigScale = graphic.transform.localScale;
        
        eyeBlinkTime = Random.Range(minBlinkInterval, maxBlinkInterval);
        InvokeRepeating("BlinkEyes", eyeBlinkTime, eyeBlinkTime);
    }
    
    public void Run(Vector3 velocity, float moveSpeed, bool collisionBelow) {
        MoveEyes(velocity, moveSpeed, collisionBelow);
        
        SquishAndStretch(velocity);
    }
    
    void MoveEyes(Vector3 velocity, float moveSpeed, bool collisionBelow) {
        float leftX = leftEyeIdle.x;
        float leftY = leftEyeIdle.y;
        float rightX = rightEyeIdle.x;
        float rightY = rightEyeIdle.y;
        float scale = eyesOrigScale.y;
        
        float eyeDistanceModifier = 0.0f;
        
        //Vertical
        if (velocity.y > 0) {
            //Up
            leftY = 0.18f * (velocity.y / 40) + leftEyeIdle.y;
            rightY = 0.18f * (velocity.y / 40) + rightEyeIdle.y;
            scale *= velocity.y / 40;
            eyeDistanceModifier = 0.04f * (velocity.y / 40);
        } else if (velocity.y < 0 && !collisionBelow) {
            //Down
            leftY = 0.18f * (velocity.y / 40) + leftEyeIdle.y;
            rightY = 0.18f * (velocity.y / 40) + rightEyeIdle.y;
            eyeDistanceModifier = 0.04f * (velocity.y / 40);
        }
        
        //Horizontal
        if (velocity.x < -1) {
            //Left
            leftX = 0.12f * (velocity.x / moveSpeed) + leftEyeIdle.x;
            rightX = 0.17f * (velocity.x / moveSpeed) + rightEyeIdle.x;
        } else if (velocity.x > 1) {
            //Right
            leftX = 0.17f * (velocity.x / moveSpeed) + leftEyeIdle.x;
            rightX = 0.12f * (velocity.x / moveSpeed) + rightEyeIdle.x;
        }
        
        leftX += eyeDistanceModifier;
        rightX -= eyeDistanceModifier;
        
        Vector3 leftPos = new Vector3(leftX, leftY, -0.1f);
        leftEye.localPosition = Vector3.Lerp(leftEye.localPosition, leftPos, 0.1f);
        leftEye.localScale = Vector3.Lerp(leftEye.localScale, new Vector3(leftEye.localScale.x, scale, leftEye.localScale.z), 0.05f);
        
        Vector3 rightPos = new Vector3(rightX, rightY, -0.1f);
        rightEye.localPosition = Vector3.Lerp(rightEye.localPosition, rightPos, 0.1f); 
        rightEye.localScale = Vector3.Lerp(rightEye.localScale, new Vector3(rightEye.localScale.x, scale, rightEye.localScale.z), 0.05f);
    }
    
    void BlinkEyes() {
        if (gameObject.activeSelf)
            StartCoroutine(Blink());
    }
    
    void SquishAndStretch(Vector3 velocity) {
        float scaleChange = (velocity.y / 40) * maxScaleChange;
        
        Vector3 targetScale = new Vector3(graphicOrigScale.x - scaleChange, graphicOrigScale.y, graphicOrigScale.z);
        graphic.transform.localScale = Vector3.Lerp(graphic.transform.localScale, targetScale, 0.05f);
    }
    
    IEnumerator Blink() {
        rightEye.gameObject.SetActive(false);
        leftEye.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(0.1f);
        
        rightEye.gameObject.SetActive(true);
        leftEye.gameObject.SetActive(true);
    }
}
