using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Image portraitImage;
    public Canvas canvas;
    public Animator animator;

    private PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        player.IsTalking = true;

        nameText.text = dialogue.name;
        portraitImage = dialogue.portrait;

        canvas.GetComponent<BasicInkExample>().InkJSONAsset = dialogue.inkFile;
        canvas.GetComponent<BasicInkExample>().StartStory();
    }

    /*IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }*/

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        player.IsTalking = false;
    }
}
