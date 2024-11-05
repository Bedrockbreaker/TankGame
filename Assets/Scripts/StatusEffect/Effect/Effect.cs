using UnityEngine;

using Util;

/**
 * <summary>
 * Generic status effect -- buff, debuff, etc.
 * </summary>
 */
public abstract class Effect {

	[field: SerializeField]
	public float Duration { get; protected set; }
	[field: SerializeField]
	public int Level { get; protected set; }
	[field: SerializeField]
	public Optional<Controller> AppliedBy { get; protected set; }

	public Effect(float duration, int level, Optional<Controller> appliedBy) {
		Duration = duration;
		Level = level;
		AppliedBy = appliedBy;
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
		if (Duration <= 0) {
			Remove(pawn);
		}
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