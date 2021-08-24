using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling{
    /*
    Generates pools of objects to pull from.
    */
    public class ObjectPooler : Singleton<ObjectPooler>
    {
#region Serialization
        [System.Serializable]
        public class Pool{
            public string tag;
            public GameObject template;
            public int count;
        }

        [SerializeField]
        private Pool[] pools;
#endregion

        private Dictionary<string, Queue<GameObject>> poolBook;

        private void Awake(){
            poolBook = new Dictionary<string, Queue<GameObject>>();

            for(int i = 0; i < pools.Length; i++){
                Queue<GameObject> poolQueue = new Queue<GameObject>();
                for(int j = 0; j < pools[i].count; j++){
                    GameObject g = Instantiate(pools[i].template, Vector3.zero, Quaternion.identity);
                    g.SetActive(false);
                    g.transform.SetParent(transform);
                    poolQueue.Enqueue(g);
                }
                poolBook.Add(pools[i].tag, poolQueue);
            }
        }

        /*
        Spawns an object from the dictionary.
        */
        public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation, GameObject manager){
            GameObject g = poolBook[tag].Dequeue();
            g.transform.SetParent(null);
            g.transform.position = position;
            g.transform.rotation = rotation;
            g.SetActive(true);
            g.GetComponent<ISpawnable>().Spawn(manager);
            return g;
        }

        /*
        Despawns an object, returns it to the dictionary.
        */
        public void DespawnObject(string tag, GameObject gameObject){
            gameObject.SetActive(false);
            poolBook[tag].Enqueue(gameObject);
            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<ISpawnable>().Despawn();
        }
    }
}
