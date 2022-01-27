using System.Collections.Generic;
using UnityEngine;
using NoMoney_Core;

namespace ObjectPooling{
    /// <summary>
    /// Generates pools of objects to pull from.
    /// </summary>
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

        /// <summary>
        /// Spawns an object from the dictionary.
        /// </summary>
        /// <param name="key">Object key in the dictionary.</param>
        /// <param name="position">Position of the spawned object.</param>
        /// <param name="rotation">Rotation of the spawned object.</param>
        /// <param name="manager">GameObject that spawned the object.</param>
        /// <returns></returns>
        public GameObject SpawnObject(string key, Vector3 position, Quaternion rotation, GameObject manager){
            GameObject g = poolBook[key].Dequeue();
            g.transform.SetParent(null);
            g.transform.position = position;
            g.transform.rotation = rotation;
            g.SetActive(true);
            g.GetComponent<ISpawnable>().Spawn(manager);
            return g;
        }

        /// <summary>
        /// Despawns the object, returning it to the dictionary.
        /// </summary>
        /// <param name="key">Key for the object.</param>
        /// <param name="gameObject">The object to despawn.</param>
        public void DespawnObject(string key, GameObject gameObject){
            gameObject.SetActive(false);
            poolBook[key].Enqueue(gameObject);
            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<ISpawnable>().Despawn();
        }
    }
}
