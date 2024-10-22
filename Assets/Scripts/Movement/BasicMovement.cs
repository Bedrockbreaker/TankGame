using UnityEngine;

/**
 * <summary>
 * Provides a basic movement component
 * </summary>
 */
public abstract class BasicMovement : MonoBehaviour {

	[SerializeField]
	new protected Rigidbody rigidbody;

	public virtual void Start() { }

	public virtual void Update() { }

	/**
	 * <summary>
	 * Move the rigidbody with the given direction and speed.<br/>
	 * Call continuously in the Update function
	 * </summary>
	 */
	public abstract void Move(Vector3 direction, float amount);


	/**
	 * <summary>
	 * Set a destination for the rigidbody.<br/>
	 * Call once.
	 * </summary>
	 */
	public abstract void MoveTo(Vector3 position);

	/**
	 * <summary>
	 * Cancel a previous call to MoveTo
	 * </summary>
	 */
	public abstract void CancelMove();

	/**
	 * <summary>
	 * Rotate the rigidbody with the given angular speed.<br/>
	 * Call continuously in the Update function
	 * </summary>
	 * <param name="amount">The angular speed in radians per second</param>
	 */
	public abstract void Rotate(float amount);

	/**
	 * <summary>
	 * Rotate the rigidbody to look at the given position.<br/>
	 * Call once.
	 * </summary>
	 */
	public abstract void Focus(Vector3 position);

	/**
	 * <summary>
	 * Rotate the rigidbody to look at the given transform.<br/>
	 * Call once.
	 * </summary>
	 */
	public abstract void Focus(Transform transform);

	/**
	 * <summary>
	 * Cancel a previous call to Focus
	 * </summary>
	 */
	public abstract void ClearFocus();
}