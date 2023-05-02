using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    public int treeLevel;
    public int treeHP = 5;
    public WaveSystem wave;
    public Player player;
    public int waterReq = 1;
    public int sunReq = 0;
    public GameObject GunToSpawn;
    
    public SpriteRenderer treeSprite;
    public SpriteRenderer treeBottom;
    
    public Button nextLevelBtn;

    public BoxCollider2D boxCol;

    public AudioClip treeLevelUp;

    public TextMeshProUGUI treeLevelText;

    
    public GameObject treeHPimage;
    private float _treehpWidthImg = 0.467f;
    public GameObject[] treeHPimgs;

    private void Start()
    {
        wave = GameObject.Find("WaveSystem").GetComponent<WaveSystem>();
        player = GameObject.Find("Player").GetComponent<Player>();
        treeSprite = GetComponent<SpriteRenderer>();
        treeBottom = GameObject.Find("Tree Bottom").GetComponent<SpriteRenderer>();
        boxCol = GetComponentInChildren<BoxCollider2D>();
        treeLevelText = GameObject.Find("TreeLevelText").GetComponent<TextMeshProUGUI>();
        
        treeLevel = 1;
        SetWaterSunRequirements();

        treeLevelText.SetText(treeLevel.ToString());
    }

    private void FixedUpdate()
    {
        
        treeHPimage.GetComponent<SpriteRenderer>().size = new Vector2(_treehpWidthImg * treeHP, 0.66f);
       
    }

    public void TreeGrow()
    {
        if (player.waterCount >= waterReq && player.sunCount >= sunReq)
        {
            transform.position += new Vector3(0, 0.25f);
            transform.localScale += new Vector3(0, 0.5f);
            //boxCol.size -= new Vector2(0, 0.05f);
            player.waterCount -= waterReq;
            player.sunCount -= sunReq;
            treeLevel += 1;
            treeLevelText.SetText(treeLevel.ToString());
            StartCoroutine(TreeLevelUp());
            SetWaterSunRequirements();
            
            if (treeLevel >= 5)
            {
                nextLevelBtn.gameObject.SetActive(true);
            }

        }
        CheckWin();
    }
    
    public void LoadNextLevel()
    {
        if (treeLevel >= 5)
        {
            wave.level++;
            wave.waveNum = 1;
            treeLevel = 1;
            treeLevelText.SetText(treeLevel.ToString());
            nextLevelBtn.gameObject.SetActive(false);
            SetWaterSunRequirements();

            StartCoroutine(TreeLevelUpGrow());


        }
        
    }

    private void SetWaterSunRequirements()
    {
        waterReq = (treeLevel + 2) * wave.level;
        sunReq = treeLevel + wave.level;
    }
    
    public void TreeShrink()
    {
        
        transform.position -= new Vector3(0, 0.05f);
        transform.localScale -= new Vector3(0, 0.1f);
        //boxCol.size += new Vector2(0, 0.01f);
        treeHP--;
        StartCoroutine(TreeHurt());
    }

    public void IncreaseHP()
    {
        if (player.waterCount > 9)
        {
            if (treeHP < 5)
            {
               treeHP++;
                player.waterCount -= 10; 
            }
            else
            {
                print("Health is at max(5)");
            }
            
        }
    }

    public void CheckWin()
    {
        if (treeHP <= 0)
        {
            wave.gameObject.SetActive(false);
        }
    }
    
    
    public void BuyGun()
    {
        Instantiate(GunToSpawn, new Vector3(-3f, 0f), quaternion.identity);
    }

    IEnumerator TreeLevelUpGrow()
    {
        
        AudioSource.PlayClipAtPoint(treeLevelUp, new Vector3(0f,0f));
        
        for (int i = 0; i < 20; i++)
        {
            transform.position += new Vector3(0, 0.02f);
            transform.localScale += new Vector3(0, 0.04f);
            StartCoroutine(TreeLevelUp());
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 60; i++)
        {
            transform.position -= new Vector3(0, 0.02f);
            transform.localScale -= new Vector3(0, 0.04f);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(0f, 1.75f, 0f);
        transform.localScale = new Vector3(0.5f, 2.5f,1f);
        yield break;
    }
    
    IEnumerator TreeLevelUp()
    {
        treeSprite.color = new Color(0.07357506f, 1f, 0.1f,1f);
        treeBottom.color = new Color(0.07357506f, 1f, 0.1f,1f);
        yield return new WaitForSeconds(0.075f);
        treeSprite.color = new Color(0.07357506f, 0.5188679f, 0f,1f);
        treeBottom.color = new Color(0.07357506f, 0.5188679f, 0f,1f);
    }
    
    IEnumerator TreeHurt()
    {
        treeSprite.color = new Color(.6f, 0f, 0f, .9f);
        treeBottom.color = new Color(.6f, 0f, 0f, .9f);
        yield return new WaitForSeconds(0.075f);
        treeSprite.color = new Color(0.07357506f, 0.5188679f, 0f,1f);
        treeBottom.color = new Color(0.07357506f, 0.5188679f, 0f,1f);
    }
}
