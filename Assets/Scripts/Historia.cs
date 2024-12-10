using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Asegúrate de tener esto

public class HistoriaController : MonoBehaviour
{
    public string nombreMenuPrincipal = "MenuPrincipal";
    public string nombrePrimerNivel = "Nivel1";

    // Referencia a tus objetos TextMeshProUGUI
    // Arrastra Historia_1, Historia_2, Historia_3 desde el inspector.
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
            indiceActual++;
            MostrarTexto();
        }
        else
        {
            // Si estamos en el último texto, al presionar Siguiente pasamos al Nivel1
            SceneManager.LoadScene(nombrePrimerNivel);
        }
    }

    public void Anterior()
    {
        if (indiceActual > 0)
        {
            indiceActual--;
            MostrarTexto();
        }
        else
        {
            // Si estamos en el primer texto y le damos a Anterior, volvemos al Menú Principal
            SceneManager.LoadScene(nombreMenuPrincipal);
        }
    }

    void MostrarTexto()
    {
        // Oculta todos los textos
        for (int i = 0; i < textosHistoria.Length; i++)
        {
            textosHistoria[i].gameObject.SetActive(false);
        }

        // Muestra el texto correspondiente al índice actual
        if (indiceActual >= 0 && indiceActual < textosHistoria.Length)
        {
            textosHistoria[indiceActual].gameObject.SetActive(true);
        }
    }
}
