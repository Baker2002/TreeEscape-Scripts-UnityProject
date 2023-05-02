using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float horizontalInput;
    public float verticalInput;
    public float speed = 3f;
    public int waterCount;
    public int sunCount;
    
    public Text text;
    public Tree tree;
    
    public float screenWidth;
    public float screenHeight;
    public int screenOffset;
    
    public Transform gunPos;
    
    public WeaponScript weapon;
    public GameObject currentWeapon;
    public GameObject equippedWeapon;
    public float timeToShoot = 0.2f;
    public bool StayOnTree;
    public bool StayOnGun;
    public bool canEquip;
    public float GunOnFloorDMG;
    public TextMeshProUGUI upgradeGunCostText;
    public Button gunUpgrade;

    public GameObject treePanel;
    public GameObject GunMenu;
    public TextMeshProUGUI waterCountText;
    public TextMeshProUGUI sunCountText;

    public TextMeshProUGUI waterReq;
    public TextMeshProUGUI sunReq;

    public AudioClip walking_1;
    public AudioClip walking_2;
    public AudioClip walking_3;
    public AudioClip waterPickupAudio;
    public AudioClip sunPickupAudio;

    private AudioClip[] walkingClips;
    private int walkingIndex;

    private bool playingAudio;
    private float time;

    private float timeShoot;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        text.gameObject
            .SetActive(false); //ovdje sam stavio Press "E" text disabled da se ne prikazuje odmah jer se mora kasnije prikazivati

        screenWidth = Screen.width / 2;
        screenHeight = Screen.height / 2; //ovdje dijelim sirinu i visinu ekrana jer krindž unity neke cudne nacine
        //moram koristii da bi mi text pratio igraca jer ne koriste iste mjere (player je na x,y osi od +10 do -10 a ekran zavisi od rezolucije npr. 1920x1080
        setScreenOffset(); //stavio offset na text da nije u istoj visini kao player nego malo iznad da se vidi

        gunPos = gameObject.GetComponentInChildren<Transform>();
        waterCountText = GameObject.Find("PlayerWater").GetComponent<TextMeshProUGUI>();
        sunCountText = GameObject.Find("PlayerSun").GetComponent<TextMeshProUGUI>();
        
        
        waterCountText.SetText($"Water Count:   {waterCount}");
        sunCountText.SetText($"Sun Count:   {sunCount}");

        text = FindObjectOfType<Text>(true);
           
            
        walkingClips = new[] { walking_1, walking_2, walking_3 };
        
        
    }
    
    

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * speed; //dobijam horizontal input od 1 do -1
        verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * speed; // isto ali za vertical
        MovementCorrection(); //ogranicavam speed kada pritisne u isto vrijeme npr. gore i desno jer se vrijednosti nadodaju pa bude brzi nego inace
        transform.position += new Vector3(horizontalInput, verticalInput, 0); //assignam vrijednosti na position playera
        time += Time.deltaTime;
        if (horizontalInput > 0f || verticalInput > 0f || horizontalInput < 0f || verticalInput < 0f)
        {
            StartCoroutine(WalkingAudio());
        }

        if (StayOnGun)
        {
            //ShowGunMenu();
        }


        waterCountText.SetText($"Water Count:   {waterCount}");
        sunCountText.SetText($"Sun Count:   {sunCount}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (StayOnGun)
            {
                canEquip = true;
            }
            else
            {
                canEquip = false;
            }
        }
        
        
        ShowTreePanel();
    }

    private void ShowTreePanel()
    {
        if (StayOnTree)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (treePanel.gameObject.activeInHierarchy)
                {
                    treePanel.gameObject.SetActive(false);
                }
                else
                {
                    treePanel.gameObject.SetActive(true);
                }
            }
        }
    }

    public void GunUpgrade()
    {
        int cost = equippedWeapon.GetComponent<WeaponScript>().damage * 2;
        if (sunCount >= cost)
        {
            sunCount -= cost;
            equippedWeapon.GetComponent<WeaponScript>().damage += 1;
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Droplet"))
        {
            waterCount++;
            waterCountText.SetText($"Water Count:   {waterCount}");
            Destroy(col.gameObject);
            AudioSource.PlayClipAtPoint(waterPickupAudio,transform.position, 2f);
        }
        else if (col.CompareTag("Sun"))
        {
            sunCount++;
            sunCountText.SetText($"Sun Count:   {sunCount}");
            Destroy(col.gameObject);
            AudioSource.PlayClipAtPoint(sunPickupAudio,transform.position,1f);
        }

       
    }

    private void OnTriggerStay2D(Collider2D col2)
    {
        if (col2.CompareTag("Gun"))
        {
            StayOnGun = true; 
            GunOnFloorDMG = col2.GetComponent<WeaponScript>().damage;
            GunMenu.GetComponentInChildren<TextMeshProUGUI>().SetText($"Damage: {GunOnFloorDMG}");
            GunMenu.gameObject.SetActive(StayOnGun);
        }
        
        if (col2.CompareTag("Tree"))
        {
            screenWidth = Screen.width / 2;
            screenHeight = Screen.height / 2;
            text.gameObject.SetActive(true);
            text.gameObject.transform.position =
                new Vector3(((transform.position.x * (screenHeight / 5)) + screenWidth),
                    (transform.position.y * (screenHeight / 5)) + screenHeight + screenOffset);
            StayOnTree = true;
            waterReq.SetText($"Water Needed      {tree.waterReq}");
            sunReq.SetText($"Sun Needed      {tree.sunReq}");
            if (equippedWeapon)
            {
                upgradeGunCostText.SetText($"Cost to upgrade: {equippedWeapon.GetComponent<WeaponScript>().damage * 2}");
            }
            
        }
        
        if (canEquip)
        {
            if (GetComponentInChildren<WeaponScript>())
            {
                weapon = transform.GetComponentInChildren<WeaponScript>();
                currentWeapon = weapon.gameObject;
                Destroy(currentWeapon);
                weapon = col2.transform.GetComponent<WeaponScript>();
                col2.transform.position = gunPos.transform.position;
                col2.transform.SetParent(gunPos);
                col2.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                equippedWeapon = col2.gameObject;
                canEquip = false;
                timeToShoot = col2.gameObject.GetComponent<WeaponScript>().timeToShoot;
                col2.GetComponent<WeaponScript>().equipped = true;
            }
            else
            {
                weapon = col2.transform.GetComponent<WeaponScript>();
                col2.transform.position = gunPos.transform.position;
                col2.transform.SetParent(gunPos);
                col2.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                equippedWeapon = col2.gameObject;
                canEquip = false;
                timeToShoot = col2.gameObject.GetComponent<WeaponScript>().timeToShoot;
                col2.GetComponent<WeaponScript>().equipped = true;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tree"))
        {
            text.gameObject.SetActive(false);
            treePanel.gameObject.SetActive(false);
            StayOnTree = false;
        }

        if (other.CompareTag("Gun"))
        {
            StayOnGun = false;
            GunMenu.gameObject.SetActive(StayOnGun);
        }
    }
    private void MovementCorrection()
    {
        
        if (horizontalInput > .05f && verticalInput > .05f)
        {
            StartCoroutine(MovementCorrectionIE());
        }
        else if (horizontalInput < -.05f && verticalInput < -.05f)
        {
            StartCoroutine(MovementCorrectionIE());
        }
        else if (horizontalInput > .05f && verticalInput < -.05f)
        {
            StartCoroutine(MovementCorrectionIE());
        }
        else if (horizontalInput < -.05f && verticalInput > .05f)
        {
            StartCoroutine(MovementCorrectionIE());
        }
    }


    IEnumerator MovementCorrectionIE()
    {
        horizontalInput = horizontalInput / 4 * 3;
        verticalInput = verticalInput / 4 * 3;
        yield break;
    }
    
    IEnumerator WalkingAudio()
    {
        
        
        if (time > 0.6f)
        {
            time = 0;
            playingAudio = false;
        }
        
        if (!playingAudio)
        {
            AudioSource.PlayClipAtPoint(walkingClips[walkingIndex],new Vector3(0f,0f), 0.25f);
            walkingIndex++;
            if (walkingIndex > 2)
            {
                walkingIndex = 0;
            }
            playingAudio = true;
        }
        yield break;
    }
    
    
    private void setScreenOffset()
    {
        if (screenHeight > 500)
        {
            screenOffset = 60;
        }
        else
        {
            screenOffset = Screen.height / 20;
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Level1");
    }

}