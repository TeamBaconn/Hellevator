using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public int y = 1;
    public float max_y = 3.3f;
    public bool destroy = true;
    public bool follow = true;
    private void Update()
    {
        if ((transform.position.y >= max_y && y > 0) || (transform.position.y <= max_y && y < 0))
        {
            if (destroy)
                Destroy(gameObject);
            else return;
        }
        transform.position += new Vector3(0, y) * Time.deltaTime/4 * (follow ? GameCore.instance.getSpeed() : 1);
    }
}
