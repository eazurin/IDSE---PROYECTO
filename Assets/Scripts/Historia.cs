using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HistoriaController : MonoBehaviour
{
    public string nombreMenuPrincipal = "MenuPrincipal";
    public string nombrePrimerNivel = "Nivel1";
    public string nombreNivelHistoria1 = "Historia1";
    public string nombreNivel4 = "Nivel4";

    // Referencia a tus objetos TextMeshProUGUI
    public TextMeshProUGUI[] textosHistoria;

    private int indiceActual = 0;

    void Start()
    {
        MostrarTexto();
    }

    public void Siguiente()
    {
        if (indiceActual < textosHistoria.Length - 1)
        {
            // Avanzar al siguiente texto si no estamos en el último
            indiceActual++;
            MostrarTexto();
        }
        else
        {
            // Si estamos en el último texto y en la escena "Historia1"
            if (SceneManager.GetActiveScene().name == nombreNivelHistoria1)
            {
                SceneManager.LoadScene(nombreNivel4);
            }
            else
            {
                // Comportamiento original: ir al Nivel1
                SceneManager.LoadScene(nombrePrimerNivel);
            }
        }
    }

    public void Anterior()
    {
        if (indiceActual > 0)
        {
            // Retrocede al texto anterior
            indiceActual--;
            MostrarTexto();
        }
        else
        {
            // Si estamos en el primer texto, regresa al menú principal
            SceneManager.LoadScene(nombreMenuPrincipal);
        }
    }

    void MostrarTexto()
    {
        // Oculta todos los textos primero
        for (int i = 0; i < textosHistoria.Length; i++)
        {
            textosHistoria[i].gameObject.SetActive(false);
        }

        // Asegúrate de mostrar solo el texto actual
        if (indiceActual >= 0 && indiceActual < textosHistoria.Length)
        {
            textosHistoria[indiceActual].gameObject.SetActive(true);
        }
    }
}
