using System.Collections.Generic;
using UnityEngine;

public class GeneradorObjetos : MonoBehaviour
{
    public GameObject combustiblePrefab;
    public GameObject recursoPrefab;
    public Vector3 areaGeneracionMin;
    public Vector3 areaGeneracionMax;
    public int cantidadCombustibles = 5;
    public int cantidadRecursos = 5;
    public float distanciaMinima = 2f;

    public int objetosCombustiblesGenerados { get; private set; }
    public int objetosRecursosGenerados { get; private set; }

    private List<Vector3> posicionesGeneradas = new List<Vector3>();

    void Awake()
    {
        GenerarObjetosAleatorios();
    }

    void Start()
    {
        // Otros inicializadores si es necesario
    }

    private void GenerarObjetosAleatorios()
    {
        objetosCombustiblesGenerados = GenerarPrefabs(combustiblePrefab, cantidadCombustibles);
        objetosRecursosGenerados = GenerarPrefabs(recursoPrefab, cantidadRecursos);
    }

    private int GenerarPrefabs(GameObject prefab, int cantidad)
    {
        int objetosGenerados = 0;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 posicionAleatoria;
            int intentos = 0;

            do
            {
                posicionAleatoria = new Vector3(
                    Random.Range(areaGeneracionMin.x, areaGeneracionMax.x),
                    Random.Range(areaGeneracionMin.y, areaGeneracionMax.y),
                    Random.Range(areaGeneracionMin.z, areaGeneracionMax.z)
                );
                intentos++;
            } while (!EsPosicionValida(posicionAleatoria) && intentos < 100);

            if (intentos < 100)
            {
                posicionesGeneradas.Add(posicionAleatoria);
                Instantiate(prefab, posicionAleatoria, Quaternion.identity);
                objetosGenerados++;
            }
            else
            {
                Debug.LogWarning("No se pudo encontrar una posición válida después de 100 intentos.");
            }
        }

        return objetosGenerados;
    }

    private bool EsPosicionValida(Vector3 nuevaPosicion)
    {
        foreach (Vector3 posicion in posicionesGeneradas)
        {
            if (Vector3.Distance(posicion, nuevaPosicion) < distanciaMinima)
            {
                return false;
            }
        }
        return true;
    }
}