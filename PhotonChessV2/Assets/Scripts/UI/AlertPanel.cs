using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanel : MonoBehaviour
{
    public enum AlertType { Warning, Error, Info }
    [Header("UI Elements")]
    public Text title;
    public Text description;
    public Image[] alertIcons;
    [Header("Sources")]
    public Sprite error;
    public Sprite warning;
    public Sprite info;
    public void Activate(string title,string description,AlertType alertType)
    {
        this.title.text = title;
        this.description.text = description;
        switch (alertType)
        {
            case AlertType.Error:
                this.description.color = Color.red;
                SetIcons(error);
                break;
            case AlertType.Info:
                this.description.color = Color.black;
                SetIcons(info);
                break;
            case AlertType.Warning:
                this.description.color = Color.black;
                SetIcons(warning);
                break;
        }
        gameObject.SetActive(true);
    }

    private void SetIcons(Sprite sprite)
    {
        foreach(Image icon in alertIcons)
            icon.sprite = sprite;
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
