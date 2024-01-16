using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class TextManager : MonoBehaviour
{
    public Button buttonPrefab;
    public Button continuePrefab;
    public Text textBox;
    public static TextManager instance;
    private List<Text> textBoxes = new List<Text>();
    private List<Button> buttons = new List<Button>();
    private Story story;
    public GameObject scrollList;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        scrollList.SetActive(false);
    }

    public void Initialize(TextAsset storyJSON)
    {
        ClearText();
        PlayerManager.instance.player.canMove = false;
        PlayerManager.instance.player.rb.velocity = Vector2.zero;
        story = new Story(storyJSON.text);
        if(DataManager.instance.variablesState != null)
        {
            foreach (var item in DataManager.instance.variablesState)
            {
                if(story.variablesState.GlobalVariableExistsWithName(item))
                {
                    story.variablesState[item] = DataManager.instance.variablesState[item];
                }
            }
        }
        ContinueStory();
    }

    void ContinueStory()
    {
        //Debug.Log("Continuing the story.");
        PrintStory();
        //Debug.Log("Continuing the story.");
        var Choices = story.currentChoices;
        if (Choices.Count > 0)
        {
            //Debug.Log("There are multiple choices.");
            foreach (Choice choice in Choices)
            {
                Button choiceButton = Instantiate(buttonPrefab, this.transform);
                buttons.Add(choiceButton);

                Text text = choiceButton.GetComponent<Text>();
                text.text = $"<color=white>{(choice.index+1).ToString()}.-</color> {choice.text}";

                choiceButton.onClick.AddListener(delegate { OnClickChoiceButton(choice); });

            }
        }
        else
        {
            //Debug.Log("No Choices, Continuing.");
            if (story.canContinue)
            {
                Button continueButton = Instantiate(continuePrefab, this.transform);
                buttons.Add(continueButton);
                continueButton.onClick.AddListener(delegate { OnClickChoiceButton(null); });
            }
            else
            {
                Finish();
            }
        }

    }
    public void AddTextBox(string text) 
    {
        var newTextBox = Instantiate(textBox, this.transform);
        newTextBox.text = text;
        textBoxes.Add(newTextBox);
    }

    void OnClickChoiceButton(Choice choice)
    {
        if (choice != null) story.ChooseChoiceIndex(choice.index);
        ClearButtons();
        if (choice != null) PrintStory();
        ContinueStory();
    }

    void ClearButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
    }

    void ClearText()
    {
        for (int i = 0; i < textBoxes.Count; i++)
        {
            Destroy(textBoxes[i].gameObject);
        }
        textBoxes.Clear();
    }

    void PrintStory()
    {
        if (story.canContinue)
        {
            string text = story.Continue();
            foreach(string tag in story.currentTags)
            {
                if(tag.IndexOf("s:") == 0)
                {
                    text = tag.Substring(2) + " - " + text;
                }
            }
            AddTextBox(text);
        }
    }

    void Finish()
    {
        ClearText();
        DataManager.instance.variablesState = story.variablesState;
        PlayerManager.instance.player.canMove = true;
        scrollList.SetActive(false);
    }

}
