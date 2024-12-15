using UnityEngine;

public abstract class Menu : MonoBehaviour {

	public virtual void Awake() {
		if (GameManager.Instance.ActiveMenu && GameManager.Instance.ActiveMenu.Value == this) {
			return;
		}
		gameObject.SetActive(false);
	}
}