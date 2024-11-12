using UnityEngine;

using Util;

/**
 * <summary>
 * Regenerates health over time
 * </summary>
 */
public class RegenEffect : StatusEffect {

	public RegenEffect(
		float duration,
		float strength,
		Optional<Controller> appliedBy
	) : base(duration, strength, appliedBy) { }

	public Optional<Health> Health { get; protected set; } = Optional<Health>.None;

	public override void Apply(Pawn pawn) {
		Health = pawn.GetComponent<Health>();
	}

	public override void Tick(Pawn pawn) {
		base.Tick(pawn);

		if (!Health) return;
		Health.Value.Heal(Strength * Time.deltaTime * 10f);
	}
}