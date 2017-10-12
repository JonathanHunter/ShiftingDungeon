namespace ShiftingDungeon.Character.Hero
{
    using UnityEngine;
    using Util;

    public class HeroInput : MonoBehaviour
    {
        private Animator anim = null;
        private HeroBehavior behavior = null;
        private int moveHash = 0;
        private int attackHash = 0;

        /// <summary> True if up is pressed. </summary>
        public bool Up { get; private set; }
        /// <summary> True if down is pressed. </summary>
        public bool Down { get; private set; }
        /// <summary> True if left is pressed. </summary>
        public bool Left { get; private set; }
        /// <summary> True if right is pressed. </summary>
        public bool Right { get; private set; }

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.behavior = GetComponent<HeroBehavior>();
            this.moveHash = Animator.StringToHash("Move");
            this.attackHash = Animator.StringToHash("Attack");
        }

        private void Update()
        {
            if (Managers.GameState.Instance.IsPaused)
                return;

            this.Up = CustomInput.BoolHeld(CustomInput.UserInput.Up);
            this.Down = CustomInput.BoolHeld(CustomInput.UserInput.Down);
            this.Left = CustomInput.BoolHeld(CustomInput.UserInput.Left);
            this.Right = CustomInput.BoolHeld(CustomInput.UserInput.Right);
            this.anim.SetBool(this.moveHash, this.Up || this.Down || this.Left || this.Right);

            if (behavior.CurrentState == Enums.HeroState.Attack && CustomInput.BoolFreshPress(CustomInput.UserInput.Attack))
                this.behavior.AttackQueued = true;

            this.anim.SetBool(this.attackHash, CustomInput.BoolFreshPress(CustomInput.UserInput.Attack));
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.NextWeapon))
                this.behavior.GoToNextWeapon();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.PrevWeapon))
                this.behavior.GoToPreviousWeapon();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Target))
                this.behavior.TargetEnemy();
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
