using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCDependencies;

public class NPCScript : MonoBehaviour, IDialogueSender, ITaskDispatcher
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

    public void OnRepliedDialogue()
    {
        throw new System.NotImplementedException();
    }

    public void OnSendDialogue()
    {
        throw new System.NotImplementedException();
    }
    public void SendTask(PlayerController controller){
        
        throw new System.NotImplementedException();
    }
    public void OnSendTask(){
        throw new System.NotImplementedException();
    }
    public void OnAccomplishedTask(){
        throw new System.NotImplementedException();
    }
}
