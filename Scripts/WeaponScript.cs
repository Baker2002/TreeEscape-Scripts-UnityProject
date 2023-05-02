using System;
using UnityEngine;
using static Rarity;

public class WeaponScript : MonoBehaviour
{
    public GameObject bullet;
    public Vector3 mousePosition;
    public Vector3 mousePosition2;
    public int damage;
    public WaveSystem wave;
    public AudioClip shootClip;
    public float timeToShoot;
    public bool equipped;

    private Player player;
    private float timeShoot;
    private Camera _cameraMain;
    
    
    [SerializeField]
    private int dmgBonus;
    [SerializeField]
    private float attSpdBonus;
    [SerializeField]
    private RarityType rarity;
    private RarityStats _stats;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        wave = FindObjectOfType<WaveSystem>();
        _stats = GetRarity();
        
        dmgBonus = _stats.GetDamageBonus();
        attSpdBonus = _stats.GetAttackSpeedBonus();
        rarity = _stats.GetRarityType();
        
        damage += dmgBonus;

        timeShoot /= attSpdBonus;

        _cameraMain = Camera.main;
    }

    private void Update()
    {
        if (equipped)
        {
            timeShoot += Time.deltaTime;
            if (Input.GetMouseButton(0) && timeShoot > timeToShoot)
            {
                Shoot();
                timeShoot = 0;
            }
        }
        
    }
    
    private void Shoot()
    {
        mousePosition = _cameraMain.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);
        Instantiate(bullet,new Vector2(transform.position.x,transform.position.y), transform.rotation);
        AudioSource.PlayClipAtPoint(shootClip, new Vector3(0f,0f));
    }
    
}
