using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 target;
    public WeaponScript weapon;
    public GameObject ob;
    public GameObject player;
    public float speed;
    public int secondsBeforeDestroy = 4;
    public float time;
    public int weaponDamage;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        weapon = player.gameObject.GetComponentInChildren<WeaponScript>();
        weaponDamage = weapon.damage;
        
        
        ob = GameObject.FindGameObjectWithTag("Gun");
        target = weapon.mousePosition;
        target.Normalize();
        target /= 10;
        target.x *= speed;
        target.y *= speed;
    }

    private void FixedUpdate()
    {
        transform.Translate(target.x,target.y,target.z);
        time += Time.deltaTime;
        if (time > secondsBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
    
    
}
