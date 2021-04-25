using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Enemy
{
    protected override void Start()
    {
        player = GameObject.Find("Player");
        rotationStart = transform.rotation.z;

        speed += Random.Range(0f, 0.025f);
        maxAngle += Random.Range(0, 25);

        Vector2 dir = getDirection();
        dir.Normalize();
        transform.position = (Vector2)transform.position +
            dir * -GetComponent<SpriteRenderer>().sprite.rect.width / 16f * 1.42f;

        origin = transform.position; 
        
        GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<SpriteRenderer>().sprite.rect.width / 32f, 0);
        GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<SpriteRenderer>().sprite.rect.width / 16f, 0.1f);

    }
    protected override Vector3 getDirection()
    {
        return new Vector3(player.transform.position.x - transform.position.x, 0).normalized;
    }

    protected override void Update()
    {
        if (killed) return;
        Vector2 dir = getDirection();
        dir.Normalize();

        if (transform.childCount != 0)
        {
            charge += Time.deltaTime * 4;
            if (!outside)
            {
                outside = true;
                SpawnRandom(dir, 10);
            }
            if (charge > 1)
            {
                killed = true;
                player.GetComponent<SpriteRenderer>().enabled = false;
                player.transform.position = new Vector2(0, -3.24f);
            }
            transform.position = Vector2.Lerp(origin + dir * GetComponent<SpriteRenderer>().sprite.rect.width / 16f * 1.42f, origin - dir * 2, charge);
            return;
        }

        if (charge < 1)
        {
            charge += Time.deltaTime*4;
            transform.position = new Vector2(
                Mathf.Lerp(origin.x, 
                origin.x + GetComponent<SpriteRenderer>().sprite.rect.width / 16f * 1.42f * dir.x, charge), transform.position.y);
            if (charge >= 1)
            {
                //Spawn some particle and shake
                SpawnRandom(dir, 5);
                CameraMovement.instance.Shake();
            }
            return;
        }
        if (player.GetComponent<BoxCollider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds) &&
            player.transform.parent == null && !GameCore.instance.over)
        {
            GameCore.instance.GameOver();
            player.transform.SetParent(transform);
            GetComponent<SpikeMovement>().enabled = false;
            charge = 0;
        }
    }
}
