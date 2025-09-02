#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomPropertyDrawer(typeof(SoundToPlay))]
public class SoundToPlayDrawer : PropertyDrawer
{
	private static Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var typeDefProp = property.FindPropertyRelative("typeDefinition");
		string soundName = typeDefProp.objectReferenceValue != null ?
			typeDefProp.objectReferenceValue.name : "NONE";

		string key = property.propertyPath;

		Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

		
		if (!foldoutStates.ContainsKey(key))
			foldoutStates[key] = true;


		foldoutStates[key] = EditorGUI.Foldout(
			foldoutRect,
			foldoutStates[key],
			GUIContent.none, // sem texto
			true
		);

		EditorGUI.LabelField(foldoutRect, $"Sound: {soundName}", EditorStyles.boldLabel);
				

		if (!foldoutStates[key])
		{
			return;
		}

		EditorGUI.BeginProperty(position, label, property);


		Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		// Search properties
		var clipProp = property.FindPropertyRelative("_clip");
		var clipsProp = property.FindPropertyRelative("_clips");
		var randomClipProp = property.FindPropertyRelative("shouldRandomizeClip");
		var randomPitchProp = property.FindPropertyRelative("shouldRandomizePitch");
		var minPitchProp = property.FindPropertyRelative("_minPitch");
		var maxPitchProp = property.FindPropertyRelative("_maxPitch");
		var shouldRandomizeVolume = property.FindPropertyRelative("shouldRandomizeVolume");
		var minVolumeProp = property.FindPropertyRelative("_minVolume");
		var maxVolumeProp = property.FindPropertyRelative("_maxVolume");
		var defaultVolume = property.FindPropertyRelative("_defaultVolume");
		var shouldLoop = property.FindPropertyRelative("shouldLoop");
		var shouldOverrideSourceSettings = property.FindPropertyRelative("shouldOverrideSourceSettings");
		var shouldUseAudioMixer = property.FindPropertyRelative("shouldUseAudioMixer");
		var mixer = property.FindPropertyRelative("_mixer");

		var priority = property.FindPropertyRelative("_priority");
		var _stereoPan = property.FindPropertyRelative("_stereoPan");
		var _spatialBlend = property.FindPropertyRelative("_spatialBlend");
		var _reverbZoneMix = property.FindPropertyRelative("_reverbZoneMix");
		var _dopplerLevel = property.FindPropertyRelative("_dopplerLevel");
		var _spread = property.FindPropertyRelative("_spread");
		var _minDistance = property.FindPropertyRelative("_minDistance");
		var _maxDistance = property.FindPropertyRelative("_maxDistance");
		var _volumeRolloff = property.FindPropertyRelative("_volumeRolloff");

		GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
		boldStyle.fontStyle = FontStyle.Bold;
		boldStyle.fontSize = 14;

