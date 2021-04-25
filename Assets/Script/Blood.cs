using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Objective"))
        {
            if (Vector2.Distance(obj.transform.position, transform.position) < 0.3f)
            {
                Destroy(obj);
            }
        }
    }
}
