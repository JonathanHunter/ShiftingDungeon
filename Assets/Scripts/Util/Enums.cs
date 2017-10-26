namespace ShiftingDungeon.Util
{
    public class Enums
    {
        public enum GameState { Playing, Paused, Tranisioning, Cutscene }

        public enum HeroState { Idle, Move, Attack, Hurt, length }

        public enum BulletTypes { HeroBasic, EnemyBasic }

        public enum EnemyTypes { Basic, Shooter, Melee, Grass, Boss }

        public enum Direction { None, Up, Down, Left, Right }

        public enum Tags { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard, Pickup, Trap }

        public enum Layers { Hero = 8, HeroSuspended = 17, Enemy = 9, HeroWeapon = 10, EnemyWeapon = 11, Trap = 15, Dungeon = 13, Pickup = 14 }

        public enum Traps { Spike, SlowGoo }
    }
}
