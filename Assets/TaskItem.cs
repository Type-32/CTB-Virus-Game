using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;
    [SerializeField] public Transform subtaskUIHolder;
    [HideInInspector] public List<SubtaskItem> subtaskItems;
    public void SetAppearance(string title, Sprite icon = null)
    {
        this.title.text = title;
        if(icon != null) this.icon.sprite = icon;
    }
    public void Finished()
    {
        group.alpha = 0.3f;
        Destroy(this.gameObject);
    }
}
