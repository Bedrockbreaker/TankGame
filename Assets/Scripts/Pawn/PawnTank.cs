using UnityEngine;

using Util;

/**
 * <summary>
 * A tank pawn
 * </summary>
 */
public class PawnTank : Pawn {

	public AudioClip deathSound;

	public override void AttachCamera(Camera camera) {
		base.AttachCamera(camera);
		// TODO: don't directly attach the camera
		// Instead, create a spring arm with smooth movement
		// which allows disconnected rotation from local forwards
		camera.transform.parent = transform;
		camera.transform.SetPositionAndRotation(
			cameraTransform.position,
			cameraTransform.rotation
		);
	}

	public override void DetachCamera() {
		AttachedCamera
			.Where(x => x.transform.parent == transform)
			.Then(x => x.transform.parent = null);
		base.DetachCamera();
	}

	public override void Move(float distance) {
		Movement.Move(transform.forward, distance);
	}

	public override void OnDeath(Optional<Controller> attacker) {
		base.OnDeath(attacker);
		GameManager.Instance.PlayOneShot(deathSound);
	}
}