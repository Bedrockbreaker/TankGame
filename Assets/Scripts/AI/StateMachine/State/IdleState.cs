using AI.BehaviorTree;

namespace AI.StateMachine {
	/**
	 * <summary>
	 * Idle state. Do bare minimum
	 * </summary>
	 */
	public class IdleState : AIState {

		public IdleState(Blackboard blackboard) : base(blackboard) {
			behaviorTree = new(blackboard, new WaitBehavior(5f));
		}
	}
}