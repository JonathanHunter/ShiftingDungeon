namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour
    {
        /// <summary> The level of this weapon. </summary>
        public int Level { get; internal set; }

        /// <summary> The numerical progression this weapon's combo </summary>
        protected int comboCounter = 0;

        /// <summary> The maximum number of attacks in a weapon's combo </summary>
        protected int maxCombo = 1;

        /// <summary> The maximum time interval between attacks to continue the combo in milliseconds </summary>
        protected float comboInterval = 400f;

        /// <summary> The last game time moment an attack was made </summary>
        private float lastAttackTime;

        /// <summary> Initializes this weapon. </summary>
        public void Init()
        {
            LocalInit();
        }

        /// <summary> Readys this weapon to attack. </summary>
        public void ReInit()
        {
            comboCounter = Time.time - lastAttackTime <= comboInterval / 1000f
                ? (comboCounter + 1) % maxCombo : 0;
            LocalReInit();
            this.gameObject.SetActive(true);
        }

        /// <summary> Cleans up this object </summary>
        public void CleanUp()
        {
            LocalCleanUp();
            lastAttackTime = Time.time;
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
