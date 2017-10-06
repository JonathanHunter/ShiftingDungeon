﻿namespace ShiftingDungeon.Util
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
                StateMachines = new string[] {})]
            Move,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] {})]
            Attack,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] {})]
            Hurt,
            [Attributes.StateMap(Layer = "Base Layer",
                StateMachines = new string[] {})]
            length
        }

        public enum BulletTypes { HeroBasic, EnemyBasic }

        public enum EnemyTypes { Basic, Shooter }

        public enum Direction { None, Up, Down, Left, Right }

        public enum Tags { Hero, Enemy, HeroWeapon, EnemyWeapon, DungeonHazard, Pickup, Trap }

        public enum Layers { Hero = 8, Enemy = 9, HeroWeapon = 10, EnemyWeapon = 11, DungeonHazard = 12, Dungeon = 13, Pickup = 14 }

        public enum Traps { Spike, SlowGoo }
    }
}
