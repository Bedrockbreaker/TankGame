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