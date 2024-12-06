using UnityEditor;

using UnityEngine;

using Util;

/**
 * <summary>
 * Inspector drawer for ReadOnly fields
 * </summary>
 */
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer {
	public override void OnGUI(
		Rect position,
		SerializedProperty property,
		GUIContent label
	) {
		bool previousGUIEnabled = GUI.enabled;
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = previousGUIEnabled;
	}
}