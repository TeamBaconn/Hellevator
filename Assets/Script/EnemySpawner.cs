using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spikePrefab;
    public GameObject upspikePrefab;
    public GameObject signalPrefab;
    public GameObject warnPrefab;

    public bool enable = true;

    public float offset_x = 2;

    public float offset_y = 5;

    private float nextSpawn;
    private float nextSpawnSpike;
    private float nextSpawnUpSpike;

    public GameObject stage2;

    public GameObject stage3;

    public GameObject healthBar;

    public int health = 20;

    public List<Sprite> hands;

    void Start()
    {
        nextSpawn = 4 + Random.Range(0, 4f);
        nextSpawnSpike = 8 + Random.Range(0, 4f);
        nextSpawnUpSpike = 8 + Random.Range(0, 4f);
    }

    void Update()
    {
        if (!enable) return;
        if (nextSpawn <= 0)
        {
            StartCoroutine(spawnEnemy());
            nextSpawn = 4 + Random.Range(0, 4f) - 4 * (1 - GameCore.instance.getSpeed()) + 4 * GameCore.instance.getProgress();
        }
        if (nextSpawnSpike <= 0)
        {
            StartCoroutine(spawnSpike());
            nextSpawnSpike = 8 + Random.Range(0, 4f);
        }
        if (nextSpawnUpSpike <= 0)
        {
            StartCoroutine(spawnUpSpike());
            nextSpawnUpSpike = 8 + Random.Range(0, 4f);
        }
        if (GameCore.instance.getProgress() > 0.28f)
        {
            if (stage2 != null)
            {
                Instantiate(stage2).gameObject.name = "Stage2Boss";
                stage2 = null;
            }
        }
        if (GameCore.instance.getProgress() > 0.66f)
        {
            if (stage3 != null)
            {
                Instantiate(stage3);
                stage3 = null;
                SpikeMovement move = GameObject.Find("Stage2Boss").GetComponent<SpikeMovement>();
                move.y = 15;
                move.max_y = 5;
                move.destroy = true;
                healthBar.SetActive(true);
                GameObject.Find("Depth").SetActive(false);
                StartCoroutine(startHealth());
            }
        }
        if (GameCore.instance.getProgress() > 0.70f)
            nextSpawnUpSpike -= Time.deltaTime;
        if (GameCore.instance.getProgress() > 0.33f)
            nextSpawnSpike -= Time.deltaTime;
        nextSpawn -= Time.deltaTime;
    }
    IEnumerator startHealth()
    {
        Image img = GameObject.Find("Health").GetComponent<Image>();
        for(float i = 0; i <= 1; i += 0.2f)
        {
            img.fillAmount = i;
            CameraMovement.instance.Shake();
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator spawnEnemy()
    {
        int minusx = Random.Range(0, 2) == 0 ? -1 : 1;
        int minusy = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 des = transform.position + new Vector3(offset_x * minusx, Random.Range(0, offset_y) * minusy);
        GameObject sig = Instantiate(signalPrefab, des, Quaternion.identity, transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(sig);
        GameObject obj = Instantiate(enemyPrefab, des, Quaternion.identity, transform);
        obj.transform.localScale = new Vector3(1, minusx * -1, 1);
        obj.GetComponent<SpriteRenderer>().sprite = hands[Random.Range(0, hands.Count)];
    }
    IEnumerator spawnSpike()
    {
        int minusx = Random.Range(0, 2) == 0 ? -1 : 1;
        int minusy = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 des = transform.position + new Vector3(offset_x * minusx, Random.Range(0, offset_y) * minusy + 2*GameCore.instance.getProgress());
        GameObject sig = Instantiate(warnPrefab, des, Quaternion.identity, transform);
        yield return new WaitForSeconds(3f + GameCore.instance.getProgress() * 6);
        Vector2 desSig = sig.transform.position;
        Destroy(sig);
        GameObject obj = Instantiate(spikePrefab, desSig, Quaternion.identity, transform);
        obj.transform.localScale = new Vector3(minusx * -1, 1, 1);
    }
    IEnumerator spawnUpSpike()
    {
        int minusx = Random.Range(0, 2) == 0 ? -1 : 1;
        int minusy = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 des = transform.position + new Vector3(Random.Range(0, offset_y) * minusy, offset_x * minusx);
        GameObject sig = Instantiate(signalPrefab, des, Quaternion.identity, transform);
        yield return new WaitForSeconds(3f);
        Vector2 desSig = sig.transform.position;
        Destroy(sig);
        GameObject obj = Instantiate(upspikePrefab, desSig, Quaternion.identity, transform);
        obj.transform.localScale = new Vector3(1, minusx*-1, 1);
    }
}
