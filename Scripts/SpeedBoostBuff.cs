using System;
using UnityEngine;

public class SpeedBoostBuff : MonoBehaviour
{
    public CircleCollider2D circleColl;
    public Enemy enemy;


    private void Start()
    {
        circleColl = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().speed = 1.5f;
        }
    }
}
