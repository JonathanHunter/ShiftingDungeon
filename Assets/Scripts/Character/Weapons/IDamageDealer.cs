namespace ShiftingDungeon.Character.Weapons
{
    public interface IDamageDealer
    {
        /// <summary> The damage this object does on contact. </summary>
        /// <returns> The damage amount. </returns>
        int GetDamage();
    }
}
