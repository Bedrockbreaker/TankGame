using System.Collections.Generic;

using AI.BehaviorTree;

namespace AI.StateMachine {
	/**
	 * <summary>
	 * Flee state. Move away from player
	 * </summary>
	 */
	public class FleeState : AIState {
		public FleeState(Blackboard blackboard) : base(blackboard) {
			behaviorTree = new(blackboard, new Sequence(new List<Node> {
				new AvoidTransformBehavior("PlayerTransform", 0.1f)
			}));
		}
	}
}