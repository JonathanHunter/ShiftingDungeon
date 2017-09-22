namespace ShiftingDungeon.Util
{
    public class Enums
    {
        public enum GameState { Playing, Paused, Tranisioning, Cutscene }

        public enum HeroState
        {
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] {})]
            Idle,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            North,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            NorthEast,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            East,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            SouthEast,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            South,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            SouthWest,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            West,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { "Movement" })]
            NorthWest,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { })]
            Attack,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] { })]
            Hurt,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] {})]
            length
        }

        public enum BulletTypes { HeroBasic }

        public enum EnemyTypes { Basic }

        public enum Direction { None, Up, Down, Left, Right }

        public enum Tags { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard, Pickup, Trap }

        public enum Layers { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard, Dungeon, Pickup }

        public enum Traps { Spike }
    }
}
