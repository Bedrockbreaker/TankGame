using System;

using UnityEngine;

using Util;

/**
 * <summary>
 * Movement component for tanks
 * </summary>
 */
public class TankMovement : BasicMovement {

	protected Optional<Vector3> movePosition;
	protected Optional<Transform> focusTransform;
	protected Optional<Vector3> focusPosition;

	public float speed = 10;
	public float maxLinearSpeed = 10;
	public float angularSpeed = Mathf.PI / 2f;
	public float maxAngularSpeed = Mathf.PI / 2f;

	public override void Update() {
		if (movePosition) {
			// TODO: dumb implementation with raycasts
		}

		if (focusTransform || focusPosition) {
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
	}

	public override void Move(Vector3 direction, float amount) {
		float delta = Math.Clamp(amount * speed, -maxLinearSpeed, maxLinearSpeed) * Time.deltaTime;
		rigidbody.MovePosition(rigidbody.position + delta * direction.normalized);
	}

	public override void MoveTo(Vector3 position) {
		movePosition = position;
	}

	public override void CancelMove() {
		movePosition = Optional<Vector3>.None;
	}

	public override void Rotate(float amount) {
		float delta = Mathf.Clamp(
			amount * angularSpeed,
			-maxAngularSpeed,
			maxAngularSpeed
		) * Time.deltaTime;
		transform.Rotate(0, delta * Mathf.Rad2Deg, 0);
	}

	public override void Focus(Vector3 position) {
		focusPosition = position;
	}

	public override void Focus(Transform transform) {
		focusTransform = transform;
	}

	public override void ClearFocus() {
		focusTransform = Optional<Transform>.None;
		focusPosition = Optional<Vector3>.None;
	}
}