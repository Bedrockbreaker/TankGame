using UnityEngine;

/**
 * <summary>
 * Incendiary projectile. Applies napalm on contact
 * </summary>
 */
public class IncendiaryProjectile : Projectile {

	public float effectDurationSeconds = 3;
	public int effectLevel = 1;

	public override void Start() {
		base.Start();

		OnContact += Hit;
	}

	public void Hit(Collider other) {
		if (other.TryGetComponent<StatusEffectManager>(
			out var statusEffectManager
		)) {
			statusEffectManager.Apply(new NapalmEffect(
				effectDurationSeconds,
				effectLevel,
				owner
			));
		}

		Destroy(gameObject);
	}
}