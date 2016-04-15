using UnityEngine;

public class DataManager : MonoBehaviour {
    public static DataManager Instance = null;
    [HideInInspector]
    public int reachedLevel;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }    
        DontDestroyOnLoad(gameObject);
        
        reachedLevel = 1;
    }

    void Update()
    {
        
    }
}