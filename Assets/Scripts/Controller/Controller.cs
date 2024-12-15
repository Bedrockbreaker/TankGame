using System;

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
	[SerializeField]
	private int lives = 3;
	[SerializeField]
	private int score;

	[field: SerializeField]
	public string Name { get; protected set; }
	public virtual int Score {
		get => score;
		protected set {
			score = value;
			OnScoreChanged?.Invoke();
		}
	}
	public virtual int Lives {
		get => lives;
		protected set {
			lives = value;
			OnLivesChanged?.Invoke();
		}
	}
	[field: SerializeField]
	public int ScoreToLifeRatio { get; protected set; } = 1000;
	[field: SerializeField]
	public Optional<Camera> AttachedCamera { get; protected set; }

	public Optional<Pawn> PawnOptional {
		get => pawn;
		protected set {
			if (isUpdating) return;
			isUpdating = true;

			pawn.Then(x => {
				OnUnpossess?.Invoke();
				x.UnbindController();
			});

			pawn = value;

			pawn.Then(x => {
				x.BindController(this);
				OnPossess?.Invoke(x);
			});

			isUpdating = false;
		}
	}
	public Pawn Pawn => pawn.ValueOrDefault();

	public event OnPossessHandler OnPossess;
	public event OnUnpossessHandler OnUnpossess;
	public event Action OnLivesChanged;
	public event Action OnScoreChanged;

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
	public virtual bool Possess(Pawn pawn) {
		if (isUpdating) return false;
		PawnOptional = pawn;
		AttachedCamera.Then(x => pawn.AttachCamera(x));
		return true;
	}

	/**
	 * <summary>
	 * Unposess the current pawn
	 * </summary>
	 */
	public virtual bool Unpossess() {
		if (isUpdating) return false;
		PawnOptional.Then(x => x.DetachCamera());
		PawnOptional = Optional<Pawn>.None;
		return true;
	}

	/**
	 * <summary>
	 * Add the given amount of score.<br/>
	 * Automatically add lives if the score passes a multiple of ScoreToLifeRatio
	 * </summary>
	 */
	public virtual int AddScore(int amount) {
		int delta = (Score % ScoreToLifeRatio) + amount;
		Score += amount;
		if (delta >= ScoreToLifeRatio) AddLives(delta / ScoreToLifeRatio);
		return Score;
	}

	/**
	 * <summary>
	 * Subtract the given amount of score
	 * </summary>
	 */
	public virtual int RemoveScore(int amount) {
		Score -= amount;
		return Score;
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

	/**
	 * <summary>
	 * Bind the given camera to this controller
	 * </summary>
	 */
	public virtual void BindCamera(Camera camera) {
		AttachedCamera = camera;
		PawnOptional.Then(x => x.AttachCamera(camera));
	}

	/**
	 * <summary>
	 * Unbind the camera from this controller
	 * </summary>
	 */
	public virtual void UnbindCamera() {
		PawnOptional.Then(x => x.DetachCamera());
		AttachedCamera = Optional<Camera>.None;
	}

	public virtual void Start() {
		GameManager.Instance.RegisterController(this);

		Score = score;
		Lives = lives;
	}

	public virtual void Update() {
		HandleInput();
	}

	public virtual void OnDestroy() {
		Unpossess();
		GameManager.Instance.UnregisterController(this);
	}
}