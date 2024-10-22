using UnityEngine;
using UnityEngine.AI;

public class TankNavMeshAgent : TankMovement {

	[SerializeField]
	protected NavMeshAgent agent;

	public override void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.updatePosition = false;
		agent.updateRotation = false;

		agent.angularSpeed = maxAngularSpeed * Mathf.Rad2Deg;
		agent.speed = maxLinearSpeed;
	}

	public override void Update() {
		if (
			agent.hasPath
			&& !agent.pathPending
			&& agent.remainingDistance > agent.stoppingDistance
		) {
			Vector3 direction = agent.steeringTarget - transform.position;
			direction.y = 0;

			float angle = Vector3.SignedAngle(
				transform.forward,
				direction,
				transform.up
			);

			if (Mathf.Abs(angle) > 0.01f) {
				Rotate(angle);
			}

			Vector3 projected = Vector3.Project(direction, transform.forward);
			if (Vector3.Dot(projected, transform.forward) < 0) {
				projected = Vector3.zero;
			}
			float speed = projected.sqrMagnitude / direction.sqrMagnitude;

			Move(transform.forward, speed);

		} else if (focusTransform || focusPosition) {
			Rotate(Vector3.SignedAngle(
				transform.forward,
				(
					focusTransform
						? focusTransform.Value.position
						: focusPosition.Value
				) - rigidbody.position,
				transform.up
			) * Mathf.Deg2Rad);
		}
		agent.nextPosition = transform.position;
	}

	public override void Move(Vector3 direction, float amount) {
		base.Move(direction, amount);
		// agent.nextPosition = transform.position;
	}

	public override void MoveTo(Vector3 position) {
		agent.SetDestination(position);
		// Debug.Log($"Moving to {position}");
	}

	public override void CancelMove() {
		agent.isStopped = true;
	}
}