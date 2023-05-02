using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpeedster : MonoBehaviour
{
    public GameObject target;
    public GameObject reward;
    public GameObject sunReward;
    public int hp = 1;
    public Tree tree;
    public int rand;
    public float speed;

    public AudioClip onTreeDestroyClip;


    private void Start()
    {
        target = GameObject.Find("Tree");
        tree = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(target.transform.position.x, target.transform.position.y),speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D col4)
    { 
        if (col4.gameObject.CompareTag("Tree"))
        {
            rand = Random.Range(0, 6);
            if (rand > 2)
            {
                Instantiate(sunReward, transform.position, quaternion.identity);   
            }else
            {
                Instantiate(reward, transform.position, quaternion.identity);
            }
            AudioSource.PlayClipAtPoint(onTreeDestroyClip,new Vector3(0f,0f));
            Destroy(gameObject);
        }
    }

    
}