using UnityEngine;

/**
 * <summary>
 * The base class for all pawns
 * </summary>
 */
public abstract class Pawn : MonoBehaviour {

	public float angularSpeed = 0;
	public float linearSpeed = 0;

	[SerializeField]
	protected BasicMovement Movement;

	public virtual void Start() {
		GameManager.Instance.RegisterPawn(this);
	}

	/**
	 * <summary>
	 * Move along the pawn's relative forward axis
	 * </summary>
	 */
	public abstract void Move(float distance);

	/**
	 * <summary>
	 * Rotate counterclockwise around the the pawn's relative up axis
	 * </summary>
	 */
	public abstract void Rotate(float radians);

	public virtual void OnDestroy() {
		GameManager.Instance.UnregisterPawn(this);
	}
}