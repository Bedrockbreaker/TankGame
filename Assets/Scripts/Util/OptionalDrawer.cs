using UnityEditor;

using UnityEngine;

using Util;

/**
 * <summary>
 * Inspector drawer for Optionals
 * </summary>
 */
[CustomPropertyDrawer(typeof(Optional<>))]
public class OptionalDrawer : PropertyDrawer {
	public override void OnGUI(
		Rect position,
		SerializedProperty property,
		GUIContent label
	) {
		SerializedProperty value = property.FindPropertyRelative("value");
		EditorGUI.PropertyField(position, value, label, true);
	}

	public override float GetPropertyHeight(
		SerializedProperty property,
		GUIContent label
	) {
		SerializedProperty value = property.FindPropertyRelative("value");
		return EditorGUI.GetPropertyHeight(value);
	}
}