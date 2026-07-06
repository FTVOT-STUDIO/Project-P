using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public float delayTime = 2f; // 로딩 화면을 표시한 후 대기할 시간

    void Start()
    {
        // delayTime 이후에 Main 씬으로 이동하는 코루틴 시작
        StartCoroutine(LoadMainSceneAfterDelay());
    }

    IEnumerator LoadMainSceneAfterDelay()
    {
        // 대기
        yield return new WaitForSeconds(delayTime);

        // Main 씬으로 이동
        SceneManager.LoadScene("MainScenes");
    }
}
