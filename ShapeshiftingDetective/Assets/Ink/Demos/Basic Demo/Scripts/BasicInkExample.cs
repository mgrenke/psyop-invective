using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;
	
    /*void Awake () {
		// Remove the default message
		RemoveChildren();
		StartStory();
	}*/

	// Creates a new Story object with the compiled story which we can then play!
	public void StartStory () {
		story = new Story (inkJSONAsset.text);
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();
		
		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("...");
			var dialogueManager = FindObjectOfType<DialogueManager>();
			choice.onClick.AddListener(delegate{
				dialogueManager.EndDialogue();
			});
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		storyText = Instantiate (textPrefab) as Text;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(text));
		storyText.transform.SetParent (canvas.transform, false);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = "> " + text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	IEnumerator TypeSentence(string sentence)
    {
        storyText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(0.02f);			
        }
    }


	/*void ParseTags()
	{
		tags = story.currentTags;
		foreach(string t in tags)
		{
			string prefix = t.Split(' ')[0];
			string param = t.Split(' ')[1];

			switch(prefix.ToLower())
			{
				
			}
		}
	}*/

	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;
	List<string> tags;
	Text storyText;

	[SerializeField]
	private Canvas canvas = null;

	// UI Prefabs
	[SerializeField]
	private Text textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;

	public TextAsset InkJSONAsset 
	{
		set { inkJSONAsset = value; }
	}
}
