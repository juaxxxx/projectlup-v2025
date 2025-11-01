using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.UI;

using Roguelike.Util;

public class DisplayableButton : MonoBehaviour , IDisplayable
{
    private Sprite btnImage;
    private Action bindedAction;
    private Button button;
    private string btnName = null;
    private int canInteract = 1;

    void Awake()
    {
        button = GetComponent<Button>();

        if(button == null)
        {
            //UnityEngine.Debug.LogError("Fail To Make DisplayableDataButton Button");
            UnityEngine.Debug.LogError("Fail To Make DisplayableDataButton Button", this.gameObject);
        }
    }

    public void SetDisplayableImage(Sprite BtnImage)
    {
        if(BtnImage == null)
        {
            //UnityEngine.Debug.LogError("The Button Image is Null!!", this.gameObject);
            //return;
        }

        btnImage = BtnImage;

        Image img = button.GetComponent<Image>();

        if (img != null)
            img.sprite = btnImage;

    }
    public Sprite GetDisplayableImage()
    {
        if(btnImage != null)
        {
            return btnImage;
        }

        else
        {
            UnityEngine.Debug.LogWarning("Fail To Get Button Image", this.gameObject);
            return null;
        }
        
    }

    public void BindingFunction(Action action)
    {
        if(action == null)
        {
            UnityEngine.Debug.LogError("Bin Button Action Failed!", this.gameObject);
            return;
        }

        bindedAction = action;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => bindedAction.Invoke());
    }

    public Action GetBindedFunction()
    {
        if (bindedAction == null)
        {
            UnityEngine.Debug.LogError("DisplayableData Function was Null", this.gameObject);
            return null;
        }

        return bindedAction;
    }

    public string GetDisplayableName()
    {
        return btnName;
    }

    public int GetExtraInfo()
    {
        return canInteract;
    }

    public void SetExtraInfo(int extraInfo) { canInteract = extraInfo; }
}
