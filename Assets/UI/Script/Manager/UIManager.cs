using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject ui_gameScene;

    private GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        GameObject go = Instantiate(ui_gameScene, Root.transform);
        go.GetComponent<UI_GameScene>().Init();
    }

    public T ShowPopup<T>(string name = null)
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        Debug.Log("UI/" + name);
        GameObject obj = Resources.Load<GameObject>("UI/" + name);
        GameObject popup = Instantiate(obj);
        popup.transform.SetParent(Root.transform, worldPositionStays: false);

        return popup.GetComponent<T>();
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        Destroy(popup.gameObject);
    }
}
