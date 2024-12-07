using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDePersonaje : MonoBehaviour
{
    Rigidbody rigidbody; 
    Transform transform;
    AudioSource audiosource;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); 
        transform = GetComponent<Transform>();
        audiosource = GetComponent<AudioSource>();
    }

   
    void Update()
    {
        ProcesarInput();
    }

    private void ProcesarInput()
    {
        Propulsion();

        Rotacion();
    }

    private void Propulsion()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // print("Propulsor...");
            rigidbody.AddRelativeForce(Vector3.up);

            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
        else
        {
            audiosource.Stop();
        }
    }

    private void Rotacion()
    {
        if (Input.GetKey(KeyCode.D))
        {
            var rotarDerecha = transform.rotation;
            rotarDerecha.z -= Time.deltaTime *1;
            transform.rotation = rotarDerecha;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            var rotarIzquierda = transform.rotation;
            rotarIzquierda.z += Time.deltaTime *1;
            transform.rotation = rotarIzquierda;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
        case "ColisionSegura":
            print("Bien jugado!");
            break;

        case "ColisionPeligrosa":
            print("Muerto!");
            break;
        }
    
    }

}

