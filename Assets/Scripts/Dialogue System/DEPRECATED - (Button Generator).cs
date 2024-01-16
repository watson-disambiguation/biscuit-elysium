using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class ButtonGenerator : MonoBehaviour
{
    public Button buttonPrefab;
    public Button continuePrefab;
    public TextAsset storyJSON;
    private List<Button> buttons;
    private Story story;
    void Start()
    {
        story = new Story(storyJSON.text);
        ContinueStory();
    }

    void ContinueStory()
    {

        PrintStory();
        var Choices = story.currentChoices;
        if(Choices.Count > 0)
        {
            foreach(Choice choice in Choices)
            {
                Button choiceButton = Instantiate(buttonPrefab, this.transform);
                buttons.Add(choiceButton);

                Text text = choiceButton.GetComponent<Text>();
                text.text = choice.text;

                choiceButton.onClick.AddListener(delegate { OnClickChoiceButton(choice); });
                
            }
        }
        else 
        {
            if(story.canContinue)
            {
                Button continueButton = Instantiate(continuePrefab, this.transform);
                buttons.Add(continueButton);
                continueButton.onClick.AddListener(delegate { OnClickChoiceButton(null); });
            }
        }

    }

    void OnClickChoiceButton(Choice choice)
    {
        if(choice != null) story.ChooseChoiceIndex(choice.index);
        ClearButtons();
        if (choice != null) PrintStory();
        ContinueStory();
    }

    void ClearButtons()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
    }

    void PrintStory()
    {
        if (story.canContinue)
        {
            TextManager.instance.AddTextBox(story.Continue());
        }
    }


}
