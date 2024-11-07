using UnityEngine;

/**
 * <summary>
 * Basic projectile. Damages on contact and then destroys itself.
 * </summary>
 */
public class BasicProjectile : Projectile {

	[field: SerializeField]
	public ContactDamager Damager { get; protected set; }

	public override void Start() {
		base.Start();

		Damager.owner = owner;

		OnContact += (other) => Destroy(gameObject);
	}
}