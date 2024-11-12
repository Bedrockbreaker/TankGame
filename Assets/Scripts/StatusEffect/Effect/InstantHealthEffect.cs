using Util;

/**
 * <summary>
 * Instantly heal the target.<br/>
 * Duration has no effect
 * </summary>
 */
public class InstantHealthEffect : StatusEffect {

	public InstantHealthEffect(
		float amount,
		Optional<Controller> appliedBy
	) : base(0, amount, appliedBy) { }

	public override void Apply(Pawn pawn) {
		if (!pawn.TryGetComponent<Health>(out var health)) return;

		health.Heal(Strength);
		Remove(pawn);
	}
}