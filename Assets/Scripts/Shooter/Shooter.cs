using System.Collections;
using System.Collections.Generic;

using AI;
using AI.Sense;

using UnityEngine;

using Util;

/**
 * <summary>
 * The base class for all shooters
 * </summary>
 */
public abstract class Shooter : MonoBehaviour {

	[SerializeField]
	protected float force = 1000f;
	[SerializeField]
	protected float cooldown = 0.5f;
	[SerializeField]
	protected float volume = 5f;
	protected bool canShoot = true;

	public Transform spawnTransform;
	public GameObject projectilePrefab;
	public Optional<Controller> owner;

	/**
	 * <summary>
	 * Cooldown before the next shot can be fired
	 * </summary>
	 */
	protected virtual IEnumerator RateLimit() {
		canShoot = false;
		yield return new WaitForSeconds(cooldown);
		canShoot = true;
	}

	/**
	 * <summary>
	 * Launch a projectile
	 * </summary>
	 */
	public virtual bool Shoot() {
		if (!canShoot) return false;

		List<Sound> sounds = Blackboard.GLOBAL.Get(GameManager.Instance.SoundsKey);
		sounds.Add(new(sounds, owner, spawnTransform.position, volume));

		StartCoroutine(RateLimit());
		return true;
	}
}