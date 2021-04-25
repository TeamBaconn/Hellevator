using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawable : MonoBehaviour
{
    public GameObject blood;

    public static Drawable instance;

    public RuntimeAnimatorController bloodAnimator;

    private Collider2D collider;
    private Vector2? position;
    private Vector2 firstBlood;
    void Start()
    {
        instance = this;
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (GameCore.instance.over) return;
        if (Input.GetMouseButtonUp(0)) position = null;
        if (Input.GetMouseButton(0))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (collider.bounds.Contains(vec))
            {
                if (position != null && Vector3.Distance(vec, position.Value) < 0.05f) return;
                if(position != null)
                {
                    int times = (int)(Vector3.Distance(vec, position.Value) / 0.05f);
                    DrawBlood(vec);
                    Vector2 direction = vec - position.Value;
                    direction.Normalize();
                    position = vec;
                    for (int i = 0; i < times; i++)
                    {
                        vec += direction*0.05f;
                        DrawBlood(vec);
                    }
                    return;
                }
                position = vec;
            }
        }   
    }
    private void DrawBlood(Vector2 des)
    {
        if (!collider.bounds.Contains(des)) return;
            GameObject obj = Instantiate(blood, des, Quaternion.identity, transform);
        if (Vector2.Distance(firstBlood, obj.transform.position) > 1)
        {
            firstBlood = obj.transform.position;
            obj.GetComponent<Animator>().runtimeAnimatorController = bloodAnimator;
        }
    }
}
