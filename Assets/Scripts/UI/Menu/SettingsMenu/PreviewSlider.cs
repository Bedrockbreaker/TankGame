using UnityEngine;

public class PreviewSlider : AbstractSlider {

	public AudioClip referenceClip;

	public override void OnValueChanged() { }

	public override void OnEndDrag() {
		GameManager.Instance.SetMixerAttenuation(volumeParameter, Slider.value);
		GameManager.Instance.PlayOneShot(referenceClip, 1f);
	}
}