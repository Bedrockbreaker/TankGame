using UnityEngine;

using Util;

public class OnDeathScore : MonoBehaviour {

	public int scoreRewarded;
	public Health health;

	public void Start() {
		health.OnDeath += OnDeath;
	}

	public void OnDeath(Optional<Controller> attacker) {
		if (!attacker) return;

		if (scoreRewarded > 0) {
			attacker.Value.AddScore(scoreRewarded);
		} else if (scoreRewarded < 0) {
			attacker.Value.RemoveScore(scoreRewarded);
		}
	}
}