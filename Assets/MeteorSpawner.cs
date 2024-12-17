using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Configuraci�n de Meteoritos")]
    public GameObject meteorPrefab; // Prefab del meteorito grande
    public int meteorCount = 5; // N�mero de meteoritos a generar
    public float spawnHeight = 10f; // Altura desde la que caen los meteoritos
    public float spawnDelay = 1f; // Retraso antes de empezar a generar meteoritos

    [Header("Rango de Generaci�n")]
    public float spawnXMin = -65f; // L�mite izquierdo
    public float spawnXMax = -39f; // L�mite derecho
    public float fixedSpawnZ = -2f; // Profundidad fija para los meteoritos

    // M�todo p�blico para iniciar la generaci�n
    public void StartSpawning()
    {
        StartCoroutine(SpawnMeteoritesWithDelay());
    }

    private IEnumerator SpawnMeteoritesWithDelay()
    {
        Debug.Log($"Esperando {spawnDelay} segundos antes de generar meteoritos.");
        yield return new WaitForSeconds(spawnDelay);

        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnXMin, spawnXMax),
                spawnHeight,
                fixedSpawnZ
            );

            Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        }

        Debug.Log("Generaci�n de meteoritos completada.");
    }
}
