using System.Collections.Generic;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Ticks all children until one returns failure.<br/>
	 * If all succeed, returns success
	 * </summary>
	 */
	public class Sequence : Node {

		protected int currentIndex = 0;

		public readonly List<Node> children = new();

		private Blackboard blackboard;
		public override Blackboard Blackboard {
			protected get => blackboard;
			set {
				blackboard = value;
				children.ForEach(child => child.Blackboard = blackboard);
			}
		}

		public Sequence(List<Node> children) {
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

				if (childState == NodeState.Running) {
					return NodeState.Running;
				}

				if (childState == NodeState.Failure) {
					currentIndex = 0;
					return NodeState.Failure;
				}

				children[currentIndex].Stop(controller);
				if (currentIndex < children.Count - 1) {
					children[currentIndex + 1].Start(controller);
				}
			}

			currentIndex = 0;
			return NodeState.Success;
		}

		public override void Stop(AIController controller) {
			children[currentIndex].Stop(controller);
		}

		public override void ExitTree(AIController controller) {
			children.ForEach(child => child.ExitTree(controller));
		}
	}
}