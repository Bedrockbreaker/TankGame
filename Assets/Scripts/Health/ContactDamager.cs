using UnityEngine;

using Util;

/**
 * <summary>
 * Damages health components on contact
 * </summary>
 */
public class ContactDamager : ContactListener {

	public float damage = 10f;
	public Optional<Controller> owner = Optional<Controller>.None;

	public void Start() {
		OnContact += Damage;
	}

	public void Damage(Collider other) {
		if (other.TryGetComponent<Health>(out var healthOther)) {
			healthOther.Damage(damage, owner);
		}
	}
}