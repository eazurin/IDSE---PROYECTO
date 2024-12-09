using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraSeguirPersonaje : MonoBehaviour
{
    public Transform personaje;
    public float velocidadSeguir = 5f;
    public float limiteXMin = -5f, limiteXMax = 5f;
    public float limiteYMin = 0f, limiteYMax = 5f;

    private Camera camara;

    void Start()
    {
        camara = Camera.main;
    }

    void Update()
    {
        int nivelActual = SceneManager.GetActiveScene().buildIndex;

        if (nivelActual == 2)
        {
            SeguirPersonaje();
        }
    }

    private void SeguirPersonaje()
    {
        Vector3 posicionObjetivo = personaje.position;

        float xPos = Mathf.Clamp(posicionObjetivo.x, limiteXMin, limiteXMax);
        float yPos = Mathf.Clamp(posicionObjetivo.y, limiteYMin, limiteYMax);

        Vector3 nuevaPosicion = new Vector3(xPos, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nuevaPosicion, velocidadSeguir * Time.deltaTime);
    }
}