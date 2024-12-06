using System;

using UnityEngine;

using Util;

public delegate void OnDeathHandler(Optional<Controller> attacker);

/**
 * <summary>
 * Health component for objects
 * </summary>
 */
public class Health : MonoBehaviour {

	[field: SerializeField]
	public float MaxHealth { get; protected set; } = 100f;
	[field: SerializeField]
	public float CurrentHealth { get; protected set; } = 100f;
	public bool IsDead => CurrentHealth <= 0f;

	public event OnDeathHandler OnDeath;

	/**
	 * <summary>
	 * Damage this object by an amount, with an optional attacker
	 * </summary>
	 * <remarks>
	 * Invokes <see cref="OnDeath"/> if the object becomes dead
	 * </remarks>
	 */
	public virtual void Damage(float amount, Optional<Controller> attacker) {
		CurrentHealth = Math.Max(CurrentHealth - amount, 0f);
		if (CurrentHealth <= 0f) {
			SetDead(attacker); // this.SetDead(...)
		}
	}

	/**
	 * <summary>
	 * Heal this object by an amount.<br/>
	 * Clamps to the maximum health
	 * </summary>
	 */
	public virtual void Heal(float amount) {
		CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
	}

	/**
	 * <summary>
	 * Sets the health to 0 and invokes <see cref="OnDeath"/>
	 * </summary>
	 */
	public virtual void SetDead(Optional<Controller> attacker) {
		CurrentHealth = 0f;
		OnDeath?.Invoke(attacker); // this.OnDeath?.Invoke(...)

		Destroy(gameObject);
	}
}