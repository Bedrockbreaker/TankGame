using System;

using UnityEngine;

using Util;

/**
 * <summary>
 * Generic status effect -- buff, debuff, etc.
 * </summary>
 */
public abstract class StatusEffect {

	[field: SerializeField]
	public float Duration { get; protected set; }
	[field: SerializeField]
	public float Strength { get; protected set; }
	[field: SerializeField]
	public Optional<Controller> AppliedBy { get; protected set; }

	public StatusEffect(float duration, float strength, Optional<Controller> appliedBy) {
		Duration = duration;
		Strength = strength;
		AppliedBy = appliedBy;
	}

	public virtual void Reapply(StatusEffect newEffect) {
		Duration = Math.Max(Duration, newEffect.Duration);
		Strength = Math.Max(Strength, newEffect.Strength);
		AppliedBy = newEffect.AppliedBy;
	}

	/**
	 * <summary>
	 * Apply the effect, initializing any state
	 * </summary>
	 */
	public virtual void Apply(Pawn pawn) { }

	/**
	 * <summary>
	 * Tick the effect
	 * </summary>
	 */
	public virtual void Tick(Pawn pawn) {
		Duration -= Time.deltaTime;
	}

	/**
	 * <summary>
	 * Remove the effect, optionally clearing without triggering effects
	 * </summary>
	 */
	public virtual void Remove(Pawn pawn, bool clear = false) {
		Duration = 0;
	}
}