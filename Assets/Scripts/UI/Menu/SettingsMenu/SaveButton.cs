public class SaveButton : TransitionButton {

	public AbstractSlider mainSlider;
	public AbstractSlider musicSlider;
	public AbstractSlider effectSlider;

	public void OnRelease(bool apply) {
		base.OnRelease();

		mainSlider.ExitSettings(apply);
		musicSlider.ExitSettings(apply);
		effectSlider.ExitSettings(apply);
	}
}