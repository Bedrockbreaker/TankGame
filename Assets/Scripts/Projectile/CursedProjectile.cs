using UnityEngine;

/**
 * <summary>
 * Cursed projectile. Applies mortal coil on contact
 * </summary>
 */
public class CursedProjectile : Projectile {

	public float effectDurationSeconds = 10;

	public override void Hit(Collider other) {
		base.Hit(other);

		if (other.TryGetComponent<StatusEffectManager>(
			out var statusEffectManager
		)) {
			statusEffectManager.Apply(new MortalCoilEffect(
				effectDurationSeconds,
				owner
			));
		}

		Destroy(gameObject);
	}
}