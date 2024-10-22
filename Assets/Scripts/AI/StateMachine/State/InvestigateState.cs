using System.Collections.Generic;

using AI.BehaviorTree;

namespace AI.StateMachine {
	/**
	 * <summary>
	 * Investigate state. Move to nearest sound
	 * </summary>
	 */
	public class InvestigateState : AIState {
		public InvestigateState(Blackboard blackboard) : base(blackboard) {
			behaviorTree = new(blackboard, new Sequence(new List<Node> {
				new InvestigateSoundBehavior(10f, 1f),
				new WaitBehavior(1f)
			}));
		}
	}
}