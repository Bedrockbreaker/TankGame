using UnityEngine;

using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Continously rotate towards a given object.<br/>
	 * Returns success immediately, keeps running
	 * </summary>
	 */
	public class FocusObjectBehavior : Node {

		public BlackboardKey<Optional<Transform>> targetKey;

		public FocusObjectBehavior(BlackboardKey<Optional<Transform>> targetKey) {
			this.targetKey = targetKey;
		}

		public override NodeState Tick(AIController controller) {
			Optional<Pawn> pawnOptional = controller.PawnOptional;
			if (!pawnOptional) return NodeState.Failure;
			Pawn pawn = pawnOptional.Value;

			Optional<Transform> target = Blackboard.Get(targetKey);
			if (!target) return NodeState.Failure;

			pawn.Focus(target.Value);
			return NodeState.Success;
		}

		public override void ExitTree(AIController controller) {
			Optional<Pawn> pawnOptional = controller.PawnOptional;
			if (!pawnOptional) return;
			Pawn pawn = pawnOptional.Value;
			pawn.ClearFocus();
		}
	}
}