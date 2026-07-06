using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapEvent : MonoBehaviour
{
    Tilemap tilemap;
    public float changeTime;
    Color curruntColor;
    Coroutine fadeCoroutine; // 현재 실행 중인 코루틴을 추적하는 변수

    void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
    }

    void Start()
    {
        curruntColor = tilemap.color; // 원래 색상 저장
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (fadeCoroutine != null && gameObject.activeSelf)
            {
                StopCoroutine(fadeCoroutine);
            }
            if (gameObject.activeSelf)
                fadeCoroutine = StartCoroutine(FadeIn()); // FadeIn 코루틴 시작
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (fadeCoroutine != null && gameObject.activeSelf)
            {
                StopCoroutine(fadeCoroutine);
            }
            if (gameObject.activeSelf)
                fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        float currentTime = 0;
        Color startColor = tilemap.color;

        while (currentTime < changeTime)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / changeTime;
            tilemap.color = Color.Lerp(startColor, new Color(0, 0, 0, 0), normalizedTime); // 색상에서 투명으로 보간
            yield return null;
        }

        tilemap.color = new Color(0, 0, 0, 0); // 최종적으로 투명한 색상으로 설정
    }

    IEnumerator FadeOut()
    {
        float currentTime = 0;
        Color startColor = tilemap.color;

        while (currentTime < changeTime)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / changeTime;
            tilemap.color = Color.Lerp(startColor, curruntColor, normalizedTime); // 투명에서 원래 색상으로 보간
            yield return null;
        }

        tilemap.color = curruntColor; // 최종적으로 원래 색상으로 설정
    }
}
