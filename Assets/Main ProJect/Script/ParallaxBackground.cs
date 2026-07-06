using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform playerTransform;
    public Transform[] backgrounds;
    public float backgroundXSize;
    public float backgroundYSize;
    public float XmoveSpeed;
    public float YmoveSpeed;

    private Vector3 lastPlayerPosition;
    private float backgroundCount;

    void Start()
    {
        lastPlayerPosition = playerTransform.position;
        backgroundCount = backgrounds.Length - 1;
    }

    void Update()
    {
        Vector3 playerPos = playerTransform.position - lastPlayerPosition;

        Vector3 Xmovement = new Vector3(-playerPos.x, 0f, 0f) * XmoveSpeed * Time.deltaTime;
        Vector3 Ymovement = new Vector3(0f, -playerPos.y, 0f) * YmoveSpeed * Time.deltaTime;

        MoveBackgrounds(Xmovement);
        MoveBackgrounds(Ymovement);


        lastPlayerPosition = playerTransform.position;
    }

    void MoveBackgrounds(Vector3 movement)
    {
        foreach (Transform background in backgrounds)
        {

            background.position += movement;

            Reposition(background, backgroundXSize, Vector3.right);
            Reposition(background, backgroundYSize, Vector3.up);

        }
    }

    // 배경 위치 조정 함수
    private void Reposition(Transform background, float size, Vector3 direction)
    {
        float offset = playerTransform.position.x - background.position.x;

        if (Mathf.Abs(offset) > size * backgroundCount)
        {
            background.position += direction * size * backgrounds.Length * Mathf.Sign(offset);
        }
    }
}
