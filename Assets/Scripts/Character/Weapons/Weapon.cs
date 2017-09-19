namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour
    {
        /// <summary> The level of this weapon. </summary>
        public int Level { get; internal set; }

        /// <summary> The combo counter of this weapon </summary>
        private int comboCounter = 0;

        /// <summary> Initializes this weapon. </summary>
        public void Init()
        {
            LocalInit();
        }

        /// <summary> Readys this weapon to attack. </summary>
        public void ReInit()
        {
            LocalReInit();
            this.gameObject.SetActive(true);
        }

        /// <summary> Cleans up this object </summary>
        public void CleanUp()
        {
            LocalCleanUp();
            this.gameObject.SetActive(false);
        }

        /// <summary> Performs this weapons attack. </summary>
        /// <returns> True if this weapons attack is finished. </returns>
        public abstract bool WeaponUpdate();

        /// <summary> Local Init for subclasses. </summary>
        protected abstract void LocalInit();
        /// <summary> Local ReInit for subclasses. </summary>
        protected abstract void LocalReInit();
        /// <summary> Local CleanUp for subclasses. </summary>
        protected abstract void LocalCleanUp();
    }
}
