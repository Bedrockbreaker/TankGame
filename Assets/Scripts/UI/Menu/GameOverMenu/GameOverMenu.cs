using TMPro;

public class GameOverMenu : Menu {

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI highscoreText;
	public TextMeshProUGUI seedText;

	public void SetText(int score, int highscore, long seed) {
		scoreText.text = score.ToString("0");
		highscoreText.text = highscore.ToString("0");
		seedText.text = seed.ToString("0");
	}
}