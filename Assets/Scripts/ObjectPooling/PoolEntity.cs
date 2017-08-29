namespace ShiftingDungeon.ObjectPooling
{
    public class PoolEntity
    {
        /// <summary> The entity being managed. </summary>
        public IPoolable Entity { get; private set; }

        /// <summary> True if the managed entity is active in the scene. </summary>
        public bool IsSpawned { get; private set; }

        public PoolEntity(IPoolable entity)
        {
            this.Entity = entity;
            this.IsSpawned = false;
        }

        /// <summary> Allocates this object for use in the scene. </summary>
        public void Allocate()
        {
            this.Entity.ReInitialize();
            this.IsSpawned = true;
        }

        /// <summary> Removes this object from the scene and puts it on standby. </summary>
        public void Deallocate()
        {
            this.Entity.Deallocate();
            this.IsSpawned = false;
        }

        /// <summary> Completely removes this object from the scene. </summary>
        public void Delete()
        {
            this.Entity.Deallocate();
            this.Entity.Delete();
            this.IsSpawned = false;
        }
    }
}
