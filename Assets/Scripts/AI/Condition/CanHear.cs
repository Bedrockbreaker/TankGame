using System;
using System.Collections.Generic;
using System.Linq;

using AI.Sense;

using Util;

namespace AI {
	/**
	 * <summary>
	 * Return true if the player is within a spherical range
	 * </summary>
	 */
	public class CanHear : AICondition {

		protected Optional<Controller> soundOwner;
		protected float maxDistance;

		public CanHear(Optional<Controller> soundOwner, float maxDistance) {
			this.soundOwner = soundOwner;
			this.maxDistance = maxDistance;
		}

		protected override bool EvaluateInternal(
			Blackboard blackboard,
			AIController controller
		) {
			if (!controller.PawnOptional) return false;

			List<Sound> sounds = blackboard.Get(GameManager.Instance.SoundsKey);

			Tuple<Optional<Sound>, float> nearestSound = sounds.Aggregate(
				new Tuple<Optional<Sound>, float>(Optional<Sound>.None, float.MaxValue),
				(nearest, current) => {
					float distanceSquared = (
						current.position
						- controller.Pawn.transform.position
					).sqrMagnitude;

					return distanceSquared < nearest.Item2
						? new(current, distanceSquared)
						: nearest;
				});

			if (!nearestSound.Item1) return false;

			return nearestSound.Item2
				<= (maxDistance + nearestSound.Item1.Value.volume)
				* (maxDistance + nearestSound.Item1.Value.volume);
		}
	}
}