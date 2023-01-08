using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtaskItem : MonoBehaviour
{
    [SerializeField] Text content, demand;
    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;
    public void SetAppearance(string text, string demand, Sprite icon = null)
    {
        this.content.text = text;
        this.demand.text = demand;
        this.icon.sprite = icon;
    }
    public void Finished()
    {
        group.alpha = 0.3f;
    }//
}
