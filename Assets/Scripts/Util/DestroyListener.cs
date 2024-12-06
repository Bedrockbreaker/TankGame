using System;

using UnityEngine;

/**
 * <summary>
 * Listens for object destruction and invokes an event
 * </summary>
 */
public class DestroyListener : MonoBehaviour {
	public event Action OnDestroyed;

	private void OnDestroy() {
		OnDestroyed?.Invoke();
	}
}