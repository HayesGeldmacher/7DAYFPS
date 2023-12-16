using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    public bool hasStarted = false;
    [SerializeField] private Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            TriggerDialogue();
        } 
    }

    private void TriggerDialogue()
    {
        if (!hasStarted)
        {
            manager.StartDialogue(dialogue, this);
            hasStarted = true;
        }
        else
        {
            manager.DisplayNextSentence();
        }


    }
}
