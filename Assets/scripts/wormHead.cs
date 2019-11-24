using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormHead : MonoBehaviour
{
    [SerializeField]
    private wormMovement movement;

    public spawnObject SO;

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Food")
        {
            movement.addBodyPart();

            Destroy(col.gameObject);

            SO.spawnFood();
        }

        else
            if(col.transform != movement.BodyPart[1] && movement.IsAlive)
            
                if(Time.time - movement.TimeFromLastRetry > 5)
                    movement.DIE();
                
            
        
    }
}
