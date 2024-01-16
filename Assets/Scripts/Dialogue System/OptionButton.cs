using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
	public Button button;
	public Text textBox;
	public string text;

	void Start()
	{
		button = GetComponent<Button>();
		textBox = GetComponentInChildren<Text>();
		textBox.text = text;
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		TextManager.instance.AddTextBox(text);
	}
}
