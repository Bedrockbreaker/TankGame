using System;

using UnityEngine;

/**
 * <summary>
 * Listens for contact triggers (overlaps)
 * </summary>
 */
public class ContactListener : MonoBehaviour {

	public event Action<Collider> OnContact;

	public void OnTriggerEnter(Collider other) {
		OnContact?.Invoke(other);
	}
}