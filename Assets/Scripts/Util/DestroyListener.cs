using System;

using UnityEngine;

public class DestroyListener : MonoBehaviour {
	public event Action OnDestroyed;

	private void OnDestroy() {
		OnDestroyed?.Invoke();
	}
}