using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float thrustforce = 100f;
    [SerializeField] float rotationforce = 100f;
    [SerializeField] AudioClip engineThrust;
    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem rightBooster;
    [SerializeField] ParticleSystem leftBooster;

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
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();

        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }

    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustforce * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineThrust);
        }
        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainBooster.Stop();
    }

    void StopRotating()
    {
        rightBooster.Stop();
        leftBooster.Stop();
    }

    void RotateLeft()
    {
        ApplyRotation(rotationforce);
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-rotationforce);
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }
    
    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually rotate 
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation so the physics system can take over
    }
}
