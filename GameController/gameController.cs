using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public GameObject enemies;

    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public Transform playfieldAnchorTransform;
    public List<Pool> bulletPools;
    public Dictionary<string, List<GameObject>> bulletDictionary;

    public static gameController sharedOverseer;
    
    //mending ga usah dijadiin satu class sendiri
    #region EnemyGenerationPattern

    private Timer generateTimer;
    //public Timer[] timerList;
    public Vector2[] locationList;
    public float[] rotationList, timerList;

    public void generateEnemy(Vector2 location, float rotation)
    {
        Instantiate(enemies, new Vector3(playfieldAnchorTransform.position.x+location.x, playfieldAnchorTransform.position.y + location.y, 0), Quaternion.Euler(0,0,rotation));
    }

    #endregion

    void Awake()
    {
        sharedOverseer = this;
    }

    void Start()
    {
        bulletDictionary=new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in bulletPools)
        {
            List<GameObject> objPool = new List<GameObject>();
            for(int i=0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(playfieldAnchorTransform);
                objPool.Add(obj);
            }
            bulletDictionary.Add(pool.tag, objPool);
        }

        generateTimer = new Timer(timerList[0], true);
    }

    void Update()
    {
        int i=0;
        if(i<locationList.Length)
        {
            if(generateTimer.timerCount())
            {
                generateEnemy(locationList[i], rotationList[i]);
                generateTimer.changeDuration(timerList[i+1]);
            }
            i++;
        }
    }

    public void shoot(Vector3 position, float rotate, float speed)
    {
        for(int i=0;i<bulletDictionary["AlifNormal"].Count;i++)
        {
            if(!bulletDictionary["AlifNormal"][i].activeInHierarchy)
            {
                bulletDictionary["AlifNormal"][i].SetActive(true);
                bulletDictionary["AlifNormal"][i].transform.localPosition=position;
                bulletDictionary["AlifNormal"][i].transform.Rotate(new Vector3(0,0,rotate));
                SpeedClass spd = bulletDictionary["AlifNormal"][i].GetComponent<SpeedClass>();
                spd.speed = speed;
                return;
            }
        }
    }
}
