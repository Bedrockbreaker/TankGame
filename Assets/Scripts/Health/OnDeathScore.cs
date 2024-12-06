using UnityEngine;

using Util;

/**
 * <summary>
 * Adds/removes score to the attacker when this object dies
 * </summary>
 */
public class OnDeathScore : MonoBehaviour {

	public int scoreRewarded;
	public Health health;

	public void Start() {
		health.OnDeath += OnDeath;
	}

	/**
	 * <summary>
	 * Listener for <see cref="Health.OnDeath"/>
	 * </summary>
	 */
	public void OnDeath(Optional<Controller> attacker) {
		if (!attacker) return;

		if (scoreRewarded > 0) {
			attacker.Value.AddScore(scoreRewarded);
		} else if (scoreRewarded < 0) {
			attacker.Value.RemoveScore(scoreRewarded);
		}
	}
}