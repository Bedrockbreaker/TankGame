using System.Collections.Generic;

namespace AI {
	/**
	 * <summary>
	 * Return true if at least one condition is true
	 * </summary>
	 */
	public class SomeCondition : AICondition {

		protected List<AICondition> conditions;

		public SomeCondition(List<AICondition> conditions) {
			this.conditions = conditions;
		}

		protected override bool EvaluateInternal(
			Blackboard blackboard,
			AIController controller
		) {
			return conditions.Exists(condition => {
				return condition.Evaluate(blackboard, controller);
			});
		}
	}
}