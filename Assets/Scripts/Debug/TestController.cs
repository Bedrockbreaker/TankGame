using UnityEngine;

using Util;

/**
 * <summary>
 * An AI controller for testing
 * </summary>
 */
public class TestController : NoAIController {

	public Health other;

	public override void Start() {
		base.Start();

		other.OnDeath += OnOtherDeath;
	}

	public override void OnDestroy() {
		base.OnDestroy();
		other.OnDeath -= OnOtherDeath;
	}

	protected void OnOtherDeath(Optional<Controller> attacker) {
		if (attacker) {
			Debug.Log($"{attacker.Value.name} killed {other.name}");
		} else {
			Debug.Log($"{other.name} died");
		}
	}
}