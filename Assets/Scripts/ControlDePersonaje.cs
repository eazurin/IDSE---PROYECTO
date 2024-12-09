using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControlDePersonaje : MonoBehaviour
{
    private Rigidbody rigidbodyComponent;
    private Transform transformComponent;

    private AudioSource audioSourcePropulsor;
    private AudioSource audioSourceRecoleccion;

    public AudioClip sonidoPropulsor;
    public AudioClip sonidoCombustible;
    public AudioClip sonidoRecurso;

    private int combustibleRecolectado = 0;
    private int recursosRecolectados = 0;

    private int cantidadCombustibles;
    public float velocidadPropulsion = 200f;
    private int cantidadRecursos;

    private HashSet<GameObject> objetosProcesados = new HashSet<GameObject>();

    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        transformComponent = GetComponent<Transform>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            audioSourcePropulsor = gameObject.AddComponent<AudioSource>();
            audioSourceRecoleccion = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSourcePropulsor = audioSources[0];
            audioSourceRecoleccion = audioSources[1];
        }

        if (sonidoPropulsor != null)
        {
            audioSourcePropulsor.clip = sonidoPropulsor;
            audioSourcePropulsor.loop = true;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un AudioClip para el propulsor.");
        }

        ObtenerCantidadMateriales();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        combustibleRecolectado = 0;
        recursosRecolectados = 0;
        objetosProcesados.Clear();

        ObtenerCantidadMateriales();
    }

    void ObtenerCantidadMateriales()
    {
        GeneradorObjetos generador = FindObjectOfType<GeneradorObjetos>();
        if (generador != null)
        {
            cantidadCombustibles = generador.objetosCombustiblesGenerados;
            cantidadRecursos = generador.objetosRecursosGenerados;
        }
        else
        {
            cantidadCombustibles = 0;
            cantidadRecursos = 0;
            Debug.LogWarning("No se encontró el script GeneradorObjetos en la escena.");
        }
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
            rigidbodyComponent.AddRelativeForce(Vector3.up * velocidadPropulsion * Time.deltaTime);
            if (!audioSourcePropulsor.isPlaying)
            {
                audioSourcePropulsor.Play();
            }
        }
        else
        {
            if (audioSourcePropulsor.isPlaying)
            {
                audioSourcePropulsor.Stop();
            }
        }
    }

    private void Rotacion()
    {
        if (Input.GetKey(KeyCode.D))
        {
            var rotarDerecha = transformComponent.rotation;
            rotarDerecha.z -= Time.deltaTime * 1;
            transformComponent.rotation = rotarDerecha;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            var rotarIzquierda = transformComponent.rotation;
            rotarIzquierda.z += Time.deltaTime * 1;
            transformComponent.rotation = rotarIzquierda;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objetosProcesados.Contains(other.gameObject))
            return;

        switch (other.gameObject.tag)
        {
            case "Combustible":
                RecolectarCombustible(other.gameObject);
                break;

            case "Recurso":
                RecolectarRecurso(other.gameObject);
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ColisionPeligrosa":
                ManejarColisionPeligrosa();
                break;

            case "ColisionSegura":
                ManejarColisionSegura();
                break;

            default:
                Debug.Log("Colisión con un objeto no identificado.");
                break;
        }
    }

    private void RecolectarCombustible(GameObject combustible)
    {
        if (combustible != null && combustible.activeSelf)
        {
            combustibleRecolectado++;
            ReproducirSonidoRecoleccion(sonidoCombustible);
            Debug.Log($"Combustible recolectado: {combustibleRecolectado}");
            ProcesarObjetoRecolectable(combustible);
        }
    }

    private void RecolectarRecurso(GameObject recurso)
    {
        if (recurso != null && recurso.activeSelf)
        {
            recursosRecolectados++;
            ReproducirSonidoRecoleccion(sonidoRecurso);
            Debug.Log($"Recursos recolectados: {recursosRecolectados}");
            ProcesarObjetoRecolectable(recurso);
        }
    }

    private void ManejarColisionPeligrosa()
    {
        Debug.Log("¡Colisión peligrosa! Reiniciando nivel...");
        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nivelActual);
    }

    private void ManejarColisionSegura()
    {
        int nivelActual = SceneManager.GetActiveScene().buildIndex;

        if (combustibleRecolectado >= cantidadCombustibles && recursosRecolectados >= cantidadRecursos)
        {
            Debug.Log($"Requisitos cumplidos en el nivel {nivelActual}! Avanzando al siguiente nivel...");

            if (nivelActual + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nivelActual + 1);
            }
            else
            {
                SceneManager.LoadScene(1); 
            }
        }
        else
        {
            Debug.Log($"Requisitos no cumplidos: {combustibleRecolectado}/{cantidadCombustibles} combustibles, {recursosRecolectados}/{cantidadRecursos} recursos.");
        }
    }

    private void ProcesarObjetoRecolectable(GameObject objeto)
    {
        objetosProcesados.Add(objeto);
        objeto.SetActive(false);
        Destroy(objeto, 0.1f);
    }

    private void ReproducirSonidoRecoleccion(AudioClip clip)
    {
        if (clip != null && audioSourceRecoleccion != null)
        {
            audioSourceRecoleccion.PlayOneShot(clip);
        }
    }
}