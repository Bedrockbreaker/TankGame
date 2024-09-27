using UnityEngine;

/**
 * <summary>
 * A pawn controller receiving inputs from a physical player
 * </summary>
 */
public class PlayerController : Controller {

	protected override void HandleInput() {
		Vector2 movement = inputMove.ReadValue<Vector2>();
		pawn.Rotate(movement.x);
		pawn.Move(movement.y);
	}
}