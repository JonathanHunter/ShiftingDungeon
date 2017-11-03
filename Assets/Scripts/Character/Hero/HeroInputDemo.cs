namespace ShiftingDungeon.Character.Hero
{
    using System.Collections.Generic;
    using UnityEngine;
    using Managers;
    using Util;

    /// <summary> Controls the hero automatically in the title screen demo. </summary>
    public class HeroInputDemo : HeroInput
    {

        /// <summary> Behavior states for the hero to act out </summary>
        private enum AIState { DelaySwitch, FindTarget, Approach, Target, Swing, Shoot };

        /// <summary> The hero's current behavior state. </summary>
        private AIState state = AIState.Approach;

        /// <summary> Used to slightly delay the switching of states. </summary>
        private AIState delayedState;
        /// <summary> The amount of time to delay switching by. </summary>
        private const float DELAY_SWITCH_TIME = 0.1f;
        /// <summary> Timer for delaying state switches. </summary>
        private float delaySwitchTimer = 0;

        /// <summary> The position that the hero is heading for. </summary>
        private Vector2 goal;
        /// <summary> The enemy that the hero is currently targeting. </summary>
        private Collider2D currentEnemy;

        /// <summary>A set of actions to execute in the current frame.</summary>
        private HashSet<CustomInput.UserInput> actions = new HashSet<CustomInput.UserInput>();

        /// <summary> The distance away from a target that will cause the player to move. </summary>
        private const float MOVE_THRESHOLD = 0.85f;
        /// <summary> The distance away from a target that will cause the player to attack. </summary>
        private const float ATTACK_THRESHOLD = MOVE_THRESHOLD * 1.5f;

        /// <summary> The radius to consider the hero's size as. </summary>
        private const float HERO_RADIUS = 0.25f;

        /// <summary> The maximum number of shots to take before trying to move elsewhere. </summary>
        private const int MAX_SHOTS = 3;
        /// <summary> The number of shots that have been taken. </summary>
        private int shotCounter = 0;

        /// <summary> The last position of the hero when approaching. </summary>
        private Vector2 lastPos;
        /// <summary> The time to wait before resetting from enemies being too far away. </summary>
        private const float RESET_TIME = 0.5f;

        /// <summary> Gets inputted actions from the player. </summary>
        /// <param name="actions">A set of actions to execute in the current frame.</param>
        protected override void GetInput(out HashSet<CustomInput.UserInput> actions)
        {
            Up = Down = Left = Right = false;
            this.actions.Clear();
            actions = this.actions;

            Collider2D[] targets = behavior.getEnemiesInRange(25);
            if (targets.Length == 0)
            {
                // Reset the level if all enemies are dead.
                DungeonManager.StartResetTitleScreen();
                return;
            }
            if (currentEnemy != targets[0])
            {
                ResetDemoHero();
            }
            currentEnemy = targets[0];
            goal = currentEnemy.transform.position;

            switch (this.state)
            {
            case AIState.DelaySwitch: DelaySwitch(); break;
            case AIState.FindTarget: FindTarget(); break;
            case AIState.Approach: Approach(); break;
            case AIState.Target: Target(); break;
            case AIState.Swing: Swing(); break;
            case AIState.Shoot: Shoot(); break;
            }
        }

        /// <summary> Finds a target to head towards. </summary>
        private void DelaySwitch()
        {
            this.delaySwitchTimer += Time.deltaTime;
            if (delaySwitchTimer > DELAY_SWITCH_TIME)
            {
                this.delaySwitchTimer = 0;
                this.state = delayedState;
            }
        }

        /// <summary> Finds a target to head towards. </summary>
        private void FindTarget()
        {
            // Switch to the sword.
            if (behavior.CurrentWeapon == 1)
            {
                this.actions.Add(CustomInput.UserInput.NextWeapon);
            }
            this.state = AIState.Approach;
        }

        /// <summary> Travels to the target. </summary>
        private void Approach()
        {
            Vector2 currentPos = transform.position;
            Vector2 posDifference = goal - currentPos;
            if (posDifference.x < -MOVE_THRESHOLD)
            {
                this.Left = true;
            }
            else if (posDifference.x > MOVE_THRESHOLD)
            {
                this.Right = true;
            }
            if (posDifference.y < -MOVE_THRESHOLD)
            {
                this.Down = true;
            }
            else if (posDifference.y > MOVE_THRESHOLD)
            {
                this.Up = true;
            }

            float goalDistance = GetGoalDistance();
            if (goalDistance <= ATTACK_THRESHOLD)
            {
                delaySwitchTimer = 0;
                ChangeStateDelayed(AIState.Target);
            }

            // Switch to the gun if blocked by walls.
            if (goalDistance < behavior.TargetRange &&
                Physics2D.OverlapCircle(currentPos + posDifference / posDifference.SqrMagnitude() * HERO_RADIUS, HERO_RADIUS, 1 << (int) Enums.Layers.Dungeon))
            {
                if (behavior.CurrentWeapon == 0)
                {
                    this.actions.Add(CustomInput.UserInput.NextWeapon);
                }
                delaySwitchTimer = 0;
                ChangeStateDelayed(AIState.Target);
            }

            if (Vector2.Distance(lastPos, currentPos) < Mathf.Epsilon)
            {
                delaySwitchTimer += Time.deltaTime;
                if (delaySwitchTimer >= RESET_TIME)
                {
                    // Reset the level if enemies are too far away.
                    DungeonManager.StartResetTitleScreen();
                }
            }
            else
            {
                delaySwitchTimer = 0;
            }
            lastPos = currentPos;
        }

        /// <summary> Targets the enemy. </summary>
        private void Target()
        {
            if (!IsAttacking())
            {
                this.actions.Add(CustomInput.UserInput.Target);
                if (behavior.CurrentWeapon == 0)
                {
                    ChangeStateDelayed(AIState.Swing);
                }
                else
                {
                    ChangeStateDelayed(AIState.Shoot);
                }
            }
        }

        /// <summary> Swings a sword at the target. </summary>
        private void Swing()
        {
            if (!IsAttacking())
            {
                if (GetGoalDistance() > ATTACK_THRESHOLD)
                {
                    // Re-approach if the enemy moves out of range.
                    this.state = AIState.FindTarget;
                }
                else
                {
                    this.actions.Add(CustomInput.UserInput.Attack);
                }
            }
        }

        /// <summary> Shoots at the target. </summary>
        private void Shoot()
        {
            if (!IsAttacking())
            {
                if (++this.shotCounter > MAX_SHOTS)
                {
                    // Camping is boring, so at least try to approach again after a while.
                    this.shotCounter = 0;
                    this.state = AIState.FindTarget;
                }
                else
                {
                    this.actions.Add(CustomInput.UserInput.Attack);
                }
            }
        }

        /// <summary> Causes the hero to change states after a short delay. </summary>
        /// <param name="newState">The state to switch to after a delay.</param>
        private void ChangeStateDelayed(AIState newState)
        {
            this.state = AIState.DelaySwitch;
            this.delayedState = newState;
        }

        /// <summary> Gets the distance from the hero to its goal. </summary>
        /// <returns>The distance from the hero to its goal.</returns>
        private float GetGoalDistance()
        {
            Vector2 currentPos = transform.position;
            return Vector2.Distance(currentPos, this.goal);
        }

        /// <summary> Checks if the hero is currently in its attacking animation. </summary>
        /// <returns>Whether the hero is currently in its attacking animation.</returns>
        private bool IsAttacking()
        {
            return this.behavior.CurrentState == Enums.HeroState.Attack;
        }

        /// <summary> Resets the state of the demo hero. </summary>
        public void ResetDemoHero()
        {
            this.state = AIState.FindTarget;
            this.delaySwitchTimer = 0;
            this.shotCounter = 0;
        }
    }
}
