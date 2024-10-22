using UnityEngine;

/**
 * <summary>
 * Base class for all projectiles
 * </summary>
 */
public abstract class Projectile : MonoBehaviour {

	public float lifetime = 7f;

	public virtual void Start() {
		Destroy(gameObject, lifetime);
	}
}