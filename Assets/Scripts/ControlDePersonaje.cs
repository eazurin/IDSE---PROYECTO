using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlDePersonaje : MonoBehaviour
{
    // Variables generales
    private Rigidbody rigidbody; 
    private Transform transform;
    private AudioSource audiosource;

    // Variables específicas para la escena 1
    private int combustibleRecolectado = 0;
    private int recursosRecolectados = 0;

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
            rotarDerecha.z -= Time.deltaTime * 1;
            transform.rotation = rotarDerecha;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            var rotarIzquierda = transform.rotation;
            rotarIzquierda.z += Time.deltaTime * 1;
            transform.rotation = rotarIzquierda;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Gestión para ambas escenas
        switch (collision.gameObject.tag)
        {
            case "ColisionSegura":
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    SceneManager.LoadScene(1); // Pasar a escena 1
                }
                else if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    // Solo avanzar si se han recolectado los requisitos
                    if (combustibleRecolectado >= 2 && recursosRecolectados >= 1)
                    {
                        Debug.Log("¡Requisitos cumplidos! Pasando a la siguiente escena...");
                        SceneManager.LoadScene(2); // Pasar a la siguiente escena
                    }
                    else
                    {
                        Debug.Log("Aún no has recolectado el combustible y los recursos necesarios.");
                    }
                }
                break;

            case "ColisionPeligrosa":
                // Reiniciar escena actual
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "Combustible":
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    combustibleRecolectado++;
                    Destroy(collision.gameObject); // Eliminar el objeto recolectado
                    Debug.Log($"Combustible recolectado: {combustibleRecolectado}");
                }
                break;

            case "Recurso":
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    recursosRecolectados++;
                    Destroy(collision.gameObject); // Eliminar el objeto recolectado
                    Debug.Log($"Recursos recolectados: {recursosRecolectados}");
                }
                break;

            default:
                Debug.Log("Colisión con un objeto no identificado");
                break;
        }
    }
}
