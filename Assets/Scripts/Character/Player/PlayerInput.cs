namespace ShiftingDungeon.Character.Player
{
    using UnityEngine;
    using Util;

    public class PlayerInput : MonoBehaviour
    {
        private Animator anim = null;
        private PlayerBehavior behavior = null;
        private int moveHash = 0;
        private int attackHash = 0;

        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Left { get; private set; }
        public bool Right { get; private set; }

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.behavior = GetComponent<PlayerBehavior>();
            this.moveHash = Animator.StringToHash("Move");
            this.attackHash = Animator.StringToHash("Attack");
        }

        private void Update()
        {
            this.Up = CustomInput.BoolHeld(CustomInput.UserInput.Up);
            this.Down = CustomInput.BoolHeld(CustomInput.UserInput.Down);
            this.Left = CustomInput.BoolHeld(CustomInput.UserInput.Left);
            this.Right = CustomInput.BoolHeld(CustomInput.UserInput.Right);
            this.anim.SetBool(this.moveHash, this.Up || this.Down || this.Left || this.Right);
            this.anim.SetBool(this.attackHash, CustomInput.BoolHeld(CustomInput.UserInput.Attack));
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.NextWeapon))
                this.behavior.GoToNextWeapon();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.PrevWeapon))
                this.behavior.GoToPreviousWeapon();
        }
    }
}
