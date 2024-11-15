using UnityEngine;

/**
 * <summary>
 * Represents a valid spawn (position and rotation) for any pawn/controller
 * </summary>
 */
public class PawnSpawnPoint : MonoBehaviour {

	private void Start() {
		GameManager.Instance.RegisterSpawnPoint(gameObject);
	}

	private void OnDestroy() {
		GameManager.Instance.UnregisterSpawnPoint(gameObject);
	}
}