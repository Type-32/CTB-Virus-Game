using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NPCDependencies
{
    [System.Serializable]
    public class Dialogue
    {
        [Tooltip("Sender Content. Shows as the title in the Dialogue UI.")]
        public string sent;
        [Tooltip("Receiver Content. Shows as the choice(s) in the Dialogue UI.")]
        public string received;
        [Tooltip("Counted in Seconds")]
        public int waitDuration = 10;
        public bool hasChoice;
        [Tooltip("List of Available Choices Correspondent to the events.")]
        public List<string> choices;
        [Tooltip("List of Events Correspondent to the choices.")]
        public List<UnityEvent> events;
    }
    [System.Serializable]
    public class NPCProperties
    {
        public string npcName;
        public bool allowDialogueRepeats = true;
    }
}
namespace TaskSystem
{
    [System.Serializable]
    public class TaskInfo
    {
        public string taskName;
        public List<SubtaskInfo> subtasks = new();
    }
    [System.Serializable]
    public class SubtaskInfo
    {
        public string taskContent;
        public int limit = 1;
        public int current = 0;
        public UnityEvent finishedEvent;
    }
}
