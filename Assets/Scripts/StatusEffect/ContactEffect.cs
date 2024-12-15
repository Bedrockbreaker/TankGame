using System;

using UnityEngine;

using Util;

public class ContactEffect : ContactListener {

	public float duration = 5f;
	public float strength = 1f;
	// [TemplatedType(typeof(StatusEffect), nameof(effectTemplate))]
	[Type(typeof(StatusEffect), false)]
	public string effectType;
	// [HideInInspector]
	// public Template effectTemplate = new();
	public AudioClip pickupSound;

	public void Start() {
		OnContact += Apply;
	}

	public void Apply(Collider other) {
		if (other.TryGetComponent<StatusEffectManager>(out var statusEffectManager)) {
			GameManager.Instance.PlayOneShot(pickupSound);

			// StatusEffect effect = effectTemplate.CreateInstance() as StatusEffect;
			StatusEffect effect = Activator.CreateInstance(
				Type.GetType(effectType),
				duration,
				strength,
				Optional<Controller>.None
			) as StatusEffect;

			statusEffectManager.Apply(effect);
		}

		Destroy(gameObject);
	}
}