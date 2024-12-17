using UnityEngine;

public class ObstacleMovement2DSmall : MonoBehaviour
{
    public enum MovementDirection
    {
        DiagonalDownLeft,
        DiagonalDownRight
    }

    [Header("Movimiento del Meteorito Pequeño")]
    public MovementDirection direction = MovementDirection.DiagonalDownLeft;
    public float speed = 4f; // Velocidad del meteorito pequeño
    public float despawnY = -5f; // Coordenada Y donde desaparece el meteorito

    void Update()
    {
        MoveMeteor();
    }

    private void MoveMeteor()
    {
        // Movimiento diagonal hacia abajo
        Vector2 movement = direction == MovementDirection.DiagonalDownLeft
            ? new Vector2(-1, -1)
            : new Vector2(1, -1);

        transform.Translate(movement * speed * Time.deltaTime);

        // Destruir meteorito pequeño al salir del rango
        if (transform.position.y <= despawnY)
        {
            Destroy(gameObject);
        }
    }
}
