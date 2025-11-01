using System.Runtime.CompilerServices;
using UnityEngine;

public class AddAbilty :  MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private bool  hascolider = false;
    void OnCollisionEnter(Collision collsiion)
    {
        if (hascolider) return;
        if(collsiion.gameObject.CompareTag("Player"))
        {
            Debug.Log("충돌");
            hascolider = true;
            PlayerMove move = collsiion.gameObject.GetComponent<PlayerMove>();
            if (move)
            {
                move.speed += 20;
                Debug.Log($"스피드증가{move.speed}");
            }
            Destroy(gameObject);
            Debug.Log("실행됨");

        }
        
    }
}
