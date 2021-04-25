using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSpike : Enemy
{
    public int health = 2;
    protected override void Start()
    {
        player = GameObject.Find("Player");
        rotationStart = transform.rotation.z;

        speed += Random.Range(0f, 0.025f);
        maxAngle += Random.Range(0, 25);

        Vector2 dir = getDirection();
        dir.Normalize();
        Debug.Log(dir);
        transform.position = (Vector2)transform.position +
            dir * -GetComponent<SpriteRenderer>().sprite.rect.height / 16f * 1.42f;

        origin = transform.position; 
        
        GetComponent<BoxCollider2D>().offset = new Vector2(0,GetComponent<SpriteRenderer>().sprite.rect.height / 32f);
        GetComponent<BoxCollider2D>().size = new Vector2(1,GetComponent<SpriteRenderer>().sprite.rect.height / 16f);

    }
    protected override Vector3 getDirection()
    {
        return new Vector3(0, player.transform.position.y - transform.position.y).normalized;
    }

    protected override void Update()
    {
        if (killed) return;
        Vector2 dir = getDirection();
        dir.Normalize();

        if (transform.childCount != 0)
        {
            charge += Time.deltaTime*5;
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
                GameCore.instance.GameOver();
                return;
            }
            transform.position = new Vector2(transform.position.x,
                Mathf.Lerp(origin.y,
                origin.y + GetComponent<SpriteRenderer>().sprite.rect.height / 16f * 1.42f * dir.y, 1-charge));
            return;
        }

        if (charge < 1)
        {
            charge += Time.deltaTime*8;
            transform.position = new Vector2(transform.position.x,
                Mathf.Lerp(origin.y, 
                origin.y + GetComponent<SpriteRenderer>().sprite.rect.height / 16f * 1.42f * dir.y, charge));
            if (charge >= 1)
            {
                //Spawn some particle and shake
                SpawnRandom(dir, 5);
                CameraMovement.instance.Shake();
            }
            return;
        }
        if (player.GetComponent<BoxCollider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds) &&
            player.transform.parent == null)
        {
            player.transform.SetParent(transform);
            charge = 0;
        }
    }
    public override void Kill()
    {
        health--;
        if(health < 0)
        base.Kill();
    }
}

