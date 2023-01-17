using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TaskSystem;

public class TaskItem : MonoBehaviour
{
    public TaskInfo infoHolder;
    [SerializeField] Text title;
    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;
    [SerializeField] public Transform subtaskUIHolder;
    [HideInInspector] public List<SubtaskItem> subtaskItems;
    public void SetAppearance(string title, TaskInfo info, Sprite icon = null)
    {
        this.title.text = title;
        infoHolder = info;
        if(icon != null) this.icon.sprite = icon;
    }
    public void Finished()
    {
        InGameUI.Instance.taskDict.Remove(infoHolder);
        Destroy(gameObject);
    }
}
