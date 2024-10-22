using UnityEngine;

using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Attempts to move towards a given transform.<br/>
	 * Requires a NavMeshAgent
	 * </summary>
	 */
	public class MoveToTransformBehavior : MoveToPointBehavior {

		protected BlackboardKey<Optional<Transform>> transformKey;

		public MoveToTransformBehavior(
			BlackboardKey<Optional<Transform>> transformKey,
			float acceptanceRadius
		) : base(Optional<Vector3>.None, acceptanceRadius) {
			this.transformKey = transformKey;
		}

		public override void Start(AIController controller) {
			Optional<Transform> target = Blackboard.Get(transformKey);
			if (!target) {
				state = NodeState.Failure;
				return;
			}
			optionalTarget = target.Value.position;

			base.Start(controller);
		}
	}
}