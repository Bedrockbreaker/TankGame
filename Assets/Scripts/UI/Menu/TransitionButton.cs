public class TransitionButton : Button {

	public Menu menu;

	public override void OnClick() {
		base.OnClick();
	}

	public override void OnRelease() {
		base.OnRelease();
		GameManager.Instance.SetMenuActive(menu);
	}
}