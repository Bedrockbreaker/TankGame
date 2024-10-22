namespace AI {
	/**
	* <summary>
	* Basic condition for BehaviorTree or StateTransition
	* </summary>
	*/
	public abstract class AICondition {

		protected bool inverse = false;

		/**
		 * <summary>
		 * Internal evaluation function
		 * </summary>
		 */
		protected abstract bool EvaluateInternal(
			Blackboard blackboard,
			AIController controller
		);

		/**
		 * <summary>
		 * Evaluate the condition
		 * </summary>
		 */
		public virtual bool Evaluate(
			Blackboard blackboard,
			AIController controller
		) {
			bool result = EvaluateInternal(blackboard, controller);
			return inverse ? !result : result;
		}

		/**
		 * <summary>
		 * Invert the condition
		 * </summary>
		 */
		public virtual AICondition Inverse() {
			inverse = !inverse;
			return this;
		}
	}
}