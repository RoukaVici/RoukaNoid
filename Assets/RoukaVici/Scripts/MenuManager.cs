using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PatternGenerator))]
public class MenuManager : MonoBehaviour
{
	private static MenuManager _instance;
	public static MenuManager instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<MenuManager>();
			}
			return _instance;
		}
	}


	[SerializeField]
	private Color selectedItemColor;
	private Color currentItemColor;

	[SerializeField]
	private GameObject PatternList;

	[SerializeField]
	private GameObject PatternEditorMenu;

	[SerializeField]
	private PatternEditorData patternEditorData;
	public int patternNbLimit = 20;

	public GameObject slotPrefab;

	public RectTransform scrollViewContent;


	void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		displayPatternList();
	}

	void Start()
	{

	}

	public void displayPatternList()
	{
		if (PatternList)
			PatternList.SetActive(true);
		if (PatternEditorMenu)
			PatternEditorMenu.SetActive(false);
	}

	private void displayPatternEditorMenu()
	{
		if (PatternList)
			PatternList.SetActive(false);
		if (PatternEditorMenu)
			PatternEditorMenu.SetActive(true);
	}

	public void addPattern()
	{
		if (RoukaViciController.instance.patternButtons.Count >= patternNbLimit)
			return ;
		VibrationStyle vs = new VibrationStyle();
		vs.name = "My Pattern";
		vs.delay = 0.3f;
		for (int i = 0 ; i < vs.fingers.Length ; ++i)
		{
			vs.fingers[i] = new Fingers();
			vs.fingers[i].id = i;
			vs.fingers[i].pattern = new List<int>();
			vs.fingers[i].pattern.Add(50);
		}
		RoukaViciController.instance.vibrationPatterns.Add(vs);
		editPattern(RoukaViciController.instance.vibrationPatterns.Count - 1);
	}

	public void removePattern(int id)
	{
		if (RoukaViciController.instance.patternButtons.Count <= 1)
			return;
		# if UNITY_STANDALONE_WIN
			if (File.Exists("Patterns\\" + RoukaViciController.instance.vibrationPatterns[id].name + ".json"))
				File.Delete("Patterns\\" + RoukaViciController.instance.vibrationPatterns[id].name + ".json");
		# else
			if (File.Exists("Patterns/" + RoukaViciController.instance.vibrationPatterns[id].name + ".json"))
				File.Delete("Patterns/" + RoukaViciController.instance.vibrationPatterns[id].name + ".json");
		# endif
		Destroy(RoukaViciController.instance.patternButtons[id]);
		RoukaViciController.instance.patternButtons.RemoveAt(id);
		RoukaViciController.instance.vibrationPatterns.RemoveAt(id);
		if (RoukaViciController.instance.patternID == id)
		{
			RoukaViciController.instance.patternID = 0;
			RoukaViciController.instance.patternButtons[0].GetComponent<PatternData>().background.color = selectedItemColor;
		}
		if (id != RoukaViciController.instance.patternButtons.Count)
			rearrangeButtons();
	}

	public void rearrangeButtons()
	{
		int i = 0;
		foreach (GameObject b in RoukaViciController.instance.patternButtons)
		{
			b.GetComponent<PatternData>().ID = i;
			++i;
		}
	}

	public void editPattern(int editID)
	{
		displayPatternEditorMenu();
		patternEditorData.prepareEditor(editID);
		patternEditorData.displayIteration();
	}

	public void initializeUI()
	{
		PatternData data = RoukaViciController.instance.patternButtons[0].GetComponent<PatternData>();
		currentItemColor = data.background.color;
		data.background.color = selectedItemColor;
	}

	public void selectPatternButton(int newID, int oldID)
	{
		RoukaViciController.instance.patternButtons[oldID].GetComponent<PatternData>().background.color = currentItemColor;
		RoukaViciController.instance.patternButtons[newID].GetComponent<PatternData>().background.color = selectedItemColor;
	}

	public void addPatternSlot(VibrationStyle vs)
	{
		GameObject button = Instantiate(slotPrefab);
		button.transform.SetParent(scrollViewContent, true);
		button.GetComponentInChildren<Text>().text = vs.getName();
		PatternData data = button.GetComponent<PatternData>();
		data.selectPattern.onClick.AddListener(delegate {RoukaViciController.instance.setVibrationPattern(data.ID);});
		data.editPattern.onClick.AddListener(delegate {editPattern(data.ID);});
		data.removePattern.onClick.AddListener(delegate {removePattern(data.ID);});
		RoukaViciController.instance.patternButtons.Add(button);
		rearrangeButtons();
	}
}
