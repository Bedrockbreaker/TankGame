namespace AI.StateMachine {
	/**
	 * <summary>
	 * Defines a conditional state transition to a target state
	 * </summary>
	 */
	public class StateTransition {

		protected AICondition condition;
		public AIState TargetState { get; protected set; }

		public StateTransition(
			AIState targetState,
			AICondition condition
		) {
			TargetState = targetState;
			this.condition = condition;
		}

		/**
		 * <summary>
		 * Check if the transition can occur
		 * </summary>
		 */
		public bool CanTransition(
			Blackboard blackboard,
			AIController controller
		) {
			return condition.Evaluate(blackboard, controller);
		}
	}
}