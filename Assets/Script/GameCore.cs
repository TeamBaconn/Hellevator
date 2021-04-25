using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameCore : MonoBehaviour
{
    public static GameCore instance;

    public GameObject templatePrefab;
    public Vector2 location;

    public List<Sprite> templates;

    public float score = 0;
    
    public float scoreMax = 60;

    public float energy = 2f;
    public float energyMax = 4f;

    public bool over = false;

    private RectTransform energyBar;
    void Start()
    {
        instance = this;
        StartCoroutine(spawnTemplate());
        energyBar = GameObject.Find("Energy").GetComponent<RectTransform>();
    }

    void Update()
    {
        energy -= Time.deltaTime;
        score += getSpeed() * Time.deltaTime;
        energyBar.anchoredPosition = new Vector3(-35,
            Mathf.Lerp(-18,-254,score/scoreMax),0);
        if (energy < 0) energy = 0;
    }

    public float getSpeed()
    {
        return energy / energyMax;
    }

    public float getProgress()
    {
        return score / scoreMax;
    }

    public void addEnergy()
    {
        energy += 5f;
        if (energy > energyMax) energy = energyMax;
    }

    public void castSpell()
    {
        int k = 1;
        Enemy[] enemy = FindObjectsOfType<Enemy>();
        k += enemy.Length*3/4;
        for (int i = 0; i < (k > enemy.Length ? enemy.Length : k); i++) enemy[i].Kill();
    }

    IEnumerator spawnTemplate()
    {
        while (!over)
        {
            Template temp = Instantiate(templatePrefab, location, Quaternion.identity, transform).GetComponent<Template>();
            temp.GetComponent<SpriteRenderer>().sprite = templates[Random.Range(0, templates.Count)];
            temp.Init();
            while (FindObjectOfType<Template>() != null) yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(2.5f - (1-getSpeed())*1.5f);
        }
    }
    public GameObject GameOverPrefab;
    public void GameOver()
    {
        over = true;
        Template prev = FindObjectOfType<Template>();
        if (prev != null) Destroy(prev.gameObject);
        Instantiate(GameOverPrefab, location, Quaternion.identity, null).GetComponent<GameoverText>().Init();
    }
}
