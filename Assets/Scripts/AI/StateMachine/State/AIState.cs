using System.Collections.Generic;

namespace AI.StateMachine {
	/**
	 * <summary>
	 * A state for an AI
	 * </summary>
	 */
	public abstract class AIState {

		protected Blackboard blackboard;
		protected BehaviorTree.BehaviorTree behaviorTree;

		public List<StateTransition> Transitions { get; } = new();

		public AIState(Blackboard blackboard) {
			this.blackboard = blackboard;
		}

		/**
		 * <summary>
		 * Set up the state
		 * </summary>
		 */
		public virtual void EnterState(AIController controller) {
			behaviorTree.EnterTree(controller);
			behaviorTree.Start(controller);
		}

		/**
		 * <summary>
		 * Process the state
		 * </summary>
		 */
		public virtual void Tick(AIController controller) {
			behaviorTree.Tick(controller);
		}

		/**
		 * <summary>
		 * Tear down the state
		 * </summary>
		 */
		public virtual void ExitState(AIController controller) {
			behaviorTree.Stop(controller);
			behaviorTree.ExitTree(controller);
		}

		/**
		 * <summary>
		 * Check for state transitions
		 * </summary>
		 */
		public virtual AIState CheckTransitions(AIController controller) {
			foreach (StateTransition transition in Transitions) {
				if (transition.CanTransition(blackboard, controller)) {
					return transition.TargetState;
				}
			}

			return this;
		}
	}
}