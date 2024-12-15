using System;
using System.Security.Cryptography;
using System.Text;

using TMPro;

public class PlayButton : TransitionButton {

	public TMP_InputField seedInput;

	public void OnRelease(bool multiplayer) {
		base.OnRelease();

		string seedText = string.IsNullOrEmpty(seedInput.text) ? "0" : seedInput.text;
		if (!long.TryParse(seedText, out long seed)) {
			using SHA256 sha256 = SHA256.Create();

			byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(seedText));
			seed = BitConverter.ToInt64(bytes, 0);
		}

		GameManager.Instance.StartGame(seed, multiplayer);
	}
}