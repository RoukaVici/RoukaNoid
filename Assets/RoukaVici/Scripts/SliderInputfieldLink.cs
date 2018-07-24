using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInputfieldLink : MonoBehaviour {
	[SerializeField]
	private motorID id;

	[SerializeField]
	private InputField inputFieldIntensity;

	[SerializeField]
	private Slider sliderIntensity;

	[SerializeField]
	private PatternEditorData editor;

	public float intensity;

	void Start() {
		if (inputFieldIntensity && sliderIntensity) {
			inputFieldIntensity.onValueChanged.AddListener(delegate {InputFieldUpdate();});
			sliderIntensity.onValueChanged.AddListener(delegate {SliderUpdate();});
		}
	}

	private void SliderUpdate() {
		inputFieldIntensity.text = sliderIntensity.value.ToString();
		intensity = sliderIntensity.value;
		if (editor)
			editor.pattern.fingers[(int)id].pattern[editor.getIteration()] = (int)intensity;
	}

	private void InputFieldUpdate() {
		int value = 0;
		if (inputFieldIntensity.text.Length > 0 && inputFieldIntensity.text.ToCharArray()[0] == '-')
			inputFieldIntensity.text = "0";
		else if (inputFieldIntensity.text.Length > 0)
			value = int.Parse(inputFieldIntensity.text);
		sliderIntensity.value = Mathf.Clamp(value, 0, 100);
		intensity = sliderIntensity.value;
		if (editor)
			editor.pattern.fingers[(int)id].pattern[editor.getIteration()] = (int)intensity;
	}

	public void setValues(float newIntensity)
	{
		newIntensity = Mathf.Clamp(newIntensity, 0, 100);
		inputFieldIntensity.text = newIntensity.ToString();
		sliderIntensity.value = newIntensity;
	}
}
