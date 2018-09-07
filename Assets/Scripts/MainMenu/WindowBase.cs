using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBase : MonoBehaviour
{
    public void Show()
    {
        this.gameObject.SetActive(true);
        this.OnShow();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        this.OnHide();
    }

    public virtual void OnShow()
    {

    }

    public virtual void OnHide()
    {

    }

}
