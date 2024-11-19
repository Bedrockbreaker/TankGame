using Util;

/**
 * <summary>
 * Instantly adds score to the target.<br/>
 * Duration has no effect
 * </summary>
 */
public class InstantScoreEffect : StatusEffect {

	// FIXME: remove this constructor once TemplatedTypeDrawer is fixed
	public InstantScoreEffect(
		float duration,
		float amount,
		Optional<Controller> appliedBy
	) : base(duration, amount, appliedBy) { }

	public InstantScoreEffect(
		float amount
	) : base(0, amount, Optional<Controller>.None) { }

	public override void Apply(Pawn pawn) {
		if (!pawn.ControllerOptional) return;

		if (Strength > 0) {
			pawn.Controller.AddScore((int)Strength);
		} else if (Strength < 0) {
			pawn.Controller.RemoveScore((int)Strength);
		}

		Remove(pawn);
	}
}