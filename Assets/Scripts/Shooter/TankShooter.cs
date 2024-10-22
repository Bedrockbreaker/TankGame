using UnityEngine;

/**
 * <summary>
 * Shooter for tanks
 * </summary>
 */
public class TankShooter : Shooter {

	public override bool Shoot() {
		if (!base.Shoot()) return false;

		GameObject projectile = Instantiate(
			projectilePrefab,
			spawnTransform.position,
			spawnTransform.rotation
		);
		projectile.GetComponent<ContactDamager>().owner = owner;
		projectile.GetComponent<Rigidbody>().AddForce(transform.forward * force);

		return true;
	}
}