		float fieldHeight = EditorGUI.GetPropertyHeight(typeDefProp);
	
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), typeDefProp, new GUIContent("SFX Type"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		// Random Clip toggle
		fieldHeight = EditorGUI.GetPropertyHeight(randomClipProp);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), randomClipProp, new GUIContent("Randomize Clip"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		if (randomClipProp.boolValue)
		{
			fieldHeight = EditorGUI.GetPropertyHeight(clipsProp, true);
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), clipsProp, new GUIContent("Clips"), true);
			rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
		}
		else
		{
			fieldHeight = EditorGUI.GetPropertyHeight(clipProp);
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), clipProp, new GUIContent("Clip"));
			rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		// Volume
		fieldHeight = EditorGUI.GetPropertyHeight(shouldRandomizeVolume);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), shouldRandomizeVolume, new GUIContent("Randomize Volume"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		if (shouldRandomizeVolume.boolValue)
		{
			EditorGUI.LabelField(rect, "Volume Range");
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			float minVol = minVolumeProp.floatValue;
			float maxVol = maxVolumeProp.floatValue;
			EditorGUI.MinMaxSlider(rect, ref minVol, ref maxVol, 0f, 1f);
			minVolumeProp.floatValue = minVol;
			maxVolumeProp.floatValue = maxVol;
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(rect, $"Min: {minVol:F2}, Max: {maxVol:F2}");
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}
		else
		{
			fieldHeight = EditorGUI.GetPropertyHeight(defaultVolume);
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), defaultVolume, new GUIContent("Volume"));
			rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		// Pitch
		fieldHeight = EditorGUI.GetPropertyHeight(randomPitchProp);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), randomPitchProp, new GUIContent("Randomize Pitch"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		if (randomPitchProp.boolValue)
		{
			EditorGUI.LabelField(rect, "Pitch Range");
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			float minPitch = minPitchProp.floatValue;
			float maxPitch = maxPitchProp.floatValue;
			EditorGUI.MinMaxSlider(rect, ref minPitch, ref maxPitch, 0.1f, 3f);
			minPitchProp.floatValue = minPitch;
			maxPitchProp.floatValue = maxPitch;
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(rect, $"Min: {minPitch:F2}, Max: {maxPitch:F2}");
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		// Override AudioSource
		fieldHeight = EditorGUI.GetPropertyHeight(shouldOverrideSourceSettings);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), shouldOverrideSourceSettings, new GUIContent("Override Audio Source Settings"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		if (shouldOverrideSourceSettings.boolValue)
		{
			DrawSliderWithLabel(ref rect, "Priority", priority, 0, 256);
			DrawSliderWithLabel(ref rect, "Stereo Pan", _stereoPan, -1f, 1f);
			DrawSliderWithLabel(ref rect, "Spatial Blend", _spatialBlend, 0f, 1f);
			DrawSliderWithLabel(ref rect, "Reverb Zone Mix", _reverbZoneMix, 0f, 1.1f);
			DrawSliderWithLabel(ref rect, "Doppler Level", _dopplerLevel, 0f, 5f);
			DrawSliderWithLabel(ref rect, "Spread", _spread, 0f, 360f);

			SerializedProperty rolloffMode = _volumeRolloff;
			rolloffMode.enumValueIndex = (int)(AudioRolloffMode)EditorGUI.EnumPopup(rect, "Rolloff Mode", (AudioRolloffMode)rolloffMode.enumValueIndex);
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			DrawSliderWithLabel(ref rect, "Min Distance", _minDistance, 0, 30000);
			DrawSliderWithLabel(ref rect, "Max Distance", _maxDistance, 0, 30000);
		}

		// Mixer
		fieldHeight = EditorGUI.GetPropertyHeight(shouldUseAudioMixer);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), shouldUseAudioMixer, new GUIContent("Use Audio Mixer"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		if (shouldUseAudioMixer.boolValue)
		{
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), mixer);
			rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		// Loop
		fieldHeight = EditorGUI.GetPropertyHeight(shouldLoop);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, fieldHeight), shouldLoop, new GUIContent("Should Loop"));
		rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

		EditorGUI.EndProperty();
	}


	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		string key = property.propertyPath;
		if (foldoutStates.TryGetValue(key, out bool isExpanded) && !isExpanded)
		{
			// Min height 
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		float height = 0f;

		height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2; // soundName
		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("typeDefinition")) + EditorGUIUtility.standardVerticalSpacing;
		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("shouldRandomizeClip")) + EditorGUIUtility.standardVerticalSpacing;

		if (property.FindPropertyRelative("shouldRandomizeClip").boolValue)
		{
			height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_clips"), true) + EditorGUIUtility.standardVerticalSpacing;
		}
		else
		{
			height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_clip")) + EditorGUIUtility.standardVerticalSpacing;
		}

		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("shouldRandomizeVolume")) + EditorGUIUtility.standardVerticalSpacing;

		if (property.FindPropertyRelative("shouldRandomizeVolume").boolValue)
		{
			height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3;
		}
		else
		{
			height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_defaultVolume")) + EditorGUIUtility.standardVerticalSpacing;
		}

		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("shouldRandomizePitch")) + EditorGUIUtility.standardVerticalSpacing;

		if (property.FindPropertyRelative("shouldRandomizePitch").boolValue)
		{
			height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3;
		}

		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("shouldOverrideSourceSettings")) + EditorGUIUtility.standardVerticalSpacing;

		if (property.FindPropertyRelative("shouldOverrideSourceSettings").boolValue)
		{
			height += EditorGUIUtility.singleLineHeight * 9 + EditorGUIUtility.standardVerticalSpacing * 9;
		}

		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("shouldUseAudioMixer")) + EditorGUIUtility.standardVerticalSpacing;

		if (property.FindPropertyRelative("shouldUseAudioMixer").boolValue)
		{
			height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_mixer")) + EditorGUIUtility.standardVerticalSpacing;
		}

		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("shouldLoop")) + EditorGUIUtility.standardVerticalSpacing;

		return height;
	}
	private int GetIndexFromPath(string propertyPath)
	{
		int start = propertyPath.IndexOf("[") + 1;
		int end = propertyPath.IndexOf("]");
		if (start > 0 && end > start)
		{
			string indexStr = propertyPath.Substring(start, end - start);
			if (int.TryParse(indexStr, out int index))
			{
				return index;
			}
		}
		return 0;
	}
	private void DrawSliderWithLabel(ref Rect rect, string label, SerializedProperty property, float min, float max)
	{
		float labelWidth = 100f;
		float spacing = 5f;

		EditorGUI.LabelField(
			new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight),
			label
		);

		Rect sliderRect = new Rect(
			rect.x + labelWidth + spacing,
			rect.y,
			rect.width - labelWidth - spacing,
			EditorGUIUtility.singleLineHeight
		);

		if (property.propertyType == SerializedPropertyType.Float)
		{
			property.floatValue = EditorGUI.Slider(sliderRect, GUIContent.none, property.floatValue, min, max);
		}
		else if (property.propertyType == SerializedPropertyType.Integer)
		{
			float sliderValue = EditorGUI.Slider(sliderRect, GUIContent.none, property.intValue, min, max);
			property.intValue = Mathf.RoundToInt(sliderValue);
		}
		else
		{
			EditorGUI.LabelField(rect, "Unsupported property type");
		}

		rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
	}
}

#endif
