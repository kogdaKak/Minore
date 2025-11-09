using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShooting))]
public class PlayerInstaller : MonoBehaviour
{
    private void Awake()
    {
        var input = GetComponent<PlayerInputHandler>();
        GetComponent<PlayerMovement>().Initialize(input);
        GetComponent<PlayerShooting>().Initialize(input);
    }
}
