namespace AI.StateMachine {
	/**
	 * <summary>
	 * A state machine for AI
	 * </summary>
	 */
	public class StateMachine {

		protected AIState currentState;

		public StateMachine(
			AIState initialState,
			AIController controller
		) {
			currentState = initialState;
			currentState.EnterState(controller);
		}

		/**
		 * <summary>
		 * Tick the state machine
		 * </summary>
		 */
		public void Tick(AIController controller) {
			currentState.Tick(controller);

			AIState newState = currentState.CheckTransitions(controller);
			if (newState == currentState) return;

			currentState.ExitState(controller);
			currentState = newState;
			currentState.EnterState(controller);
		}
	}
}