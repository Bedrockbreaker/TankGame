using UnityEngine;
using UnityEngine.AI;

using Util;

namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Attempts to move towards a given point.<br/>
	 * Requires a NavMeshAgent
	 * </summary>
	 */
	public class MoveToPointBehavior : Node {

		// TODO: implement some interface that exposes properties for the visual node graph
		protected BlackboardKey<Vector3> targetKey;
		protected Optional<Vector3> optionalTarget = Optional<Vector3>.None;
		protected float acceptanceRadius;
		protected NavMeshAgent agent;
		protected NodeState state = NodeState.Running;

		public MoveToPointBehavior(
			Optional<Vector3> target,
			float acceptanceRadius
		) {
			optionalTarget = target;
			this.acceptanceRadius = acceptanceRadius;
		}

		public MoveToPointBehavior(
			BlackboardKey<Vector3> targetKey,
			float acceptanceRadius
		) {
			this.targetKey = targetKey;
			this.acceptanceRadius = acceptanceRadius;
		}

		public override void Start(AIController controller) {
			Optional<Pawn> pawnOptional = controller.PawnOptional;
			if (!pawnOptional) {
				state = NodeState.Failure;
				return;
			}
			Pawn pawn = pawnOptional.Value;

			agent = pawn.GetComponent<NavMeshAgent>();
			if (agent == null) {
				state = NodeState.Failure;
				return;
			}
			agent.isStopped = false;
			agent.stoppingDistance = acceptanceRadius;

			Vector3 target = optionalTarget
				? optionalTarget.Value
				: Blackboard.Get(targetKey);

			pawn.MoveTo(target);
			state = NodeState.Running;
		}

		public override NodeState Tick(AIController controller) {
			// TODO: update agent destination
			// TODO: ensure child classes also update agent destination
			if (state != NodeState.Running) return state;

			if (agent.isPathStale) {
				state = NodeState.Failure;
			}

			if (agent.remainingDistance <= agent.stoppingDistance) {
				state = NodeState.Success;
			}

			return state;
		}

		public override void Stop(AIController controller) {
			agent.isStopped = true;
		}
	}
}