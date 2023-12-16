using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text DiaText;
    DialogueTrigger currentTrigger;
    public Animator anim;

    [SerializeField] private introManager intro;

    void Start()
    {
        sentences = new Queue<string>();
    }


    //Begins a conversation when the dialogue trigger is activated from an NPC or item
    public void StartDialogue(Dialogue dialogue, DialogueTrigger trigger)
    {
        currentTrigger = trigger;
        Debug.Log("Starting Conversation with" + dialogue.name);
        //anim.SetBool("active", true);

        sentences.Clear();

        //uses FIFO to queue up each sentence in our public dialogue box
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        //Takes the next-added sentence out of the queue and loads it into text box
        string sentence = sentences.Dequeue();
        DiaText.text = sentence;
        Debug.Log(sentence);
    }

    public void EndDialogue()
    {

        intro.LoadScene();
        
        Debug.Log("End of Conversation");
        //anim.SetBool("active", false);
        if (currentTrigger)
        {
            currentTrigger.hasStarted = false;

        }
    }

    public IEnumerator EndDialogueTimer(float time)
    {

        yield return new WaitForSeconds(time);
        EndDialogue();
    }
}
