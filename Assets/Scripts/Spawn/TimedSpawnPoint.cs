using UnityEngine;

using Util;

/**
 * <summary>
 * Represents a spawn point that repeatedly spawns an object
 * </summary>
 */
public class TimedSpawnPoint : MonoBehaviour {

	[SerializeField]
	protected bool isActive = true;
	protected float spawnTimer = 0f;
	protected Optional<GameObject> spawnedObject = Optional<GameObject>.None;

	public GameObject prefab;
	public float spawnIntervalSeconds = 1f;
	public float spawnDelaySeconds = 0f;
	public bool IsActive {
		get => isActive;
		set {
			isActive = value;
			spawnTimer = spawnIntervalSeconds + spawnDelaySeconds;
		}
	}

	public virtual void Start() {
		IsActive = isActive;
	}

	public virtual void Update() {
		if (spawnedObject) return;

		if (spawnTimer > 0f) {
			spawnTimer -= Time.deltaTime;
		} else {
			Spawn();
		}
	}

	public virtual bool Spawn() {
		if (!IsActive) return false;
		if (spawnTimer > 0f) return false;
		if (spawnedObject) return false;

		spawnedObject = Instantiate(prefab, transform.position, transform.rotation);

		spawnTimer = spawnIntervalSeconds;

		return true;
	}
}