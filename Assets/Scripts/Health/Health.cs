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

	[SerializeField]
	private float currentHealth;
	[SerializeField]
	private float maxHealth;

	public float MaxHealth {
		get => maxHealth;
		protected set {
			bool changed = value != maxHealth;
			maxHealth = value;
			CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
			if (changed) OnHealthChanged?.Invoke();
		}
	}
	public float CurrentHealth {
		get => currentHealth;
		protected set {
			bool changed = value != currentHealth;
			currentHealth = value;
			if (changed) OnHealthChanged?.Invoke();
		}
	}
	public bool IsDead => CurrentHealth <= 0f;

	public event Action OnHealthChanged;
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

	public virtual void Start() {
		CurrentHealth = currentHealth;
	}
}