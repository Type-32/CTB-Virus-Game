using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCDependencies;

public interface IDialogueSender
{
    void OnSendDialogue();
    void OnRepliedDialogue();
    async void SendDialogue(Dialogue dialogue)
    {
        await Task.Delay(0);
    }
    async void SendDialogue(List<Dialogue> dialogues)
    {
        await Task.Delay(0);
    }
}
