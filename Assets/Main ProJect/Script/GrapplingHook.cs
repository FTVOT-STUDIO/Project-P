using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrapplingHook : MonoBehaviour
{
    public static GrapplingHook instance;

    public LineRenderer line;
    public Transform hook;
    private Vector2 mousedir;

    public bool isHookActive;
    public bool isLineMax;
    public bool isAttach;

    public bool HookClick = false;

    public GameObject hookButton;
    public GameObject hookInfo;

    public float searchRadius = 5f;
    public bool hasGrapplingHook = false;

    private Vector3 targetPosition;
    private GameObject lastClosestHook;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        line.positionCount = 2;
        line.startWidth = 0f;
        line.endWidth = 0.20f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    void Update()
    {
        if (hasGrapplingHook)
        {
            SearchHook();
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (HookClick && !isHookActive)
        {
            hook.position = transform.position;

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);

            if (SearchHook() && hasGrapplingHook)
            {
                isHookActive = true;
                isLineMax = false;
                hook.gameObject.SetActive(true);
                GameManager.Instance.grapplingCount++;
                QuestUI.Instance.CollectItem(7);
                //  GameManager.Instance.Player.capsuleCollider2D.enabled = false;
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hook);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Wire);
            }
            HookClick = false;
        }

        if (isHookActive && !isAttach)
        {
            hook.position = Vector3.MoveTowards(hook.position, targetPosition, Time.deltaTime * 20);
            line.SetPosition(1, hook.position);
        }
        else if (isHookActive && isLineMax && !isAttach)
        {
            if (Vector2.Distance(hook.position, transform.position) > 0.1f)
            {
                hook.position = Vector3.MoveTowards(hook.position, transform.position, Time.deltaTime * 15);
            }
            else
            {
                ResetHook();
            }
        }
        else if (isAttach && (HookClick || GameManager.Instance.Player.inputJump || Input.GetButtonDown("Jump")))
        {
            DetachHook();
        }
    }

    private bool SearchHook()
    {
        Image hookBtnImage = hookButton.GetComponent<Image>();
        GameObject[] hooks = GameObject.FindGameObjectsWithTag("Hook");

        float closestDistance = Mathf.Infinity;
        GameObject closestHook = null;

        foreach (GameObject hookObject in hooks)
        {
            float distance = Vector2.Distance(transform.position, hookObject.transform.position);
            if (distance < closestDistance && distance <= searchRadius)
            {
                closestDistance = distance;
                closestHook = hookObject;
            }
        }

        if (closestHook != null)
        {
            targetPosition = closestHook.transform.position;
            hookBtnImage.color = Color.white;
            UpdateHookColor(closestHook);
            return true;
        }
        else
        {
            hookBtnImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            ResetLastClosestHook();
            return false;
        }
    }

    private void UpdateHookColor(GameObject closestHook)
    {
        if (lastClosestHook != null && lastClosestHook != closestHook)
        {
            SpriteRenderer lastHookRenderer = lastClosestHook.GetComponent<SpriteRenderer>();
            if (lastHookRenderer != null)
            {
                lastHookRenderer.color = Color.white;
            }
        }

        SpriteRenderer closestHookRenderer = closestHook.GetComponent<SpriteRenderer>();
        if (closestHookRenderer != null)
        {
            closestHookRenderer.color = Color.red;
        }

        lastClosestHook = closestHook;
    }

    private void ResetLastClosestHook()
    {
        if (lastClosestHook != null)
        {
            SpriteRenderer lastHookRenderer = lastClosestHook.GetComponent<SpriteRenderer>();
            if (lastHookRenderer != null)
            {
                lastHookRenderer.color = Color.white;
            }
            lastClosestHook = null;
        }
    }

    private void ResetHook()
    {
        isHookActive = false;
        isLineMax = false;
        hook.gameObject.SetActive(false);

    }

    private void DetachHook()
    {
        isAttach = false;
        isHookActive = false;
        isLineMax = false;
        hook.GetComponent<Hooking>().joint2D.enabled = false;
        hook.gameObject.SetActive(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.isAttach);
        HookClick = false;
        // GameManager.Instance.Player.capsuleCollider2D.enabled = true;
        GameManager.Instance.Player.inputJump = false;
    }
}
