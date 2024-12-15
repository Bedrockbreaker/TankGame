using System;

using AI;

using UnityEngine;
using UnityEngine.InputSystem;

using Util;

/**
 * <summary>
 * A pawn controller receiving inputs from a physical player
 * </summary>
 */
public class PlayerController : Controller {

	protected InputAction inputMove;
	protected InputAction inputShoot;
	protected HUD hud;

	[field: SerializeField]
	public PlayerInput PlayerInput { get; protected set; }

	public override void Start() {
		base.Start();

		inputMove = PlayerInput.actions.FindAction("Move");
		inputShoot = PlayerInput.actions.FindAction("Attack");
		inputShoot.performed += HandleShoot;
	}

	public override void OnDestroy() {
		base.OnDestroy();
		inputShoot.performed -= HandleShoot;
	}

	protected override void HandleInput() {
		if (!PawnOptional) return;
		Vector2 movement = inputMove.ReadValue<Vector2>();
		Pawn.Rotate(movement.x);
		Pawn.Move(movement.y);
	}

	public override bool Possess(Pawn pawn) {
		if (!base.Possess(pawn)) return false;
		UpdateBlackboardPlayerTransform();
		pawn.Health.Then(x => x.OnDeath += OnPawnDeath);
		pawn.Healthbar.Then(x => x.SetBackfaceEnabled(false));
		return true;
	}

	public override bool Unpossess() {
		PawnOptional
			.Then(x => x.Health)
			.Then(x => x.OnDeath -= OnPawnDeath);
		PawnOptional
			.Then(x => x.Healthbar)
			.Then(x => x.SetOriginalBackfaceEnabled());

		if (!base.Unpossess()) return false;
		UpdateBlackboardPlayerTransform();
		return true;
	}

	public void SetHUD(HUD hud) {
		this.hud = hud;
		hud.SetController(this);
	}

	protected void HandleShoot(InputAction.CallbackContext context) {
		PawnOptional.Then(x => x.Shoot());
	}


	protected void UpdateBlackboardPlayerTransform() {
		Blackboard.GLOBAL.Set("PlayerTransform", PawnOptional
			? Pawn.transform
			: Optional<Transform>.None
		);
	}

	protected void OnPawnDeath(Optional<Controller> attacker) {
		RemoveLives(1);

		if (Lives > 0) {
			GameManager.Instance.RespawnPlayer(
				GameManager.Instance.defaultPawnPrefab,
				this,
				Optional<PawnSpawnPoint>.None
			);
		} else {
			PlayerPrefs.SetInt(
				"highscore",
				Math.Max(Score, PlayerPrefs.GetInt("highscore", 0))
			);
			hud.SetGameOver();
		}
	}
}