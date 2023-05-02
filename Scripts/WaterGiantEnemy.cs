using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class WaterGiantEnemy : MonoBehaviour
{



    public GameObject tree;
    public GameObject waterDrop;
    public Vector2 treePos;
    public Vector2 pos;
    
    public int health;
    public float speed;
    
    
    
    
    private void Start()
    {
        tree = GameObject.Find("TreeBranch");
        speed = .5f;
        health = 4;
        treePos = tree.transform.position;
        StartCoroutine(WaterDrops(3, waterDrop));
    }

    private void FixedUpdate()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, treePos, speed * Time.deltaTime);
        pos = transform.position;
    }


    IEnumerator WaterDrops(int seconds, GameObject item)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            Instantiate(item, transform.position, quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
            health -= col.GetComponent<Bullet>().weaponDamage;
            if (health <= 0)
            {
                Destroy(gameObject);
                Instantiate(waterDrop, new Vector2(pos.x +0.25f, pos.y), quaternion.identity);
                Instantiate(waterDrop, new Vector2(pos.x - 0.25f, pos.y), quaternion.identity);
            }
        }else if (col.CompareTag("Tree"))
        {
            col.GetComponent<Tree>().treeHP -= 2;
            Destroy(gameObject);
        }
        
    }
}
