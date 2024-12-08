using AI;

using TMPro;

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

	public Optional<TextMeshProUGUI> livesText;
	public Optional<TextMeshProUGUI> scoreText;

	public override int Lives {
		protected set {
			base.Lives = value;

			if (!livesText) return;
			livesText.Value.text = new string('♥', Lives);
		}
	}
	public override int Score {
		protected set {
			base.Score = value;

			if (!scoreText) return;
			scoreText.Value.text = Score.ToString();
		}
	}

	public override void Start() {
		base.Start();

		inputMove = InputSystem.actions.FindAction("Move");
		inputShoot = InputSystem.actions.FindAction("Attack");
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

	public override void Possess(Pawn pawn) {
		base.Possess(pawn);
		UpdateBlackboardPlayerTransform();
	}

	public override void Unpossess() {
		base.Unpossess();
		UpdateBlackboardPlayerTransform();
	}

	protected void HandleShoot(InputAction.CallbackContext context) {
		if (!PawnOptional) return;
		Pawn.Shoot();
	}

	protected void UpdateBlackboardPlayerTransform() {
		Blackboard.GLOBAL.Set("PlayerTransform", PawnOptional
			? Pawn.transform
			: Optional<Transform>.None
		);
	}
}