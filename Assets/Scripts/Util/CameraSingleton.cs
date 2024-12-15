using UnityEngine;

public class CameraSingleton : MonoBehaviour {

	public static CameraSingleton Instance { get; protected set; }

	public CameraSingleton() {
		// Unity objects should not use coalescing assignment. (UNT0023)
		// Instance ??= this;
		if (Instance != null) return;
		Instance = this;
	}

	private void Awake() {
		if (Instance != this) {
			Instance.gameObject.SetActive(true);
			Destroy(gameObject);
			return;
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}
}