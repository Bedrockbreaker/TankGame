using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public abstract class AbstractSlider : MonoBehaviour {

	[field: SerializeField]
	public Slider Slider { get; protected set; }
	public AudioMixer audioMixer;
	public string volumeParameter;
	public float SavedVolume { get; protected set; }

	public virtual void Start() {
		SavedVolume = PlayerPrefs.GetFloat(volumeParameter, 1f);
		Slider.value = SavedVolume;
		GameManager.Instance.SetMixerAttenuation(volumeParameter, SavedVolume);
	}

	public virtual void OnValueChanged() {
		GameManager.Instance.SetMixerAttenuation(volumeParameter, Slider.value);
	}

	public virtual void OnEndDrag() { }

	public virtual void ExitSettings(bool apply) {
		if (apply) {
			SavedVolume = Slider.value;
			PlayerPrefs.SetFloat(volumeParameter, SavedVolume);
		} else {
			Slider.value = SavedVolume;
		}

		GameManager.Instance.SetMixerAttenuation(volumeParameter, Slider.value);
	}
}