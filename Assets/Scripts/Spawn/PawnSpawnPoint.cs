using UnityEngine;

/**
 * <summary>
 * Represents a valid spawn (position and rotation) for any pawn/controller
 * </summary>
 */
public class PawnSpawnPoint : MonoBehaviour {

	protected void Awake() {
		GameManager.Instance.RegisterSpawnPoint(gameObject);
	}

	protected void OnDestroy() {
		GameManager.Instance.UnregisterSpawnPoint(gameObject);
	}
}