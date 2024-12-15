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

	[SerializeField]
	protected Optional<Shooter> shooter;

	[field: SerializeField]
	public string Name { get; protected set; }
	[field: SerializeField]
	public Optional<Health> Health { get; protected set; }
	[field: SerializeField]
	public Optional<Healthbar> Healthbar { get; protected set; }
	[field: SerializeField]
	public BasicMovement Movement { get; protected set; }
	[field: SerializeField]
	public StatusEffectManager StatusEffectManager { get; protected set; }
	[Type(typeof(Controller))]
	public string autoPossessByController;
	public Optional<Camera> AttachedCamera { get; protected set; }

	public Optional<Controller> ControllerOptional {
		get => controller;
		protected set {
			if (isUpdating) return;
			isUpdating = true;

			controller.Then(x => {
				OnCountrollerUnbound?.Invoke();
				x.Unpossess();
			});

			controller = value;

			controller.Then(x => {
				x.Possess(this);
				OnControllerBound?.Invoke(x);
			});

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
		shooter.Then(x => x.owner = controller);
	}

	/**
	 * <summary>
	 * Unbind the controller
	 * </summary>
	 */
	public virtual void UnbindController() {
		ControllerOptional = Optional<Controller>.None;
		shooter.Then(x => x.owner = Optional<Controller>.None);
	}

	/**
	 * <summary>
	 * Called after the camera is attached
	 * </summary>
	 */
	public virtual void AttachCamera(Camera camera) {
		AttachedCamera = camera;
	}

	/**
	 * <summary>
	 * Detach the camera
	 * </summary>
	 */
	public virtual void DetachCamera() {
		AttachedCamera = Optional<Camera>.None;
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

	/**
	 * <summary>
	 * Called when the health component for this pawn emits a death event
	 * </summary>
	 */
	public virtual void OnDeath(Optional<Controller> attacker) {
		Health.Value.OnDeath -= OnDeath;
		DetachCamera();
	}

	public virtual void Start() {
		GameManager.Instance.RegisterPawn(this);

		Health.Then(x => x.OnDeath += OnDeath);

		if (ControllerOptional) return;
		if (string.IsNullOrEmpty(autoPossessByController)) return;

		Type controllerType = Type.GetType(autoPossessByController);
		if (controllerType == null) return;
		Controller controller = (Controller)gameObject.AddComponent(controllerType);
		GameManager.Instance.RegisterController(controller);

		BindController(controller);
	}

	public virtual void OnDestroy() {
		UnbindController();
		GameManager.Instance.UnregisterPawn(this);
	}
}