using System;
using System.Collections.Generic;
using System.Linq;

using AI.Sense;

using UnityEngine;

using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Attempts to move towards the nearest audible sound.<br/>
	 * Requires a NavMeshAgent
	 * </summary>
	 */
	public class InvestigateSoundBehavior : MoveToPointBehavior {

		protected float maxDistance;

		public InvestigateSoundBehavior(
			float maxDistance,
			float acceptanceRadius
		) : base(Optional<Vector3>.None, acceptanceRadius) {
			this.maxDistance = maxDistance;
		}

		public override void Start(AIController controller) {
			List<Sound> sounds = Blackboard.Get(GameManager.Instance.SoundsKey);

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

			if (!nearestSound.Item1) {
				state = NodeState.Failure;
				return;
			}

			if (
				nearestSound.Item2
				> (maxDistance + nearestSound.Item1.Value.volume)
				* (maxDistance + nearestSound.Item1.Value.volume)
			) {
				state = NodeState.Failure;
				return;
			}

			optionalTarget = nearestSound.Item1.Value.position;

			base.Start(controller);
		}
	}
}