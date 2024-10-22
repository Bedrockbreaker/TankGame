using System.Collections.Generic;

namespace AI {
	/**
	 * <summary>
	 * Return true if every condition is true
	 * </summary>
	 */
	public class EveryCondition : AICondition {

		protected List<AICondition> conditions;

		public EveryCondition(List<AICondition> conditions) {
			this.conditions = conditions;
		}

		protected override bool EvaluateInternal(
			Blackboard blackboard,
			AIController controller
		) {
			return conditions.TrueForAll(condition => {
				return condition.Evaluate(blackboard, controller);
			});
		}
	}
}