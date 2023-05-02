using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterGiantEnemy : MonoBehaviour
{
    public GameObject tree;
    public GameObject waterDrop;
    public Vector2 treePos;
    public Vector2 pos;
    public GameObject hpUI;
    private RectTransform hpUISprite;
    [FormerlySerializedAs("health")] public int hp;
    public float speed;
    
    
    
    
    private void Start()
    {
        tree = GameObject.Find("TreeBranch");
        speed = .5f;
        hp = 4;
        treePos = tree.transform.position;
        hpUISprite = hpUI.GetComponent<RectTransform>();
        hpUI.transform.position = new Vector3(transform.position.x , transform.position.y - 0.7f, 0f);
        StartCoroutine(WaterDrops(3, waterDrop));
        
    }

    private void FixedUpdate()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, treePos, speed * Time.deltaTime);
        pos = transform.position;
        
        hpUISprite.localScale = new Vector3(0f, 0.2f);
        
        hpUI.transform.position = new Vector3(transform.position.x , transform.position.y - 0.7f, 0f);
        
        for (int i = hp; i > 0; i--)
        {
            hpUISprite.localScale += new Vector3(0.3f, 0f);
        }
        
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
            hp -= col.GetComponent<Bullet>().weaponDamage;
            if (hp <= 0)
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
