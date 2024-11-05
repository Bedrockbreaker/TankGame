using UnityEngine;

using Util;

/**
 * <summary>
 * Regenerates health over time
 * </summary>
 */
public class RegenEffect : Effect {

	public RegenEffect(
		float duration,
		int level,
		Optional<Controller> appliedBy
	) : base(duration, level, appliedBy) { }

	public Optional<Health> Health { get; protected set; } = Optional<Health>.None;

	public override void Apply(Pawn pawn) {
		Health = pawn.GetComponent<Health>();
	}

	public override void Tick(Pawn pawn) {
		base.Tick(pawn);

		if (!Health) return;
		Health.Value.Heal(Level * Time.deltaTime * 10f);
	}
}