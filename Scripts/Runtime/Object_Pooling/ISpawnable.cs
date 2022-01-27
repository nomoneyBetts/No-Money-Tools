using UnityEngine;
namespace ObjectPooling{
    /*
    Any Object that can be spawned from the pool must inherit from this interface
    */
    public interface ISpawnable
    {
        /*
        Any additional commands the object should perform after separating from object pooler.
        Is Also given a reference to the manager that spawned it.
        */
        void Spawn(GameObject manager);

        /*
        Any additional commands the object should perform after rejoining the object pooler.
        */
        void Despawn();
    }
}
