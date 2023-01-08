using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    [SerializeField] Text text, key;
    [SerializeField] CanvasGroup group;
    public void SetAppearance(string text, string key)
    {
        this.text.text = text;
        this.key.text = key;
    }
    public void Selected()
    {
        group.alpha = 0.3f;
    }
}
