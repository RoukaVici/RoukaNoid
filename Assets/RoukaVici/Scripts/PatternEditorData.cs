using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper != null ? wrapper.fingers : null;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.fingers = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.fingers = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] fingers;
    }
	
}

public class PatternEditorData : MonoBehaviour
{
	public int currentPatternID = 0;

	[SerializeField]
	private Dropdown dropdownIteration;

	[SerializeField]
	private InputField InputFieldPatternName;

	[SerializeField]
	private InputField InputFieldPatternDelay;

	[SerializeField]
	private SliderInputfieldLink[] fingers = new SliderInputfieldLink[(int)motorID.LAST_MOTOR];

	[SerializeField]
	public VibrationStyle pattern;

	public void prepareEditor(int editID)
	{
		dropdownIteration.value = 0;
		pattern = new VibrationStyle();
		pattern.name = RoukaViciController.instance.vibrationPatterns[editID].name;
		pattern.delay = RoukaViciController.instance.vibrationPatterns[editID].delay;
		int j = 0;
		foreach (Fingers f in RoukaViciController.instance.vibrationPatterns[editID].fingers)
		{
			pattern.fingers[j] = new Fingers();
			pattern.fingers[j].pattern = new List<int>(f.pattern);
			pattern.fingers[j].id = j;
			++j;
		}
		currentPatternID = editID;
		dropdownIteration.options.Clear();
		InputFieldPatternName.text = pattern.name;
		InputFieldPatternDelay.text = pattern.delay.ToString();
		int count = pattern.fingers[0].pattern.Count;
		for (int i = 0 ; i < count ; ++i)
		{
			dropdownIteration.options.Add(new Dropdown.OptionData("Iteration " + (i + 1)));
		}
	}

	public void displayIteration()
	{
		for (int i = 0 ; i < (int)motorID.LAST_MOTOR ; ++i)
		{
			fingers[i].setValues(pattern.fingers[i].pattern[dropdownIteration.value]);
		}
	}

	public void savePattern()
	{
		string oldFile = RoukaViciController.instance.vibrationPatterns[currentPatternID].name;
		RoukaViciController.instance.vibrationPatterns[currentPatternID].name = pattern.name;
		RoukaViciController.instance.vibrationPatterns[currentPatternID].delay = pattern.delay;
		RoukaViciController.instance.vibrationPatterns[currentPatternID].fingers = pattern.fingers;

		string data = JsonUtility.ToJson(pattern, true);
		int index = data.IndexOf("\"fingers\"");
		data = data.Remove(index);

		string arrayData = JsonHelper.ToJson(pattern.fingers, true);
		arrayData = arrayData.Remove(arrayData.IndexOf('{'), 1);
		data += arrayData;
		# if UNITY_STANDALONE_WIN
			if (oldFile != pattern.name && File.Exists("Patterns\\" + oldFile + ".json"))
				File.Delete("Patterns\\" + oldFile + ".json");
			File.WriteAllText("Patterns\\" + pattern.name + ".json", data);
		# else
			if (oldFile != pattern.name && File.Exists("Patterns/" + oldFile + ".json"))
				File.Delete("Patterns/" + oldFile + ".json");
			File.WriteAllText("Patterns/" + pattern.name + ".json", data);
		# endif

		MenuManager.instance.addPatternSlot(RoukaViciController.instance.vibrationPatterns[currentPatternID]);
		PatternData d = RoukaViciController.instance.patternButtons[currentPatternID].GetComponent<PatternData>();
		d.patternName.text = pattern.name;
		MenuManager.instance.displayPatternList();
	}

	public void cancelEdit()
	{
		if (RoukaViciController.instance.vibrationPatterns.Count > RoukaViciController.instance.patternButtons.Count)
			RoukaViciController.instance.vibrationPatterns.RemoveAt(RoukaViciController.instance.vibrationPatterns.Count - 1);
		MenuManager.instance.displayPatternList();
	}

	public void updateName()
	{
		pattern.name = InputFieldPatternName.text;
	}

	public void updateDelay()
	{
		float v;
		if (InputFieldPatternDelay.text.ToCharArray()[0] == '-')
		{
			v = 0;
			InputFieldPatternDelay.text = "0";
		}
		else
			v = float.Parse(InputFieldPatternDelay.text);
		pattern.delay = v;
	}

	public void addIteration() 
	{
		foreach(Fingers f in pattern.fingers)
		{
			f.pattern.Insert(dropdownIteration.value + 1, 50);
		}
		dropdownIteration.options.Add(new Dropdown.OptionData("Iteration " + (dropdownIteration.options.Count + 1)));
		dropdownIteration.value += 1;
		displayIteration();
	}

	public void removeIteration()
	{
		if (pattern.fingers[0].pattern.Count == 1)
			return ;
		foreach (Fingers f in pattern.fingers)
		{
			f.pattern.RemoveAt(dropdownIteration.value);
		}
		dropdownIteration.options.RemoveAt(dropdownIteration.value);
		for (int i = dropdownIteration.value ; i < dropdownIteration.options.Count ; ++i)
			dropdownIteration.options[i].text = "Iteration " + (i + 1);
		if (dropdownIteration.value >= dropdownIteration.options.Count)
			dropdownIteration.value -= 1;
		displayIteration();
	}

	public int getIteration()
	{
		return dropdownIteration.value;
	}
}
