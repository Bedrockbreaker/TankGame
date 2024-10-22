using UnityEngine;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Waits for a given amount of seconds
	 * </summary>
	 */
	public class WaitBehavior : Node {

		// TODO: see todo in MoveToPointBehavior.cs
		protected float duration;
		protected float timeLeft;

		public WaitBehavior(float duration) {
			this.duration = duration;
			timeLeft = duration;
		}

		public override void Start(AIController controller) {
			timeLeft = duration;
		}

		public override NodeState Tick(AIController controller) {
			if (timeLeft <= 0) return NodeState.Success;
			timeLeft -= Time.deltaTime * Time.timeScale;
			return NodeState.Running;
		}
	}
}