namespace ShiftingDungeon.Util
{
    public class Enums
    {
        public enum HeroState { Idle, Move, Attack, Hurt, length }
        
        public enum BulletTypes { HeroBasic }

        public enum Tags { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard }

        public enum Layers { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard, Dungeon }
    }
}
