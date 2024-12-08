using UnityEngine;

/**
 * <summary>
 * Shooter for tanks
 * </summary>
 */
public class TankShooter : Shooter {

	public override bool Shoot() {
		if (!base.Shoot()) return false;

		Projectile projectile = Instantiate(
			projectilePrefab,
			spawnTransform.position,
			spawnTransform.rotation
		);

		projectile.owner = owner;

		Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
		projectileRigidbody.AddForce(transform.forward * force);

		return true;
	}
}