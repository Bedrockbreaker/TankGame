using UnityEngine;

/**
 * <summary>
 * The base class for all buttons
 * </summary>
 */
public abstract class Button : MonoBehaviour {

	public AudioClip clipPressed;
	public AudioClip clipReleased;

	/**
	 * <summary>
	 * Called when the button is clicked
	 * </summary>
	 */
	public virtual void OnClick() {
		GameManager.Instance.PlayOneShot(clipPressed);
	}

	/**
	 * <summary>
	 * Called when the button is released
	 * </summary>
	 */
	public virtual void OnRelease() {
		GameManager.Instance.PlayOneShot(clipReleased);
	}
}