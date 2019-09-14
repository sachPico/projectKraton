using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
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
    }

    public void shoot(Vector3 position, float rotate)
    {
        for(int i=0;i<bulletDictionary["AlifNormal"].Count;i++)
        {
            if(!bulletDictionary["AlifNormal"][i].activeInHierarchy)
            {
                bulletDictionary["AlifNormal"][i].SetActive(true);
                bulletDictionary["AlifNormal"][i].transform.localPosition=position;
                return;
            }
        }
    }
}
