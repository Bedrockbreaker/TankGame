using UnityEngine;

using Util;

/**
 * <summary>
 * Base class for all projectiles
 * </summary>
 */
public abstract class Projectile : ContactListener {

	[ReadOnly]
	public Optional<Controller> owner = Optional<Controller>.None;
	public float lifetime = 7f;
	public AudioClip hitSound;

	public virtual void Start() {
		Destroy(gameObject, lifetime);

		OnContact += Hit;
	}

	/**
	 * <summary>
	 * Called when the projectile hits something
	 * </summary>
	 */
	public virtual void Hit(Collider other) {
		GameManager.Instance.PlayOneShot(hitSound);
	}
}