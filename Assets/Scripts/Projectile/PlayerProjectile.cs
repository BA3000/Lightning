using UnityEngine;

namespace Projectile
{
    public class PlayerProjectile : Projectile
    {
        TrailRenderer trail;

        private void Awake()
        {
            trail = GetComponentInChildren<TrailRenderer>();

            if (moveDirection != Vector2.right)
            {
                transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
            }
        }

        private void OnDisable()
        {
            trail.Clear();
        }
    }
}
