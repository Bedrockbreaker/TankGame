using UnityEngine;

/**
 * <summary>
 * Cursed projectile. Applies mortal coil on contact
 * </summary>
 */
public class CursedProjectile : Projectile {

	public float effectDurationSeconds = 10;

	public override void Start() {
		base.Start();

		OnContact += Hit;
	}

	public void Hit(Collider other) {
		if (other.TryGetComponent<StatusEffectManager>(
			out var statusEffectManager
		)) {
			statusEffectManager.Apply(new MortalCoilEffect(
				effectDurationSeconds,
				1,
				owner
			));
		}

		Destroy(gameObject);
	}
}