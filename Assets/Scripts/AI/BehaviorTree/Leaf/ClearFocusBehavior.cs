using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Clears the current focus
	 * </summary>
	 */
	public class ClearFocusBehavior : Node {

		public override NodeState Tick(AIController controller) {
			Optional<Pawn> pawnOptional = controller.PawnOptional;
			if (!pawnOptional) return NodeState.Failure;
			Pawn pawn = pawnOptional.Value;

			pawn.ClearFocus();
			return NodeState.Success;
		}
	}
}