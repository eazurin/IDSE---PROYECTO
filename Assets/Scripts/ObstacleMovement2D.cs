using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement2D : MonoBehaviour
{
    public enum MovementDirection
    {
        Horizontal,
        Vertical
    }

    [Header("Movimiento")]
    public MovementDirection direction = MovementDirection.Horizontal; // Dirección del movimiento
    public float speed = 3f; // Velocidad del movimiento
    public float range = 5f; // Rango de movimiento

    [Header("Pausa")]
    public float pauseDuration = 1f; // Tiempo de pausa al llegar a un extremo

    private Vector2 startPosition;
    private bool isPaused = false;
    private float targetPosition; // Posición objetivo actual
    private int directionMultiplier = 1; // 1 para adelante, -1 para atrás

    void Start()
    {
        startPosition = transform.position; // Guarda la posición inicial
        SetTargetPosition(); // Establece la posición objetivo inicial según la dirección
    }

    void Update()
    {
        if (isPaused)
            return; // Si está en pausa, no actualizar el movimiento

        // Determinar la posición actual y la posición objetivo según la dirección
        float current = (direction == MovementDirection.Horizontal) ? transform.position.x : transform.position.y;
        float start = (direction == MovementDirection.Horizontal) ? startPosition.x : startPosition.y;

        // Calcular el cambio en la posición
        float delta = speed * Time.deltaTime * directionMultiplier;

        // Mover el objeto hacia la posición objetivo
        float newPosition = current + delta;

        // Verificar si ha alcanzado o superado la posición objetivo
        if (directionMultiplier > 0 && newPosition >= targetPosition)
        {
            newPosition = targetPosition; // Asegurarse de que no sobrepase la posición objetivo
            StartCoroutine(PauseMovement()); // Iniciar la pausa
            directionMultiplier = -1; // Cambiar la dirección
            SetTargetPosition(); // Actualizar la nueva posición objetivo
        }
        else if (directionMultiplier < 0 && newPosition <= start)
        {
            newPosition = start; // Asegurarse de que no sobrepase la posición de inicio
            StartCoroutine(PauseMovement()); // Iniciar la pausa
            directionMultiplier = 1; // Cambiar la dirección
            SetTargetPosition(); // Actualizar la nueva posición objetivo
        }

        // Aplicar la nueva posición dependiendo de la dirección
        if (direction == MovementDirection.Horizontal)
        {
            transform.position = new Vector2(newPosition, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, newPosition);
        }
    }

    // Establecer la posición objetivo dependiendo de la dirección actual
    void SetTargetPosition()
    {
        if (direction == MovementDirection.Horizontal)
        {
            targetPosition = startPosition.x + (range * directionMultiplier);
        }
        else
        {
            targetPosition = startPosition.y + (range * directionMultiplier);
        }
    }

    // Corrutina para pausar el movimiento durante un tiempo
    IEnumerator PauseMovement()
    {
        isPaused = true; // Indicar que el objeto está en pausa
        yield return new WaitForSeconds(pauseDuration); // Esperar el tiempo de pausa
        isPaused = false; // Reanudar el movimiento
    }
}