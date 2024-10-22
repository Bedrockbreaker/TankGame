using UnityEngine;

using Util;

namespace AI {
	/**
	 * <summary>
	 * Return true if the controller's pawn can see the player
	 * </summary>
	 */
	public class CanSeePlayer : AICondition {

		protected float halfFOV;
		protected float maxDistance;

		public CanSeePlayer(float halfFOV, float maxDistance) {
			this.halfFOV = halfFOV;
			this.maxDistance = maxDistance;
		}

		protected override bool EvaluateInternal(
			Blackboard blackboard,
			AIController controller
		) {
			if (!controller.PawnOptional) return false;
			Optional<Transform> playerTransform = blackboard
				.Get<Optional<Transform>>("PlayerTransform");
			if (!playerTransform) return false;

			float angle = Vector3.Angle(
				controller.Pawn.transform.forward,
				playerTransform.Value.position - controller.Pawn.transform.position
			);
			if (angle > halfFOV) return false;

			// FIXME: provide offeset in constructor? "eye offset"?
			Vector3 offset = new(0, .6f, 0);

			Physics.Raycast(
				controller.Pawn.transform.position + offset,
				playerTransform.Value.position - controller.Pawn.transform.position,
				out RaycastHit hit,
				maxDistance
			);

			return hit.transform == playerTransform.Value;
		}
	}
}