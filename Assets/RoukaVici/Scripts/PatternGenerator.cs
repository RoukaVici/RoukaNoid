using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(MenuManager))]
public class PatternGenerator : MonoBehaviour
{
	private void getFiles(string folder)
	{
		DirectoryInfo dir = new DirectoryInfo(folder);
		FileInfo[] info = dir.GetFiles("*.json");

		foreach (FileInfo f in info) {
			string data = File.ReadAllText(f.FullName);
			string arrayData = data.Remove(1, data.IndexOf("\"fingers\"") - 1);
			VibrationStyle vs = JsonUtility.FromJson<VibrationStyle>(data);
			vs.fingers = JsonHelper.FromJson<Fingers>(arrayData);
			RoukaViciController.instance.vibrationPatterns.Add(vs);
		}
		if (RoukaViciController.instance.vibrationPatterns.Count == 0)
		{
			VibrationStyle vs = new VibrationStyle();
			vs.delay = 0.5f;
			vs.name = "Default";
			int i = 0;
			foreach (Fingers f in vs.fingers)
			{
				f.id = i++;
				f.pattern.Add(50);
			}
			RoukaViciController.instance.vibrationPatterns.Add(vs);
		}
	}

	// Use this for initialization
	void Start ()
	{
		this.getFiles("Patterns");

		MenuManager menuManager = GetComponent<MenuManager>();
		float slotHeight = menuManager.slotPrefab.GetComponent<RectTransform>().rect.height;
		menuManager.scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, slotHeight * (RoukaViciController.instance.vibrationPatterns.Count + 1));

		int i = 0;
		foreach (VibrationStyle vs in RoukaViciController.instance.vibrationPatterns)
		{
			GameObject button = Instantiate(menuManager.slotPrefab);
			button.transform.SetParent(menuManager.scrollViewContent, false);
			button.GetComponentInChildren<Text>().text = vs.getName();
			PatternData data = button.GetComponent<PatternData>();
			data.ID = i;
			data.selectPattern.onClick.AddListener(delegate {RoukaViciController.instance.setVibrationPattern(data.ID);});
			data.editPattern.onClick.AddListener(delegate {menuManager.editPattern(data.ID);});
			data.removePattern.onClick.AddListener(delegate {menuManager.removePattern(data.ID);});
			RoukaViciController.instance.patternButtons.Add(button);
			i += 1;
		}
		menuManager.initializeUI();
	}
}
