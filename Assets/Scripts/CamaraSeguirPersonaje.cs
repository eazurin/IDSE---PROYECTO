using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraSeguirPersonaje : MonoBehaviour
{
    public Transform personaje;
    public float velocidadSeguir = 5f;
    public float limiteXMin = -15f, limiteXMax = 15f;
    public float limiteYMin = 0f, limiteYMax = 5f;

    void Update()
    {
        // Verifica si el nivel actual es "Nivel2"
        if (SceneManager.GetActiveScene().name == "Nivel3" || SceneManager.GetActiveScene().name == "Nivel4")
        {
            SeguirPersonaje();
        }
    }

    private void SeguirPersonaje()
    {
        // Calcula la posición de la cámara basada en los límites y el personaje
        Vector3 posicionObjetivo = personaje.position;

        float xPos = Mathf.Clamp(posicionObjetivo.x, limiteXMin, limiteXMax);
        float yPos = Mathf.Clamp(posicionObjetivo.y, limiteYMin, limiteYMax);

        Vector3 nuevaPosicion = new Vector3(xPos, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nuevaPosicion, velocidadSeguir * Time.deltaTime);
    }
}
