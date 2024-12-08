using System;

using UnityEngine;

/**
 * <summary>
 * Listens for object destruction and invokes an event
 * </summary>
 */
public class DestroyListener : MonoBehaviour {
	public event Action OnDestroyed;
	public event Action OnAfterDestroyed;

	private void OnDestroy() {
		OnDestroyed?.Invoke();
		Invoke(nameof(Delay), 0f);
	}

	private void Delay() {
		OnAfterDestroyed?.Invoke();
	}
}