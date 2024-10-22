using UnityEngine;

using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Attempts to move away from a given transform.<br/>
	 * Requires a NavMeshAgent
	 * </summary>
	 */
	public class AvoidTransformBehavior : MoveToPointBehavior {

		protected BlackboardKey<Optional<Transform>> transformKey;

		public AvoidTransformBehavior(
			BlackboardKey<Optional<Transform>> transformKey,
			float acceptanceRadius
		) : base(Optional<Vector3>.None, acceptanceRadius) {
			this.transformKey = transformKey;
		}

		public override void Start(AIController controller) {
			Optional<Transform> avoidTarget = Blackboard.Get(transformKey);
			if (!avoidTarget) {
				state = NodeState.Failure;
				return;
			}

			if (!controller.PawnOptional) {
				state = NodeState.Failure;
				return;
			}

			optionalTarget =
				3 * controller.Pawn.transform.position
				- 2 * avoidTarget.Value.position;

			base.Start(controller);
		}
	}
}