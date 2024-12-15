using UnityEngine.SceneManagement;

public class RestartButton : Button {

	public override void OnRelease() {
		base.OnRelease();

		SceneManager.LoadScene(
			SceneManager.GetActiveScene().name,
			LoadSceneMode.Single
		);
	}
}