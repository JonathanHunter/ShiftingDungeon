namespace ShiftingDungeon.ObjectPooling
{
    using UnityEngine;

    public interface IPoolable
    {
        /// <summary> Spawns a copy of this object for use in the object pool. </summary>
        /// <param name="referenceIndex"> The refence index for thi object. </param>
        /// <returns> A reference to the copy. </returns>
        IPoolable SpawnCopy(int referenceIndex);

        /// <summary> Gets the reference index for this object. </summary>
        /// <returns> The reference index. </returns>
        int GetReferenceIndex();

        /// <summary> Gets the gameobject for this object. </summary>
        /// <returns> The gameobject. </returns>
        GameObject GetGameObject();

        /// <summary> Initializes this pooled object. </summary>
        void Initialize();

        /// <summary> Resets this pooled object for use and turns it on for use in the scene. </summary>
        void ReInitialize();

        /// <summary> Resets any needed meta data and turns this object off so it can be returned to the pool. </summary>
        void Deallocate();

        /// <summary> Cleans up anything related to this object so it can be removed from the pool. </summary>
        void Delete();
    }
}
