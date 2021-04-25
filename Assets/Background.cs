using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float time = 0;
    public float timeDone = 3f;

    void Update()
    {
        time += Time.deltaTime * GameCore.instance.getSpeed() * 2;
        if (time >= timeDone) time = 0;
        transform.GetChild(0).localPosition = new Vector2(0, Mathf.Lerp(0, 8, time / timeDone));
    }
}
