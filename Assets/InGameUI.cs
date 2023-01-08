using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NPCDependencies;

public class InGameUI : MonoBehaviour
{
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
    [SerializeField] GameObject taskPrefab;
    float duration;
    void Awake()
    {
        DialogueUI.ui = this;
        TaskUI.ui = this;
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
    }
    void Update()
    {
        DialogueUI.UpdateUI();
        TaskUI.UpdateUI();
    }
}
