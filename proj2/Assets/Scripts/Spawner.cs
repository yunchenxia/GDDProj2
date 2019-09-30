using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    #region SpawnerVar
    [SerializeField]
    [Tooltip("how many enemies max can be spawned during a wave")] 
    public int max_enemy_wave;

    [SerializeField]
    [Tooltip("how many enemies max can be spawned at once outside of a wave")] 
    public int max_enemy_mini;

    [SerializeField]
    [Tooltip("how many enemies max can be living at the same time")] 
    public int max_enemy;

    [SerializeField]
    [Tooltip("how often do enemies spawn normally, spawn_rate = 0.5 is never, spawn_rate = 1 is ~5 times per second")] 
    public float spawn_rate;


    [SerializeField]
    [Tooltip("how far from the spawner can enemies spawn from, where (x1, x2): x1 = left bound, x2 = right bound")]

    public Vector2 x_range;

    [SerializeField]
    [Tooltip("how far from the spawner can enemies spawn from, where (y1, y2): y1 = lower bound, y2 = high bound")]
    public Vector2 y_range;


    [SerializeField]
    [Tooltip("time between waves of enemies")] 
    public float wave_interval;

    [SerializeField]
    [Tooltip("The HUD Controller")]
    private HUDController m_HUD;





    public GameObject TrashManPreFab;

    
    // time left before next wave
    float wave_timer;

    // current number of living enemies
    public int num_enemy;

    int wave_counter;


    #endregion


    #region Unity_Func
    // Start is called before the first frame update
    void Awake()
    {
        num_enemy = 0;
        wave_timer = wave_interval;
        wave_counter = 0;
        StartCoroutine("SpawnLoop");

    }

    // Update is called once per frame
    void Update()
    {
        wave_timer -= Time.deltaTime;
        m_HUD.UpdateWaveTimer(wave_timer);
        m_HUD.UpdateWaveCounter(wave_counter);

    }


    #endregion


    #region My_Func

    IEnumerator SpawnLoop(){
        while(true){

            if (wave_timer <= 0){
                //spawn wave
                Debug.Log("wave");
                wave_timer = wave_interval;
                spawnWave();
            } else {
                //spawn enem(ies) periodically
                if (Random.Range(0f, 1f) * spawn_rate >= 0.5){
                    Debug.Log("mini wave");
                    spawnRandom();
                }
            }

            //spawn checked every 1 seconds => ~1 tries per second
            yield return new WaitForSeconds(1f);

        }
    }

    //randomly spawns 1-3 enemies at a rate of about 
    void spawnRandom(){
        int possible_num = max_enemy - num_enemy; 
        //if max reached, do nothing
        if (possible_num <= 0) {
            return;
        }
        
        //figure out how many enemies to instantiate
        int max_instantiate = Mathf.Max(max_enemy_mini, possible_num);
        int spawnNum = Random.Range(0, max_instantiate);
        
        
        for (int i = 0; i < spawnNum; i++) {
            Vector3 rando = new Vector3(Random.Range(x_range.x, x_range.y), Random.Range(y_range.x, y_range.y), 0);
            Instantiate(TrashManPreFab, transform.position + rando, Quaternion.identity);
            num_enemy += 1;
        }
    }

    //spawns maxEnemies
    void spawnWave(){
        int possible_num = max_enemy - num_enemy;         
        //if max reached, do nothing
        if (num_enemy >= max_enemy) {
            return;
        }
        
        
        wave_counter += 1;
        //figure out how many enemies to instantiate
        int max_instantiate = Mathf.Max(max_enemy_wave, possible_num);
        int spawnNum = max_instantiate;
        
        
        for (int i = 0; i < spawnNum; i++) {
            Vector3 rando = new Vector3(Random.Range(x_range.x, x_range.y), Random.Range(y_range.x, y_range.y), 0);
            Instantiate(TrashManPreFab, transform.position + rando, Quaternion.identity);
            num_enemy += 1;
        }
    }
    #endregion

}
