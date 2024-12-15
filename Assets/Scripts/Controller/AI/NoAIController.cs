using UnityEngine;

using Util;

/**
 * <summary>
 * An AI controller with no behavior and no physics
 * </summary>
 */
public class NoAIController : AIController {

	protected bool originalIsKinematic;

	protected override void HandleInput() { }

	public override bool Possess(Pawn pawn) {
		if (!base.Possess(pawn)) return false;

		Optional<Rigidbody> rigidbody = pawn.GetComponent<Rigidbody>();
		if (rigidbody) {
			originalIsKinematic = rigidbody.Value.isKinematic;
			rigidbody.Value.isKinematic = true;
		}

		return true;
	}

	public override bool Unpossess() {
		if (PawnOptional) {
			Optional<Rigidbody> rigidbody = Pawn.GetComponent<Rigidbody>();
			if (rigidbody) {
				rigidbody.Value.isKinematic = originalIsKinematic;
			}
		}

		return base.Unpossess();
	}
}