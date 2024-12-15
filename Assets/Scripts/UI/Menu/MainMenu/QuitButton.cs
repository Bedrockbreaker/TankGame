using UnityEngine;

public class QuitButton : Button {

	public override void OnRelease() {
		Application.Quit();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}