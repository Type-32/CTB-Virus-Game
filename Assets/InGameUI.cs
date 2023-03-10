using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NPCDependencies;
using TaskSystem;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;
    public GameObject hud, gui;
    [Space, Header("Dialogue UI")]
    public GameObject dialogueUI;
    [SerializeField] Transform dialogueChoiceHolder;
    [SerializeField] GameObject dialogueChoicePrefab;
    [SerializeField] Slider dialogueDuration;
    [SerializeField] Text dialogueSender;
    public Text dialogueTitle;
    [Space, Header("Task UI")]
    [SerializeField] GameObject taskUI;
    [SerializeField] Text title;
    [SerializeField] protected GameObject taskPrefab, subtaskPrefab;
    [SerializeField] protected Transform taskPrefabHolder;
    float duration;
    public Dictionary<SubtaskInfo, SubtaskItem> subtaskDict = new();
    public Dictionary<TaskInfo, TaskItem> taskDict = new();
    void Awake()
    {
        Instance = this;
        DialogueUI.ui = this;
        TaskUI.ui = this;
        InGameUI.DialogueUI.SetUIActive(false);
    }
    public static class DialogueUI
    {
        public static InGameUI ui;
        public static bool enabled = false;
        static List<UnityEvent> dialogueEvent = new();
        public static void SetUIActive(bool boolean) { enabled = boolean; ui.dialogueUI.SetActive(enabled); }
        public static void UpdateUI()
        {
            if (!enabled) return;
            if (ui.duration > 0f) ui.duration -= Time.deltaTime;
            ui.dialogueDuration.value = ui.duration;
        }
        public static List<DialogueChoice> choicesCache = new();
        public static void SetUIData(Dialogue dialogue, NPCProperties properties)
        {
            ui.dialogueSender.text = properties.npcName;
            if (choicesCache.Count >= 1)
            {
                foreach (DialogueChoice t in choicesCache)
                {
                    Destroy(t.gameObject);
                }
                choicesCache.Clear();
            }
            dialogueEvent = dialogue.events;
            ui.dialogueTitle.text = dialogue.sent;
            int keyCount = 1;
            if (dialogue.choices.Count >= 1)
            {
                foreach (string tmp in dialogue.choices)
                {
                    DialogueChoice temp = Instantiate(ui.dialogueChoicePrefab, ui.dialogueChoiceHolder).GetComponent<DialogueChoice>();
                    temp.SetAppearance(tmp, keyCount.ToString());
                    keyCount++;
                    choicesCache.Add(temp);
                }
            }
            else
            {
                DialogueChoice temp = Instantiate(ui.dialogueChoicePrefab, ui.dialogueChoiceHolder).GetComponent<DialogueChoice>();
                temp.SetAppearance(dialogue.received, keyCount.ToString());
                choicesCache.Add(temp);
            }
            ui.dialogueDuration.value = ui.dialogueDuration.maxValue = ui.duration = dialogue.waitDuration;
        }

    }
    public static class TaskUI
    {
        public static InGameUI ui;
        public static bool enabled = false;
        public static void SetUIActive(bool boolean) { enabled = boolean; ui.taskUI.SetActive(enabled); }
        public static void UpdateUI()
        {
            if (!enabled) return;
        }
        public static TaskInfo AddTaskUI(TaskInfo info){
            Debug.Log("Adding Task UI Component...");
            TaskItem uiItem = Instantiate(ui.taskPrefab, ui.taskPrefabHolder).GetComponent<TaskItem>();
            uiItem.SetAppearance(info.taskName, info);
            ui.taskDict.Add(info, uiItem);
            foreach(SubtaskInfo tp in info.subtasks){
                SubtaskItem temp = Instantiate(ui.subtaskPrefab, uiItem.subtaskUIHolder).GetComponent<SubtaskItem>();
                uiItem.subtaskItems.Add(temp);
                temp.father = uiItem;
                temp.SetAppearance(tp.taskContent, $"{tp.current}/{tp.limit}");
                ui.subtaskDict.Add(tp, temp);
            }
            return info;
        }
        public static TaskInfo RemoveTaskUI(TaskInfo info){
            foreach(SubtaskInfo tp in info.subtasks){
                if(ui.subtaskDict.ContainsKey(tp)){
                    ui.subtaskDict[tp].Finished();
                    ui.subtaskDict.Remove(tp);
                }
            }
            if(ui.taskDict.ContainsKey(info))
            {
                ui.taskDict[info].Finished();
                ui.taskDict.Remove(info);
            }
            return info;
        }
    }
    void Update()
    {
        DialogueUI.UpdateUI();
    }
    void FixedUpdate()
    {
        TaskUI.UpdateUI();
    }
    public TaskInfo AddTaskUI(TaskInfo info)
    {
        return InGameUI.TaskUI.AddTaskUI(info);
    }
    public TaskInfo RemoveTaskUI(TaskInfo info)
    {
        return InGameUI.TaskUI.RemoveTaskUI(info);
    }
    public void SetUIActive(bool boolean) { DialogueUI.SetUIActive(boolean); }
}
