using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float stepSize = 0.75f;
    public float stepDelay = 0.5f;
    private float _stepDelay = 0;

    public int stepOffset = 2;
    public int currentStep = 0;

    private float interactDelay = 1f;

    private Animator anim;

    public AudioSource walk;
    public AudioSource generator;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (transform.parent != null) return;

        anim.SetBool("Back", currentStep == stepOffset);
        interactDelay -= Time.deltaTime;
        if(currentStep == stepOffset && Input.GetKeyDown(KeyCode.Space) && interactDelay <= 0)
        {
            anim.SetTrigger("trigger");
            interactDelay = 1f;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        _stepDelay += Time.deltaTime;
        if(horizontal != 0 && _stepDelay > stepDelay && Mathf.Abs(currentStep+horizontal) <= stepOffset)
        {
            _stepDelay = 0;
            transform.position += new Vector3(horizontal * stepSize, 0);
            transform.localScale = new Vector3(horizontal,1,1);
            currentStep += (int)horizontal;
            walk.Play();
        }
    }

    public void Shake()
    {
        generator.Play();
        CameraMovement.instance.Shake();
        GameCore.instance.addEnergy();
    }
}
