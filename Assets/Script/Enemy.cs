using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected float time = 0f;
    protected bool reverse = false;
    public float speed = 1f;
    public float maxAngle = 5f;

    protected float rotationStart;

    protected float charge = 0;
    protected Vector2 origin;

    public GameObject debrishPrefab;
    public GameObject bloodPrefab;

    public GameObject player;

    protected virtual void Start()
    {
        player = GameObject.Find("Player");
        rotationStart = transform.rotation.z;

        speed += Random.Range(0f, 0.025f);
        maxAngle += Random.Range(0, 25);

        Vector2 dir = getDirection();
        dir.Normalize();
        transform.position = (Vector2) transform.position +
            dir * -GetComponent<SpriteRenderer>().sprite.rect.width/16f * 1.42f;
        rotationStart = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float rightZ = rotationStart + Mathf.Lerp(0, maxAngle, time / speed);
        transform.rotation = Quaternion.Euler(0, 0, rightZ);
        origin = transform.position;
        GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<SpriteRenderer>().sprite.rect.width / 32f, 0);
        GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<SpriteRenderer>().sprite.rect.width / 16f,0.1f);
    }
    protected bool outside = false;
    protected virtual void Update()
    {
        if (killed) return;
        Vector2 dir = getDirection();
        dir.Normalize();
        
        if(transform.childCount != 0)
        {
            charge += Time.deltaTime*4;
            if(!outside)
            {
                outside = true;
                SpawnRandom(dir, 10);
            }
            if(charge > 1)
            {
                killed = true;
                player.GetComponent<SpriteRenderer>().enabled = false;
                player.transform.position = new Vector2(0,-3.24f);
                GameCore.instance.GameOver();
            }
            transform.position = Vector2.Lerp(origin + dir * GetComponent<SpriteRenderer>().sprite.rect.width / 16f * 1.42f, origin-dir*2, charge);
            return;
        }

        rotationStart = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (charge < 1)
        {
            charge += Time.deltaTime*5;
            transform.position = Vector2.Lerp(origin, origin + dir * GetComponent<SpriteRenderer>().sprite.rect.width / 16f * 1.42f, charge);
            if(charge >= 1)
            {
                //Spawn some particle and shake
                SpawnRandom(dir,5);
                CameraMovement.instance.Shake();
            }
            return;
        }
        time += Time.deltaTime*(reverse?-1:1);
        if (time <= 0) reverse = false;
        else if (time >= speed) reverse = true;

        float rightZ = rotationStart + Mathf.Lerp(0, maxAngle, time / speed);
        transform.rotation = Quaternion.Euler(0, 0, rightZ);

        if(player.GetComponent<BoxCollider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds) && 
            player.transform.parent == null)
        {
            player.transform.SetParent(transform);
            charge = 0;
        }
    }

    protected virtual Vector3 getDirection()
    {
        return player.transform.position + new Vector3(0,2.5f) - transform.position;
    }

    protected void SpawnRandom(Vector2 dir, int dmax)
    {
        int max = Random.Range(3, dmax);
        for (int i = 0; i < max; i++)
        {
            float angle = Random.Range(-1f, 1f);
            Vector2 ranDir = new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle),
                dir.y * Mathf.Cos(angle) + dir.x * Mathf.Sin(angle));
            SpawnDep(ranDir * 10f);
        }
    }

    protected void SpawnDep(Vector2 direction)
    {
        GameObject obj = Instantiate(debrishPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 180)), null);
        obj.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
    }

    protected void SpawnBlood(Vector2 direction)
    {
        GameObject obj = Instantiate(bloodPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 180)), null);
        obj.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
    }

    public virtual void Kill(int damage)
    {
        if (killed) return;
        killed = true;
        StartCoroutine(Fall());
    }
    protected bool killed = false;
    IEnumerator Fall()
    {
        GetComponent<Rigidbody2D>().simulated = true;
        Vector2 dir = getDirection();
        dir.Normalize();
        GetComponent<Rigidbody2D>().AddForce(dir * 12f);
        int max = Random.Range(3, 5);
        for (int i = 0; i < max; i++)
        {
            float angle = Random.Range(-1f, 1f);
            Vector2 ranDir = new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle),
                dir.y * Mathf.Cos(angle) + dir.x * Mathf.Sin(angle));
            SpawnBlood(ranDir * 10f);
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
