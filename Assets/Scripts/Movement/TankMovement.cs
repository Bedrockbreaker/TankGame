using UnityEngine;

/**
 * <summary>
 * Movement component for tanks
 * </summary>
 */
public class TankMovement : BasicMovement {

	public override void Move(Vector3 direction, float speed) {
		rigidbody.MovePosition(rigidbody.position + speed * Time.deltaTime * direction);
	}

	public override void Rotate(float angularSpeed) {
		transform.Rotate(0, angularSpeed * Time.deltaTime * Mathf.Rad2Deg, 0);
	}
}