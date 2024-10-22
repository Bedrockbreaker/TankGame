using UnityEngine;

using Util;

namespace AI {
	/**
	 * <summary>
	 * Return true if the player is within a spherical range
	 * </summary>
	 */
	public class IsPlayerNear : AICondition {

		protected float distance;

		public IsPlayerNear(float distance) {
			this.distance = distance;
		}

		protected override bool EvaluateInternal(
			Blackboard blackboard,
			AIController controller
		) {
			if (!controller.PawnOptional) return false;
			Optional<Transform> playerTransform = blackboard
				.Get<Optional<Transform>>("PlayerTransform");
			if (!playerTransform) return false;
			return (
				playerTransform.Value.position
				- controller.Pawn.transform.position
			).sqrMagnitude < distance * distance;
		}
	}
}