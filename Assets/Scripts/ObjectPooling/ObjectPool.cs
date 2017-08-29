namespace ShiftingDungeon.ObjectPooling
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ObjectPool : MonoBehaviour
    {
        private Dictionary<IPoolable, PoolEntity[]> objectPools;

        protected void Start()
        {
            this.objectPools = new Dictionary<IPoolable, PoolEntity[]>();
            IPoolable[] templets = GetTemplets();
            int[] poolSizes = GetPoolSizes();
            for (int i = 0; i < templets.Length; i++)
            {
                SpawnPool(templets[i], poolSizes[i]);
            }
        }

        private void SpawnPool(IPoolable templet, int poolSize)
        {
            PoolEntity[] pool = new PoolEntity[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                IPoolable entity = templet.SpawnCopy(i);
                entity.Initialize();
                entity.Deallocate();
                pool[i] = new PoolEntity(entity);                
            }

            this.objectPools.Add(templet, pool);
        }

        /// <summary> Gets the templets to be pooled. </summary>
        /// <returns> The array of templets. </returns>
        protected abstract IPoolable[] GetTemplets();
        /// <summary> Gets the pool size to use for each templet. </summary>
        /// <returns> The array of sizes. </returns>
        protected abstract int[] GetPoolSizes();

        /// <summary> Allocates an entity from the pool. </summary>
        /// <param name="templet"> The type of entity to spawn. </param>
        /// <returns> A reference to the allocated object or null of no more can be allocated. </returns>
        protected IPoolable AllocateEntity(IPoolable templet)
        {
            foreach(PoolEntity pe in this.objectPools[templet])
            {
                if(!pe.IsSpawned)
                {
                    pe.Allocate();
                    return pe.Entity;
                }
            }

            return null;
        }

        /// <summary> Deallocates an entity and adds it back to the pool. </summary>
        /// <param name="templet"> The type of object to deallocate. </param>
        /// <param name="entity"> The specifc object to deallocate. </param>
        protected void DeallocateEntity(IPoolable templet, IPoolable entity)
        {
            this.objectPools[templet][entity.GetReferenceIndex()].Deallocate();
        }

        /// <summary> Deletes all pooled objects and clears pool. </summary>
        public void DeletePools()
        {
            foreach (PoolEntity[] pes in this.objectPools.Values)
            {
                foreach(PoolEntity pe in pes)
                {
                    pe.Delete();
                }
            }

            this.objectPools.Clear();
        }
    }
}
