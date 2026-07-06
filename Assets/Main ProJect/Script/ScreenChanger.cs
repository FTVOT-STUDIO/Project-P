using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChanger : MonoBehaviour
{
    public GameObject loading_Scene;
    public GameObject LoadUI;
    Image images;
    public Sprite[] LoadChars;

    Animator anim;

    void OnEnable()
    {   
        images = GetComponent<Image>();
        anim = GetComponent<Animator>();
        images.sprite = LoadChars[GameManager.Instance.PlayerId];
        StartCoroutine(Unload());
      //  print(images.sprite );
    }

    public void LoadImage()
    {
        GameManager.Instance.pickUI.SetActive(false);
        loading_Scene.SetActive(true);
        
    }

    public void UnloadImage()
    {
        LoadUI.SetActive(false);
       
    }


    IEnumerator Unload()
    {   
        yield return new WaitForSeconds(2f);

        anim.SetTrigger("Change");
        loading_Scene.SetActive(false);
    }


}
