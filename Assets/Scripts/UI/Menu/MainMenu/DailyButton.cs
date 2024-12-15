using System;

public class DailyButton : TransitionButton {

	public override void OnRelease() {
		base.OnRelease();

		GameManager.Instance.StartGame(
			(long)new TimeSpan(DateTime.Now.Ticks).TotalDays
		);
	}
}