using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement2D : MonoBehaviour
{
    public enum MovementDirection
    {
        Horizontal,
        Vertical,
        DiagonalDownLeft,
        DiagonalDownRight
    }

    [Header("Movimiento")]
    public MovementDirection direction = MovementDirection.Horizontal; // Dirección del movimiento
    public float speed = 3f; // Velocidad del movimiento
    public float range = 5f; // Rango de movimiento (Horizontal/Vertical)

    [Header("Configuración para Diagonales")]
    public bool isDiagonal = false; // Identifica si el objeto es un asteroide diagonal
    public float extraFallDistance = 2f; // Distancia extra para atravesar la superficie (solo para diagonales)
    public float respawnTime = 3f; // Tiempo para reaparecer después de caer (solo para diagonales)

    private Vector2 startPosition; // Posición inicial
    private Vector2 targetPosition; // Posición objetivo (para diagonales)
    private bool isFalling = false; // Controla si el objeto está en movimiento (diagonales)
    private bool isPaused = false; // Controla si el objeto está en pausa (Horizontal/Vertical)
    private float targetPositionValue; // Posición objetivo para horizontal/vertical
    private int directionMultiplier = 1; // Control de dirección (1: adelante, -1: atrás)

    void Start()
    {
        startPosition = transform.position;

        if (isDiagonal)
        {
            // Configuración inicial para diagonales
            SetDiagonalTargetPosition();
        }
        else
        {
            // Configuración inicial para horizontal/vertical
            SetLinearTargetPosition();
        }
    }

    void Update()
    {
        if (isDiagonal)
        {
            if (!isFalling)
            {
                MoveDiagonal();
            }
        }
        else
        {
            if (!isPaused)
            {
                MoveLinear();
            }
        }
    }

    // Movimiento para objetos diagonales (asteroides)
    void MoveDiagonal()
    {
        Vector2 movement = Vector2.zero;

        if (direction == MovementDirection.DiagonalDownLeft)
        {
            movement = new Vector2(-speed * Time.deltaTime, -speed * Time.deltaTime);
        }
        else if (direction == MovementDirection.DiagonalDownRight)
        {
            movement = new Vector2(speed * Time.deltaTime, -speed * Time.deltaTime);
        }

        transform.position += (Vector3)movement;

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(RespawnDiagonal());
        }
    }

    // Configuración de la posición objetivo para diagonales
    void SetDiagonalTargetPosition()
    {
        if (direction == MovementDirection.DiagonalDownLeft)
        {
            targetPosition = new Vector2(startPosition.x - range, startPosition.y - range - extraFallDistance);
        }
        else if (direction == MovementDirection.DiagonalDownRight)
        {
            targetPosition = new Vector2(startPosition.x + range, startPosition.y - range - extraFallDistance);
        }
    }

    IEnumerator RespawnDiagonal()
    {
        isFalling = true; // Pausa el movimiento
        yield return new WaitForSeconds(respawnTime); // Espera antes de reaparecer
        transform.position = startPosition; // Reaparece en la posición inicial
        isFalling = false; // Reactiva el movimiento
    }

    // Movimiento para objetos horizontales/verticales
    void MoveLinear()
    {
        float currentPosition = (direction == MovementDirection.Horizontal) ? transform.position.x : transform.position.y;
        float start = (direction == MovementDirection.Horizontal) ? startPosition.x : startPosition.y;

        float delta = speed * Time.deltaTime * directionMultiplier;
        float newPosition = currentPosition + delta;

        if (directionMultiplier > 0 && newPosition >= targetPositionValue)
        {
            newPosition = targetPositionValue;
            StartCoroutine(PauseLinearMovement());
            directionMultiplier = -1;
            SetLinearTargetPosition();
        }
        else if (directionMultiplier < 0 && newPosition <= start)
        {
            newPosition = start;
            StartCoroutine(PauseLinearMovement());
            directionMultiplier = 1;
            SetLinearTargetPosition();
        }

        if (direction == MovementDirection.Horizontal)
        {
            transform.position = new Vector2(newPosition, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, newPosition);
        }
    }

    void SetLinearTargetPosition()
    {
        if (direction == MovementDirection.Horizontal)
        {
            targetPositionValue = startPosition.x + (range * directionMultiplier);
        }
        else
        {
            targetPositionValue = startPosition.y + (range * directionMultiplier);
        }
    }

    IEnumerator PauseLinearMovement()
    {
        isPaused = true;
        yield return new WaitForSeconds(1f); // Pausa fija para movimiento horizontal/vertical
        isPaused = false;
    }
}
