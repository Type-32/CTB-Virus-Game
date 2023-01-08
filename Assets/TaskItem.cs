using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;
    public void SetAppearance(string title, Sprite icon = null)
    {
        this.title.text = title;
        this.icon.sprite = icon;
    }
    public void Selected()
    {
        group.alpha = 0.3f;
    }
    public void AddSubtask()
    {

    }
}
