using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGameObjectScript : MonoBehaviour
{
    public string GameObjectName;
    private GameObject gameObjectToShow;

    void Awake()
    {
        gameObjectToShow = GameObject.Find(GameObjectName);
    }

	public void Show()
    {
        TimeManager.Paused = true;
        gameObjectToShow.SetActive(true);
    }

    public void Hide()
    {
        TimeManager.Paused = false;
        gameObjectToShow.SetActive(false);
    }
}
