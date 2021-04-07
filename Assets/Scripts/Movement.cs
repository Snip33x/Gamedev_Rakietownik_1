using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor
    // CACHE - e.g. references for readability or speed
    // STATE - private instance (member) variables

    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem leftBoosterback;
    [SerializeField] ParticleSystem rightBooster;
    [SerializeField] ParticleSystem rightBoosterback;

    Rigidbody rb;
    AudioSource audioSource;

    bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }


    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
        
        ///////////////////// to działa tak samo to włączania i wyłącznia dzwiętku rakiety - moje
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (!audioSource.isPlaying)
        //    {
        //        audioSource.Play();
        //    }
        //}
        //if(Input.GetKeyUp(KeyCode.Space))
        //    audioSource.Stop();
    }


    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust);  //relative force będzie nakładana na podstawie kierunku w którym zwrócowna jest rakieta // Vector3.up is shortcut for 0,1,0
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine); //musi mieć argument
        }
        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //else if żeby nie obracać się w lewo i w prawo w tym samym czasie - będziemy się obracać w lewo jak będą dwa przyciski na raz, można dać ifa, ale wtedy trzeba dzieliś els'y z particlasami
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainBooster.Stop();
    }



    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);

        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
            leftBoosterback.Play();
        }
    }
    private void RotateRight()
    {
        //transform.Rotate(Vector3.back * rotationThrust * Time.deltaTime);   // ten sposób bez tworzenia metody i zamiast znaku ujemnego
        ApplyRotation(-rotationThrust);

        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
            rightBoosterback.Play();
        }
    }
    private void StopRotating()
    {
        rightBooster.Stop();
        rightBoosterback.Stop();
        leftBooster.Stop();
        leftBoosterback.Stop();
    }


    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
