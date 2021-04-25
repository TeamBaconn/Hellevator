using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Template : MonoBehaviour
{
    public GameObject objectivePrefab;

    public int last = 6;
    private int damage = 1;
    public virtual void Init(int k)
    {
        damage = k;
        last += damage / 2;
        foreach (Blood blood in FindObjectsOfType<Blood>()) Destroy(blood.gameObject);
        CameraMovement.instance.Shake();
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = GetComponent<SpriteRenderer>().sprite.texture;

        Vector2 origin = (Vector2)transform.position - new Vector2(sprite.textureRect.width/32f,
            sprite.textureRect.height/32f);
        for(int x = 0; x < sprite.textureRect.width; x++)
            for(int y = 0; y < sprite.textureRect.height; y++)
            {
                if (texture.GetPixel(x+(int)sprite.textureRect.x, y+(int)sprite.textureRect.y).a == 0) continue;
                Vector2 vec = origin + new Vector2(x/16f,+y/16f);
                if (!NotNear(vec, 0.5f)) continue;
                Instantiate(objectivePrefab, vec, Quaternion.identity, transform);
            }

    }

    protected bool NotNear(Vector2 location, float offset)
    {
        for(int i = 0; i < transform.childCount; i++) if (Vector2.Distance(transform.GetChild(i).position, location) < offset) return false;
        return true;
    }

    protected virtual void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, GetComponent<SpriteRenderer>().color.a - Time.deltaTime/last);

        if (transform.childCount == 0 || GetComponent<SpriteRenderer>().color.a <= 0)
        {
            //Remove temmplate
            CameraMovement.instance.Shake();
            if(transform.childCount == 0) GameCore.instance.castSpell(damage);
            Destroy(gameObject);
        }
    }
}
