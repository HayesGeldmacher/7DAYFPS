using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text DiaText;
    DialogueTrigger currentTrigger;
    [SerializeField] private Animator _textAnim;
    [SerializeField] private Animator _hazardAnim;
    [SerializeField] private float waitTime;
    [SerializeField] private introManager intro;
    [SerializeField] private AudioSource _finalAudio;
    [SerializeField] private AudioSource _confirmAudio;
    private bool _isStarting = false;


    void Start()
    {
        sentences = new Queue<string>();
    }


    //Begins a conversation when the dialogue trigger is activated from an NPC or item
    public void StartDialogue(Dialogue dialogue, DialogueTrigger trigger)
    {
        currentTrigger = trigger;
        Debug.Log("Starting Conversation with" + dialogue.name);

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
        if (!_isStarting)
        {

            _confirmAudio.Play();
        
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

    }

    public void EndDialogue()
    {
        StartCoroutine(EndOfDialogue());
        
    }

    private IEnumerator EndOfDialogue()
    {
        if (!_isStarting)
        {
            _isStarting = true;
            _finalAudio.Play();
            _textAnim.SetTrigger("fade");
            _hazardAnim.SetTrigger("fade");
            yield return new WaitForSeconds(waitTime);
            intro.LoadScene();

        }
        else
        {

        }

    }

    public IEnumerator EndDialogueTimer(float time)
    {

        yield return new WaitForSeconds(time);
        EndDialogue();
    }
}
