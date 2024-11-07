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
	public List<Effect> Effects { get; } = new();

	/**
	 * <summary>
	 * Apply an effect
	 * </summary>
	 */
	public void Apply(Effect effect) {
		Effects.Add(effect);
		effect.Apply(Pawn);
	}

	/**
	 * <summary>
	 * Remove an effect, optionally clearing without triggering effects
	 * </summary>
	 */
	public void Remove(Effect effect, bool clear = false) {
		effect.Remove(Pawn, clear);
		Effects.Remove(effect);
	}

	/**
	 * <summary>
	 * Clear all effects without triggering effects
	 * </summary>
	 */
	public void ClearEffects() {
		foreach (Effect effect in Effects) {
			Remove(effect, true); // clear without triggering effects
		}
	}

	/**
	 * <summary>
	 * Get an effect
	 * </summary>
	 */
	public Optional<Effect> GetEffect(Type type) {
		foreach (Effect effect in Effects) {
			if (effect.GetType() == type) {
				return effect;
			}
		}
		return Optional<Effect>.None;
	}

	public void Update() {
		for (int i = Effects.Count - 1; i >= 0; i--) {
			Effect effect = Effects[i];
			effect.Tick(Pawn);
			if (effect.Duration <= 0) {
				Remove(effect);
			}
		}
	}
}