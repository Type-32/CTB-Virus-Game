using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtaskItem : MonoBehaviour
{
    [SerializeField] Text content, demand, accomplished;
    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;
    public void SetAppearance(string text, string demand, string accomplished, Sprite icon = null)
    {
        this.content.text = text;
        this.demand.text = demand;
        this.accomplished.text = accomplished;
        this.icon.sprite = icon;
    }
    public void Finished()
    {
        group.alpha = 0.3f;
    }//
}
