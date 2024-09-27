using UnityEngine;

/**
 * <summary>
 * Provides a basic movement component
 * </summary>
 */
public abstract class BasicMovement : MonoBehaviour {

	[SerializeField]
	new protected Rigidbody rigidbody;

	/**
	 * <summary>
	 * Move the rigidbody with the given direction and speed.<br/>
	 * Call continuously in the Update function
	 * </summary>
	 */
	public abstract void Move(Vector3 direction, float speed);

	/**
	 * <summary>
	 * Rotate the rigidbody with the given angular speed.<br/>
	 * Call continuously in the Update function
	 * </summary>
	 * <param name="angularSpeed">The angular speed in radians per second</param>
	 */
	public abstract void Rotate(float angularSpeed);
}