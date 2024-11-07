using Util;

/**
 * <summary>
 * An effect that instantly kills the target after a set duration.<br/>
 * Level has no effect
 * </summary>
 */
public class MortalCoilEffect : StatusEffect {

	public MortalCoilEffect(
		float duration,
		int level,
		Optional<Controller> appliedBy
	) : base(duration, level, appliedBy) { }

	public override void Remove(Pawn pawn, bool clear = false) {
		base.Remove(pawn, clear);
		if (!clear && pawn.TryGetComponent<Health>(out var health)) {
			health.SetDead(AppliedBy);
		}
	}
}