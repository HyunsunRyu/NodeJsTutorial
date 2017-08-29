using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<System.Type, UIChild> uiDic;

    protected override void Init()
    {
        uiDic = new Dictionary<System.Type, UIChild>();

        foreach (UIChild ui in transform.GetComponentsInChildren<UIChild>(true))
        {
            System.Type type = ui.GetType();
            if (!uiDic.ContainsKey(type))
            {
                uiDic.Add(type, ui);
                ui.Init();

                OpenUI(type, false);
            }
        }
    }

    public void OpenUI<T>() where T : UIChild
    {
        OpenUI(typeof(T), true);
    }

    public void CloseUI<T>() where T : UIChild
    {
        OpenUI(typeof(T), false);
    }

    private void OpenUI(System.Type type, bool bOpen)
    {
        if (uiDic.ContainsKey(type))
        {
            uiDic[type].gameObject.SetActive(bOpen);
            if (bOpen)
                uiDic[type].UpdateUI();
        }
    }
}
