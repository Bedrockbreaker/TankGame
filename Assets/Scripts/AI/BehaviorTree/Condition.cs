using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Executes a condition and returns success or failure
	 * </summary>
	 */
	public class Condition : Node {

		protected Optional<Node> child = Optional<Node>.None;
		protected AICondition condition;

		public Condition(AICondition condition) {
			this.condition = condition;
		}

		public Condition(
			AICondition condition,
			Optional<Node> child
		) {
			this.condition = condition;
			this.child = child;
		}

		public override void EnterTree(AIController controller) {
			if (child) {
				child.Value.EnterTree(controller);
			}
		}

		public override void Start(AIController controller) {
			if (child && condition.Evaluate(Blackboard, controller)) {
				child.Value.Start(controller);
			}
		}

		public override NodeState Tick(AIController controller) {
			bool result = condition.Evaluate(Blackboard, controller);
			if (child && result) {
				return child.Value.Tick(controller);
			}
			return result ? NodeState.Success : NodeState.Failure;
		}

		public override void Stop(AIController controller) {
			if (child) {
				child.Value.Stop(controller);
			}
		}

		public override void ExitTree(AIController controller) {
			if (child) {
				child.Value.ExitTree(controller);
			}
		}
	}
}