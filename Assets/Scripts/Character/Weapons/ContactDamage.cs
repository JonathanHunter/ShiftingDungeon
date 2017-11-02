namespace ShiftingDungeon.Character.Weapons
{
    using UnityEngine;

    public class ContactDamage : MonoBehaviour, IDamageDealer
    {
        [SerializeField]
        private int damage = 1;

        public int GetDamage()
        {
            return this.damage;
        }
    }
}
