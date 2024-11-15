using System;

using Util;

/**
 * <summary>
 * An effect that instantly kills the target after a set duration.<br/>
 * Level has no effect
 * </summary>
 */
public class MortalCoilEffect : StatusEffect {

	// FIXME: remove this constructor once TemplatedTypeDrawer is fixed
	public MortalCoilEffect(
		float duration,
		float strength,
		Optional<Controller> appliedBy
	) : base(duration, strength, appliedBy) { }

	public MortalCoilEffect(
		float duration,
		Optional<Controller> appliedBy
	) : base(duration, 1, appliedBy) { }

	public override void Reapply(StatusEffect newEffect) {
		float oldDuration = Duration;
		base.Reapply(newEffect);
		Duration = Math.Min(oldDuration, newEffect.Duration);
	}

	public override void Remove(Pawn pawn, bool clear = false) {
		base.Remove(pawn, clear);
		if (!clear && pawn.TryGetComponent<Health>(out var health)) {
			health.SetDead(AppliedBy);
		}
	}
}