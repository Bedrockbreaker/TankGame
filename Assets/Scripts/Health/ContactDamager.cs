using UnityEngine;

using Util;

/**
 * <summary>
 * Damages health components on contact
 * </summary>
 */
public class ContactDamager : MonoBehaviour {

	public float damage = 10f;
	public Optional<Controller> owner = Optional<Controller>.None;

	public void OnTriggerEnter(Collider other) {
		if (other.TryGetComponent<Health>(out var healthOther)) {
			healthOther.Damage(damage, owner);
		}

		Destroy(gameObject);
	}
}