using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Esta función carga la escena de Historia en vez del nivel directamente
    public void EmpezarHistoria(string nombreHistoria)
    {
        SceneManager.LoadScene(nombreHistoria);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Aquí se cierra la aplicación");
    }
}

