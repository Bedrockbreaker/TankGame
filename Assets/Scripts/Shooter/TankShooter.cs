using UnityEngine;

/**
 * <summary>
 * Shooter for tanks
 * </summary>
 */
public class TankShooter : Shooter {

	public override bool Shoot() {
		if (!base.Shoot()) return false;

		GameObject projectileObject = Instantiate(
			projectilePrefab,
			spawnTransform.position,
			spawnTransform.rotation
		);

		Projectile projectile = projectileObject.GetComponent<Projectile>();
		projectile.owner = owner;

		Rigidbody projectileRigidbody = projectileObject.GetComponent<Rigidbody>();
		projectileRigidbody.AddForce(transform.forward * force);

		return true;
	}
}