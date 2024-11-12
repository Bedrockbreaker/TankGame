using System;
using System.Collections.Generic;

using UnityEngine;

using Util;

/**
 * <summary>
 * Component for managing status effects
 * </summary>
 */
public class StatusEffectManager : MonoBehaviour {

	[field: SerializeField]
	public Pawn Pawn { get; protected set; }
	[field: SerializeField]
	[field: ReadOnly]
	public List<StatusEffect> Effects { get; } = new();

	/**
	 * <summary>
	 * Apply an effect
	 * </summary>
	 */
	public void Apply(StatusEffect effect) {
		Optional<StatusEffect> currentEffect
			= Effects.Find(e => e.GetType() == effect.GetType());
		if (currentEffect) {
			currentEffect.Value.Reapply(effect);
		} else {
			Effects.Add(effect);
			effect.Apply(Pawn);
		}
	}

	/**
	 * <summary>
	 * Remove an effect, optionally clearing without triggering effects
	 * </summary>
	 */
	public void Remove(StatusEffect effect, bool clear = false) {
		effect.Remove(Pawn, clear);
		Effects.Remove(effect);
	}

	/**
	 * <summary>
	 * Clear all effects without triggering effects
	 * </summary>
	 */
	public void ClearEffects() {
		foreach (StatusEffect effect in Effects) {
			Remove(effect, true); // clear without triggering effects
		}
	}

	/**
	 * <summary>
	 * Get an effect
	 * </summary>
	 */
	public Optional<StatusEffect> GetEffect(Type type) {
		foreach (StatusEffect effect in Effects) {
			if (effect.GetType() == type) {
				return effect;
			}
		}
		return Optional<StatusEffect>.None;
	}

	public void Update() {
		for (int i = Effects.Count - 1; i >= 0; i--) {
			StatusEffect effect = Effects[i];
			effect.Tick(Pawn);
			if (effect.Duration <= 0) {
				Remove(effect);
			}
		}
	}
}