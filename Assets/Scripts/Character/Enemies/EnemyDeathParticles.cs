namespace ShiftingDungeon.Character.Enemies
{

    using UnityEngine;

    /// <summary>
    /// Used to keep the particle object enabled when the enemy is reclaimed by the pooling system.
    /// </summary>
    class EnemyDeathParticles : MonoBehaviour
    {

        /// <summary> The object's original parent transform to save. </summary>
        private Transform origParent;
        /// <summary> The particle system attached to the object. </summary>
        private ParticleSystem particles;

        /// <summary> The amount of time that the particles should stay active after being triggered.</summary>
        private float activeTime;
        /// <summary>
        /// Timer for keeping the object enabled.
        /// </summary>
        private float timer = 0;

        /// <summary> Caches object components. </summary>
        private void Start()
        {
            origParent = transform.parent;
            particles = GetComponent<ParticleSystem>();
            activeTime = particles.main.startLifetime.constant;
            particles.Stop();
        }

        /// <summary>
        /// Emits a number of particles from the particle system.
        /// </summary>
        /// <param name="numDeathParticles">The number of particles to emit.</param>
        internal void Emit(int numDeathParticles)
        {
            transform.parent = null;
            particles.Emit(numDeathParticles);
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update()
        {
            if (transform.parent == null)
            {
                timer += Time.deltaTime;
                if (timer >= activeTime)
                {
                    timer = 0;
                    transform.parent = origParent;
                }
            }
        }
    }
}