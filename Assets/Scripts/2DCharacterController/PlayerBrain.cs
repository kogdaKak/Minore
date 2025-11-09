using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAiming))]
[RequireComponent(typeof(PlayerShooting))]
public class PlayerBrain : MonoBehaviour
{
    private PlayerInputHandler input;
    private PlayerMovement movement;
    private PlayerAiming aiming;
    private PlayerShooting shooting;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
        aiming = GetComponent<PlayerAiming>();
        shooting = GetComponent<PlayerShooting>();

        movement.Initialize(input);
        shooting.Initialize(input);
        aiming.Initialize(); // если firePoint уже назначен
    }

    private void Update()
    {
        movement.Tick();
        aiming.Tick();
        shooting.Tick();
    }

    private void FixedUpdate()
    {
        movement.FixedTick();
    }
}
