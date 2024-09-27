using UnityEngine;

/**
 * <summary>
 * Represents a valid spawn (position and rotation) for any pawn/controller
 * </summary>
 */
public class SpawnPoint : MonoBehaviour {

	private void Awake() {
		GameManager.Instance.RegisterSpawnPoint(gameObject);
	}

	private void OnDestroy() {
		GameManager.Instance.UnregisterSpawnPoint(gameObject);
	}
}