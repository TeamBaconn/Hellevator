using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public List<string> tut;
    public Text text;
    public AudioSource lobby;
    public AudioSource gameST;

    public AudioSource ping;
    void Start()
    {
        lobby.Play();
        StartCoroutine(run());
    }
    IEnumerator run()
    {
        while(tut.Count != 0 && GameCore.instance.getProgress() < 0.05)
        {
            text.text = tut[0];
            tut.RemoveAt(0);
            ping.Play();
            yield return new WaitForSeconds(3f);
        }
        lobby.Stop();
        gameST.Play();
        text.text = "";
    }
}
