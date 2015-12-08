using UnityEngine;
using System.Collections;
//Hi
public class EnemyHealth : MonoBehaviour {

    public float health = 3;

    void ApplyDamage(float damage)
    {
        health -= 1;
        
        if(health<=0)
        {
            Destroy(gameObject);
        }
    }
}
