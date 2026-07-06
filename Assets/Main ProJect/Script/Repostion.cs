using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repostion : MonoBehaviour
{
    void LateUpdate()
    {
        if (transform.position.x > -12)
            return;

        transform.Translate(24, 0, 0, Space.Self);
    }
}
