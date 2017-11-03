namespace ShiftingDungeon.Character.Hero
{
    using System.Collections.Generic;
    using UnityEngine;
    using Util;

    public class HeroInput : MonoBehaviour
    {
        [SerializeField]
        protected Animator anim = null;

        protected HeroBehavior behavior = null;
        private int moveHash = 0;
        protected int attackHash = 0;

        /// <summary> True if up is pressed. </summary>
        public bool Up { get; protected set; }
        /// <summary> True if down is pressed. </summary>
        public bool Down { get; protected set; }
        /// <summary> True if left is pressed. </summary>
        public bool Left { get; protected set; }
        /// <summary> True if right is pressed. </summary>
        public bool Right { get; protected set; }

        private void Start()
        {
            if (this.anim == null)
                this.anim = GetComponent<Animator>();

            this.behavior = GetComponent<HeroBehavior>();
            this.moveHash = Animator.StringToHash("Move");
            this.attackHash = Animator.StringToHash("Attack");
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            HashSet<CustomInput.UserInput> actions;
            GetInput(out actions);
            this.anim.SetBool(this.moveHash, this.Up || this.Down || this.Left || this.Right);

            if (behavior.CurrentState == Enums.HeroState.Attack && actions.Contains(CustomInput.UserInput.Attack))
                this.behavior.AttackQueued = true;

            this.anim.SetBool(this.attackHash, actions.Contains(CustomInput.UserInput.Attack));
            if (actions.Contains(CustomInput.UserInput.NextWeapon))
                this.behavior.GoToNextWeapon();
            if (actions.Contains(CustomInput.UserInput.PrevWeapon))
                this.behavior.GoToPreviousWeapon();
            if (actions.Contains(CustomInput.UserInput.Target))
                this.behavior.TargetEnemy();
        }

        /// <summary> Gets inputted actions from the player. </summary>
        /// <param name="actions">A set of action keys that the player has just pressed in the current frame.</param>
        protected virtual void GetInput(out HashSet<CustomInput.UserInput> actions)
        {
            this.Up = CustomInput.BoolHeld(CustomInput.UserInput.Up);
            this.Down = CustomInput.BoolHeld(CustomInput.UserInput.Down);
            this.Left = CustomInput.BoolHeld(CustomInput.UserInput.Left);
            this.Right = CustomInput.BoolHeld(CustomInput.UserInput.Right);
            actions = new HashSet<CustomInput.UserInput>();
            CustomInput.UserInput[] actionEnums = {
                CustomInput.UserInput.Attack,
                CustomInput.UserInput.NextWeapon,
                CustomInput.UserInput.PrevWeapon,
                CustomInput.UserInput.Target
            };
            foreach (CustomInput.UserInput action in actionEnums)
            {
                if (CustomInput.BoolFreshPress(action))
                {
                    actions.Add(action);
                }
            }
        }

        /// <summary> Resets the directional button press states. </summary>
        public void ResetInput()
        {
            Up = Down = Left = Right = false;
            this.anim.SetBool(this.moveHash, false);
            this.anim.SetBool(this.attackHash, false);
        }
    }
}
