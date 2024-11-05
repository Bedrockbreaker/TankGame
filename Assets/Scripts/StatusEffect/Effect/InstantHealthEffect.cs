using Util;

/**
 * <summary>
 * Instantly heal the target.<br/>
 * Duration has no effect
 * </summary>
 */
public class InstantHealthEffect : Effect {

	public InstantHealthEffect(
		float duration,
		int level,
		Optional<Controller> appliedBy
	) : base(duration, level, appliedBy) { }

	public override void Apply(Pawn pawn) {
		if (!pawn.TryGetComponent<Health>(out var health)) return;

		health.Heal(Level * 10f);
		Remove(pawn);
	}
}