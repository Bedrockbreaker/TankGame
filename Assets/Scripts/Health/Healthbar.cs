using TMPro;

using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 * Controls the healthbar
 * </summary>
 */
public class Healthbar : MonoBehaviour {

	[SerializeField]
	protected Health health;
	protected Material healthbarMaterial;
	[SerializeField]
	protected Image healthbar1;
	[SerializeField]
	protected Image healthbar2;
	[SerializeField]
	protected TextMeshProUGUI currentHealthText1;
	[SerializeField]
	protected TextMeshProUGUI currentHealthText2;
	[SerializeField]
	protected TextMeshProUGUI maxHealthText1;
	[SerializeField]
	protected TextMeshProUGUI maxHealthText2;
	[SerializeField]
	protected TextMeshProUGUI healthPercentText1;
	[SerializeField]
	protected TextMeshProUGUI healthPercentText2;
	[SerializeField]
	protected TextMeshProUGUI nameText1;
	[SerializeField]
	protected TextMeshProUGUI nameText2;
	[SerializeField]
	protected Image backplate1;
	[SerializeField]
	protected Image backplate2;

	public bool BackfaceEnabled { get; protected set; } = true;

	/**
	 * <summary>
	 * Update the healthbar
	 * </summary>
	 */
	public virtual void OnHealthChanged() {
		healthbarMaterial.SetFloat("_HealthRatio", health.CurrentHealth / health.MaxHealth);

		currentHealthText1.text = health.CurrentHealth.ToString("0");
		currentHealthText2.text = health.CurrentHealth.ToString("0");

		maxHealthText1.text = health.MaxHealth.ToString("0");
		maxHealthText2.text = health.MaxHealth.ToString("0");

		healthPercentText1.text = $"{(int)(health.CurrentHealth / health.MaxHealth * 100)}%";
		healthPercentText2.text = $"{(int)(health.CurrentHealth / health.MaxHealth * 100)}%";
	}

	/**
	 * <summary>
	 * Set the backface visibility
	 * </summary>
	 */
	public virtual void SetBackfaceEnabled(bool enabled) {
		BackfaceEnabled = enabled;

		healthbar2.enabled = enabled;
		currentHealthText2.enabled = enabled;
		maxHealthText2.enabled = enabled;
		healthPercentText2.enabled = enabled;
		nameText2.enabled = enabled;
		backplate2.enabled = enabled;
	}

	public virtual void Start() {
		healthbarMaterial = Instantiate(healthbar1.material);
		healthbar1.material = healthbarMaterial;
		healthbar2.material = healthbarMaterial;

		health.OnHealthChanged += OnHealthChanged;
		OnHealthChanged();
	}
}