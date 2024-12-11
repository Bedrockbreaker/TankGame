using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Util;

/**
 * <summary>
 * Controls the HUD
 * </summary>
 */
public class HUD : MonoBehaviour {

	[field: SerializeField, ReadOnly]
	private Optional<Health> health;

	protected Material healthbarMaterial;

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI livesText;
	public Image healthbar;
	[field: SerializeField]
	public Optional<PlayerController> Controller { get; protected set; }
	public Optional<Health> Health {
		get => health;
		set {
			if (health) health.Value.OnHealthChanged -= OnHealthChanged;
			health = value;
			if (health) health.Value.OnHealthChanged += OnHealthChanged;
			OnHealthChanged();
		}
	}

	public virtual void OnHealthChanged() {
		if (Health) {
			healthbarMaterial.SetFloat(
				"_HealthRatio",
				Health.Value.CurrentHealth / Health.Value.MaxHealth
			);
		} else {
			healthbarMaterial.SetFloat("_HealthRatio", 0f);
		}
	}

	public virtual void OnLivesChanged() {
		if (Controller) {
			livesText.text = new string('♥', Controller.Value.Lives);
		} else {
			livesText.text = "";
		}
	}

	public virtual void OnScoreChanged() {
		if (Controller) {
			scoreText.text = Controller.Value.Score.ToString("0");
		} else {
			scoreText.text = "";
		}
	}

	public virtual void OnPawnChanged(Pawn _) => OnPawnChanged();

	public virtual void OnPawnChanged() {
		OnLivesChanged();
		OnScoreChanged();

		Health = Controller.Value.PawnOptional
			? Controller.Value.Pawn.Health
			: Optional<Health>.None;

		OnHealthChanged();
	}

	public virtual void SetController(Optional<PlayerController> controller) {
		if (Controller) {
			Controller.Value.OnPossess -= OnPawnChanged;
			Controller.Value.OnUnpossess -= OnPawnChanged;
			Controller.Value.OnLivesChanged -= OnLivesChanged;
			Controller.Value.OnScoreChanged -= OnScoreChanged;
		}

		Controller = controller;

		if (Controller) {
			Controller.Value.OnPossess += OnPawnChanged;
			Controller.Value.OnUnpossess += OnPawnChanged;
			Controller.Value.OnLivesChanged += OnLivesChanged;
			Controller.Value.OnScoreChanged += OnScoreChanged;

			Health = Controller.Value.PawnOptional
				? Controller.Value.Pawn.Health
				: Optional<Health>.None;
		}
		OnPawnChanged();
	}

	public virtual void Start() {
		healthbarMaterial = Instantiate(healthbar.material);
		healthbar.material = healthbarMaterial;
	}
}