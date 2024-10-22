using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Commands the pawn to shoot
	 * </summary>
	 */
	public class ShootBehavior : Node {

		public override NodeState Tick(AIController controller) {
			Optional<Pawn> pawnOptional = controller.PawnOptional;
			if (!pawnOptional) return NodeState.Failure;

			Pawn pawn = pawnOptional.Value;
			bool didShoot = pawn.Shoot();
			return didShoot ? NodeState.Success : NodeState.Failure;
		}
	}
}