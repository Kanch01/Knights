using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerController controller;

    public void EnableWeaponHitbox()
    {
        controller.EnableWeaponHitbox();
    }

    public void DisableWeaponHitbox()
    {
        controller.EndAttack();
    }
}

