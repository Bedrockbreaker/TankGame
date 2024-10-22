using System.Collections.Generic;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Ticks all children until one returns success.<br/>
	 * If all fail, returns failure
	 * </summary>
	 */
	public class Selector : Node {

		protected int currentIndex = 0;

		public readonly List<Node> children;

		private Blackboard blackboard;
		public override Blackboard Blackboard {
			protected get => blackboard;
			set {
				blackboard = value;
				children.ForEach(child => child.Blackboard = value);
			}
		}

		public Selector(List<Node> children) {
			this.children = children;
		}

		public override void EnterTree(AIController controller) {
			children.ForEach(child => child.EnterTree(controller));
		}

		public override void Start(AIController controller) {
			currentIndex = 0;
			children[currentIndex].Start(controller);
		}

		public override NodeState Tick(AIController controller) {
			for (; currentIndex < children.Count; currentIndex++) {
				NodeState childState = children[currentIndex].Tick(controller);

				if (childState == NodeState.Success) {
					currentIndex = 0;
					return NodeState.Success;
				}

				if (childState == NodeState.Running) {
					return NodeState.Running;
				}

				children[currentIndex].Stop(controller);
				if (currentIndex < children.Count - 1) {
					children[currentIndex + 1].Start(controller);
				}
			}

			currentIndex = 0;
			return NodeState.Failure;
		}

		public override void Stop(AIController controller) {
			children[currentIndex].Stop(controller);
		}

		public override void ExitTree(AIController controller) {
			children.ForEach(child => child.ExitTree(controller));
		}
	}
}