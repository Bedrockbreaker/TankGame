using UnityEngine;

public class MenuSingleton : MonoBehaviour {

	public static MenuSingleton Instance { get; protected set; }

	public MenuSingleton() {
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