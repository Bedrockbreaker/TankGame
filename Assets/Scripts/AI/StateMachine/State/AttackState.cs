using System.Collections.Generic;

using AI.BehaviorTree;

namespace AI.StateMachine {
	/**
	 * <summary>
	 * Attack state. Move to point and attack
	 * </summary>
	 */
	public class AttackState : AIState {
		public AttackState(Blackboard blackboard) : base(blackboard) {
			behaviorTree = new(blackboard, new Sequence(new List<Node> {
				new MoveToTransformBehavior("PlayerTransform", 3f),
				new FocusObjectBehavior("PlayerTransform"),
				new ShootBehavior(),
				new ClearFocusBehavior()
			}));
		}
	}
}