using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCDependencies;

public class NPCScript : MonoBehaviour, IDialogueSender
{
    public NPCProperties properties;
    public List<Dialogue> dialogues = new();
    public void SendDialogue(PlayerController controller)
    {
        List<Dialogue> temp = new();
        foreach (Dialogue t in dialogues)
        {
            temp.Add(t);
        }
        controller.ReceiveDialogue(temp, true, this);
    }

    void IDialogueSender.OnRepliedDialogue()
    {
        throw new System.NotImplementedException();
    }

    void IDialogueSender.OnSendDialogue()
    {
        throw new System.NotImplementedException();
    }
}
