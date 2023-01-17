using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtaskItem : MonoBehaviour
{
    [HideInInspector] public TaskItem father;
    [SerializeField] Text content, demand;
    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;
    public void SetAppearance(string text, string demand, Sprite icon = null)
    {
        this.content.text = text;
        this.demand.text = demand;
        if(icon != null) this.icon.sprite = icon;
    }
    public void Finished(bool finishFather = false)
    {
        group.alpha = 0.1f;
    }//
}
