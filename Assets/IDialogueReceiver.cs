using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCDependencies;

public interface IDialogueReceiver
{
    void OnReceivedDialogue();
    void OnRepliedDialogue();
    void ReceiveDialogue(Dialogue dialogue, bool invokeDialogue = true, NPCScript npc = null);
    void ReceiveDialogue(List<Dialogue> dialogues, bool invokeDialogue = true, NPCScript npc = null);
    void InvokeDialogue(List<Dialogue> dialogues, NPCScript npc = null);
}
