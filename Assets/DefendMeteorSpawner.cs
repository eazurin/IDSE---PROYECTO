using UnityEngine;

public class DefendMeteorSpawner : StateMachineBehaviour
{
    private MeteorSpawner spawner; // Referencia al spawner

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entrando al estado Defend");

        // Busca el objeto con el script MeteorSpawner en la escena
        if (spawner == null)
        {
            spawner = GameObject.FindObjectOfType<MeteorSpawner>();
        }

        // Inicia la generación de meteoritos
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
        else
        {
            Debug.LogError("No se encontró un MeteorSpawner en la escena. Asegúrate de crearlo.");
        }
    }
}
