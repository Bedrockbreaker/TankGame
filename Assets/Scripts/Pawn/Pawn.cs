using System;

using UnityEngine;

using Util;

public delegate void OnControllerBoundHandler(Controller controller);
public delegate void OnControllerUnboundHandler();

/**
 * <summary>
 * The base class for all pawns
 * </summary>
 */
public abstract class Pawn : MonoBehaviour {

	private bool isUpdating = false;
	[ReadOnly]
	[SerializeField]
	private Optional<Controller> controller = Optional<Controller>.None;
	private bool originalHealthbarBackfaceEnabled;

	[SerializeField]
	protected Optional<Shooter> shooter;
	[SerializeField]
	protected Optional<Healthbar> healthbar;
	protected Optional<Camera> attachedCamera;

	[field: SerializeField]
	public BasicMovement Movement { get; protected set; }
	[field: SerializeField]
	public StatusEffectManager StatusEffectManager { get; protected set; }
	[Type(typeof(Controller))]
	public string autoPossessByController;

	public Optional<Controller> ControllerOptional {
		get => controller;
		protected set {
			if (isUpdating) return;
			isUpdating = true;

			if (controller) {
				OnCountrollerUnbound?.Invoke();
				Controller.Unpossess();
			}

			controller = value;

			if (controller) {
				Controller.Possess(this);
				OnControllerBound?.Invoke(Controller);
			}

			isUpdating = false;
		}
	}
	public Controller Controller => controller.ValueOrDefault();

	public Transform cameraTransform;
	public event OnControllerBoundHandler OnControllerBound;
	public event OnControllerUnboundHandler OnCountrollerUnbound;

	/**
	 * <summary>
	 * Bind a controller
	 * </summary>
	 * <param name="controller"></param>
	 */
	public virtual void BindController(Controller controller) {
		ControllerOptional = controller;

		if (shooter) shooter.Value.owner = controller;
		if (controller is PlayerController && healthbar) {
			originalHealthbarBackfaceEnabled = healthbar.Value.BackfaceEnabled;
			healthbar.Value.SetBackfaceEnabled(false);
		}
	}

	/**
	 * <summary>
	 * Unbind the controller
	 * </summary>
	 */
	public virtual void UnbindController() {
		if (Controller is PlayerController && healthbar) {
			healthbar.Value.SetBackfaceEnabled(originalHealthbarBackfaceEnabled);
		}

		ControllerOptional = Optional<Controller>.None;

		if (shooter) shooter.Value.owner = Optional<Controller>.None;
	}

	/**
	 * <summary>
	 * Called after the camera is attached
	 * </summary>
	 */
	public virtual void AttachCamera(Camera camera) {
		attachedCamera = camera;
	}

	/**
	 * <summary>
	 * Move along the pawn's relative forward axis
	 * </summary>
	 */
	public abstract void Move(float distance);

	/**
	 * <summary>
	 * Move to a given position
	 * </summary>
	 */
	public virtual void MoveTo(Vector3 position) {
		Movement.MoveTo(position);
	}

	/**
	 * <summary>
	 * Cancel a previous call to MoveTo.
	 * </summary>
	 */
	public virtual void CancelMove() {
		Movement.CancelMove();
	}

	/**
	 * <summary>
	 * Rotate counterclockwise around the the pawn's relative up axis
	 * </summary>
	 */
	public virtual void Rotate(float radians) {
		Movement.Rotate(radians);
	}

	/**
	 * <summary>
	 * Rotate to focus on a given position
	 * </summary>
	 */
	public virtual void Focus(Vector3 position) {
		Movement.Focus(position);
	}

	/**
	 * <summary>
	 * Rotate to focus on a given transform
	 * </summary>
	 */
	public virtual void Focus(Transform transform) {
		Movement.Focus(transform);
	}

	/**
	 * <summary>
	 * Clear the current focus
	 * </summary>
	 */
	public virtual void ClearFocus() {
		Movement.ClearFocus();
	}

	/**
	 * <summary>
	 * Shoot a projectile
	 * </summary>
	 */
	public virtual bool Shoot() {
		if (!shooter) return false;
		return shooter.Value.Shoot();
	}

	public virtual void Start() {
		GameManager.Instance.RegisterPawn(this);

		if (ControllerOptional) return;
		if (string.IsNullOrEmpty(autoPossessByController)) return;

		Type controllerType = Type.GetType(autoPossessByController);
		if (controllerType == null) return;
		Controller controller = (Controller)gameObject.AddComponent(controllerType);
		GameManager.Instance.RegisterController(controller);

		BindController(controller);
	}

	public virtual void OnDestroy() {
		if (ControllerOptional) Controller.RemoveLives(1);
		UnbindController();
		if (attachedCamera) attachedCamera.Value.transform.parent = null;
		GameManager.Instance.UnregisterPawn(this);
	}
}