using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public GameObject reward;
    public GameObject sunReward;
    public float hp = 2;
    public Tree tree;
    public int rand;
    public float speed;
    public WaveSystem wave;
    

    public AudioClip onTreeDestroyClip;


    private void Start()
    {
        target = GameObject.Find("Tree");
        tree = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
        wave = FindObjectOfType<WaveSystem>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(target.transform.position.x, target.transform.position.y),speed * Time.deltaTime);
    }

    

    private void OnTriggerEnter2D(Collider2D col4)
    {
        if (col4.gameObject.CompareTag("Bullet"))
        {
            Destroy(col4.gameObject);
            hp -= col4.gameObject.GetComponent<Bullet>().weaponDamage;
            if (hp <= 0)
            {
                Destroy(gameObject);
                rand = Random.Range(0, 6);
                if (rand > 2)
                {
                    Instantiate(reward, transform.position, quaternion.identity);   
                }else if (rand == 1)
                {
                    Instantiate(sunReward, transform.position, quaternion.identity);
                }

                

            }
        }else if (col4.gameObject.CompareTag("Tree"))
        {
            
            tree.TreeShrink();
            tree.CheckWin();
            AudioSource.PlayClipAtPoint(onTreeDestroyClip,new Vector3(0f,0f));
            Destroy(gameObject);
        }
    }

    

    
}
