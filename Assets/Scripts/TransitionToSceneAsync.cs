using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToSceneAsync : MonoBehaviour
{
    public string SceneName;
    public float MinTimeBeforeTransition = 0;

    private float currentTime = 0;
    private AsyncOperation operation;

    private void Start()
    {
        operation = SceneManager.LoadSceneAsync(SceneName);
        operation.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (MinTimeBeforeTransition > 0)
        {
            currentTime += Time.deltaTime;

            if (currentTime > MinTimeBeforeTransition)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
