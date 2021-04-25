using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverText : Template
{
    public Sprite sprite;
    private Vector2 oldDes = Vector2.zero;
    public override void Init()
    {
        foreach (Blood blood in FindObjectsOfType<Blood>()) Destroy(blood.gameObject);
        CameraMovement.instance.Shake();
        GetComponent<SpriteRenderer>().sprite = sprite;
        StartCoroutine(run());
    }
    IEnumerator run()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = GetComponent<SpriteRenderer>().sprite.texture;

        Vector2 origin = (Vector2)transform.position - new Vector2(sprite.textureRect.width / 32f,
            sprite.textureRect.height / 32f);
        for (int y = (int)sprite.textureRect.height-1; y >= 0; y--)
        {
            for (int x = 0; x < sprite.textureRect.width; x++)
            {
                if (texture.GetPixel(x + (int)sprite.textureRect.x, y + (int)sprite.textureRect.y).a == 0) continue;
                Vector2 vec = origin + new Vector2(x / 16f, +y / 16f);
                if (!NotNear(vec, 0.1f)) continue;
                GameObject obj = Instantiate(objectivePrefab, vec, Quaternion.identity, transform);
                if(Vector2.Distance(oldDes,vec) > 1)
                {
                    obj.GetComponent<Animator>().runtimeAnimatorController = Drawable.instance.bloodAnimator;
                    oldDes = vec;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    protected override void Update()
    {
    }
}
