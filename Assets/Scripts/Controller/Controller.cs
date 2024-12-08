using UnityEngine;

using Util;

public delegate void OnPossessHandler(Pawn pawn);
public delegate void OnUnpossessHandler();

/**
 * <summary>
 * The base class for all pawn controllers
 * </summary>
 */
public abstract class Controller : MonoBehaviour {

	private bool isUpdating = false;
	[ReadOnly]
	[SerializeField]
	private Optional<Pawn> pawn = Optional<Pawn>.None;

	[field: SerializeField]
	public int Score { get; protected set; } = 0;
	[field: SerializeField]
	public int Lives { get; protected set; } = 1;

	public Optional<Pawn> PawnOptional {
		get => pawn;
		protected set {
			if (isUpdating) return;
			isUpdating = true;

			if (pawn) {
				OnUnpossess?.Invoke();
				Pawn.UnbindController();
			}

			pawn = value;

			if (pawn) {
				Pawn.BindController(this);
				OnPossess?.Invoke(Pawn);
			}

			isUpdating = false;
		}
	}
	public Pawn Pawn => pawn.ValueOrDefault();

	public event OnPossessHandler OnPossess;
	public event OnUnpossessHandler OnUnpossess;

	/**
	 * <summary>
	 * Handle decision making every frame
	 * </summary>
	 */
	protected abstract void HandleInput();

	/**
	 * <summary>
	 * Posess the given pawn
	 * </summary>
	 */
	public virtual void Possess(Pawn pawn) {
		PawnOptional = pawn;
	}

	/**
	 * <summary>
	 * Unposess the current pawn
	 * </summary>
	 */
	public virtual void Unpossess() {
		PawnOptional = Optional<Pawn>.None;
	}

	/**
	 * <summary>
	 * Add the given amount of score
	 * </summary>
	 */
	public virtual void AddScore(int amount) {
		Score += amount;
	}

	/**
	 * <summary>
	 * Subtract the given amount of score
	 * </summary>
	 */
	public virtual void RemoveScore(int amount) {
		Score -= amount;
	}

	/**
	 * <summary>
	 * Add the given amount of lives
	 * </summary>
	 */
	public int AddLives(int amount) {
		Lives += amount;
		return Lives;
	}

	/**
	 * <summary>
	 * Remove the given amount of lives
	 * </summary>
	 */
	public int RemoveLives(int amount) {
		Lives -= amount;
		return Lives;
	}

	public virtual void Start() {
		GameManager.Instance.RegisterController(this);
	}

	public virtual void Update() {
		HandleInput();
	}

	public virtual void OnDestroy() {
		Unpossess();
		GameManager.Instance.UnregisterController(this);
	}
}