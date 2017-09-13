namespace ShiftingDungeon.Util
{
    public class Enums
    {
        public enum GameState { Playing, Paused, Tranisioning, Cutscene }

        public enum HeroState { Idle, Move, Attack, Hurt, length }

        public enum BulletTypes { HeroBasic }

        public enum EnemyTypes { Basic }

        public enum Direction { None, Up, Down, Left, Right }

        public enum Tags { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard }

        public enum Layers { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard, Dungeon }
    }
}
