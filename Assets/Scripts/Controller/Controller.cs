using UnityEngine;
using UnityEngine.InputSystem;

/**
 * <summary>
 * The base class for all pawn controllers
 * </summary>
 */
public abstract class Controller : MonoBehaviour {

	protected InputAction inputMove;

	public Pawn pawn;

	/**
	 * <summary>
	 * Handle input from the InputSystem
	 * </summary>
	 */
	protected abstract void HandleInput();

	public virtual void Start() {
		inputMove = InputSystem.actions.FindAction("Move");

		GameManager.Instance.RegisterController(this);
	}

	public virtual void Update() {
		HandleInput();
	}

	public virtual void OnDestroy() {
		GameManager.Instance.UnregisterController(this);
	}
}