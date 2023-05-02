using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class WaveSystem : MonoBehaviour
{
    public int level = 1;
    public int waveNum = 1;
    //public GameObject spawnPoints;
    public float time;
    public float waveTime = 12f;

    [FormerlySerializedAs("Text")] 
    public TextMeshProUGUI text;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI waveTimeLeft;
    public Tree tree;
    public bool nextWave;
    
    // ENEMIES
    public GameObject enemy;
    public GameObject speedsterEnemy;
    public GameObject waterDropEnemy;
    // ENEMIES
    
    private void Start()
    {
        text = GameObject.Find("WaveText").GetComponentInChildren<TextMeshProUGUI>();
        text.gameObject.SetActive(false);
        tree = GameObject.FindObjectOfType<Tree>();
        waveTimeLeft = GameObject.Find("timeUntilNextWaveText").GetComponent<TextMeshProUGUI>();

    }

    
    
    private void FixedUpdate()
    {
        if (level > 10)
        {
            text.SetText($"LEVEL 10 COMPLETED. YOU WIN");
            text.gameObject.SetActive(true);
            Time.timeScale = 0f;
            gameObject.SetActive(false);
        }

        if (!nextWave)
        {
            if (time < waveTime)
            {
                time += Time.deltaTime;
                levelText.SetText($"Level: {level.ToString()}");
                waveTimeLeft.SetText($"Next wave: {(waveTime - time).ToString("F")}");
            }else
            {
                SendWave();
            } 
        }
        else
        {
            SendWave();
        }
        
        
        
    }

    private void SendWave()
    {
        StartCoroutine(WaveShow());
        if (level <= 1)
        {
            SetWaveTime(waveTime,0f);
            SpawnEnemy(4, enemy, -10, 10, -6, -10, waveNum);
            StartCoroutine(WaitThanSpawn(3, 1, enemy, -10, 10, -8, -10, waveNum));
        }
        else if (level <= 2)
        {
            SetWaveTime(waveTime,.2f);
            SpawnEnemy(5, enemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(3, speedsterEnemy, -10, 10, -6, -10, waveNum);
            StartCoroutine(WaitThanSpawn(4, 6, enemy, -10, 10, -6, -10, waveNum));
        }
        else if (level <= 3)
        {
            SetWaveTime(waveTime,.2f);
            SpawnEnemy(6, enemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(4, speedsterEnemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(1, waterDropEnemy, -10,10,-7,-12,waveNum);
            StartCoroutine(WaitThanSpawn(4, 4, enemy, 12, 15, -6, 6, waveNum));
        }
        else if (level <= 4)
        {
            SetWaveTime(waveTime,.2f);
            SpawnEnemy(6, enemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(5, speedsterEnemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(2, waterDropEnemy, -10,10,-7,-12,waveNum);
            StartCoroutine(WaitThanSpawn(4, 4, enemy, 12, 15, -6, 6, waveNum));
            StartCoroutine(WaitThanSpawn(1, 4, enemy, -12, -15, -6, 6, waveNum));
        }
        else if (level <= 10)
        {
            SetWaveTime(waveTime,.3f);
            SpawnEnemy(6, enemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(6, speedsterEnemy, -10, 10, -6, -10, waveNum);
            SpawnEnemy(3, waterDropEnemy, -10,10,-7,-12,waveNum);
            StartCoroutine(WaitThanSpawn(4, 5, enemy, 12, 15, -6, 6, waveNum));
            StartCoroutine(WaitThanSpawn(2, 5, enemy, -12, -15, -6, 6, waveNum));
        }

        text.SetText($"Wave {waveNum.ToString()}");
        nextWave = false;
        time = 0;
        waveNum += 1;
    }

    void SetWaveTime(float defaultWaveTime,float add)
    {
        waveTime = defaultWaveTime + add;
    }
    public void SendNextWave()
    {
        nextWave = true;
    }
    
    private void SpawnEnemy(int numOfEnemies, GameObject toSpawn, int xRange1, int xRange2, int yRange1, int yRange2, int waveNumber)
    {
        switch (waveNumber)
        {
            case > 0 and < 7:
                for (int i = 0; i < numOfEnemies; i++)
                {
                    Instantiate(toSpawn, new Vector3(Random.Range(xRange1, xRange2), Random.Range(yRange1, yRange2), 0),
                        Quaternion.identity);
                }
                break;
            case >= 7 and <= 12:
                for (int i = 0; i < numOfEnemies + 1; i++)
                {
                    Instantiate(toSpawn, new Vector3(Random.Range(xRange1, xRange2), Random.Range(yRange1, yRange2), 0),
                        Quaternion.identity);
                }
                break;
            case > 12:
                for (int i = 0; i < numOfEnemies + 2; i++)
                {
                    Instantiate(toSpawn, new Vector3(Random.Range(xRange1, xRange2), Random.Range(yRange1, yRange2), 0),
                        Quaternion.identity);
                }
                break;
            
            default:
                print("nije nista spawnao");
                break;

        }
    }

    IEnumerator WaitThanSpawn(int seconds, int numOfEnemies2, GameObject toSpawn2, int xRange11, int xRange22, int yRange11, int yRange22, int waveNumber2)
    {
        yield return new WaitForSeconds(seconds);
        SpawnEnemy(numOfEnemies2, toSpawn2, xRange11, xRange22, yRange11, yRange22, waveNumber2);
    }
    
    IEnumerator WaveShow()
    {
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        text.gameObject.SetActive(false);
    }
    
}