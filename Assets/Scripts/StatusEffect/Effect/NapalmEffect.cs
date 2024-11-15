using UnityEngine;

using Util;

/**
 * <summary>
 * Damages health over time
 * </summary>
 */
public class NapalmEffect : StatusEffect {

	[Template]
	public NapalmEffect(
		float duration,
		float strength,
		Optional<Controller> appliedBy
	) : base(duration, strength, appliedBy) { }

	public Optional<Health> Health { get; protected set; } = Optional<Health>.None;

	public override void Reapply(StatusEffect newEffect) {
		float oldStrength = Strength;
		base.Reapply(newEffect);
		Strength = oldStrength + newEffect.Strength;
	}

	public override void Apply(Pawn pawn) {
		Health = pawn.GetComponent<Health>();
	}

	public override void Tick(Pawn pawn) {
		base.Tick(pawn);

		if (!Health) return;
		Health.Value.Damage(Strength * Time.deltaTime * 10f, AppliedBy);
	}
}