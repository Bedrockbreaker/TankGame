public class TitleScreen : Menu {

	protected static bool firstOpen = true;

	public Menu mainMenu;

	public override void Awake() {
		base.Awake();
		GameManager.Instance.SetMenuActive(firstOpen ? this : mainMenu);
		firstOpen = false;
	}
}