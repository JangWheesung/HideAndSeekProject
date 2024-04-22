using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchEnemy : MonoBehaviour
{
    [SerializeField] private GameObject E_Text;
    [SerializeField] private EnemyAgent enemyAgent;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;

    public bool PossibleCatch { get; private set; }

    void Update()
    {
        if (Physics.OverlapSphere(transform.position, range, enemyMask).Length > 0)
        {
            PossibleCatch = true;
            E_Text.SetActive(true);
        }
        else
        {
            PossibleCatch = false;
            E_Text.SetActive(false);
        }

        if (E_Text && Input.GetKeyDown(KeyCode.E))
        {
            enemyAgent.PawnPlayer();

            PossibleCatch = false;
            E_Text.SetActive(false);
        }
    }
}
