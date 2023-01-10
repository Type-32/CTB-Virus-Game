using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NPCDependencies;
using UnityEngine;

public interface IDialogueSender
{
    void OnSendDialogue();

    void OnRepliedDialogue();

    void SendDialogue(PlayerController controller);
}
