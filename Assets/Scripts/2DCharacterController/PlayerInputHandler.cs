using UnityEngine;

public interface IPlayerInput
{
    float Move { get; }
    bool JumpPressed { get; }
    bool JumpReleased { get; }
    bool FirePressed { get; }
}

public class PlayerInputHandler : MonoBehaviour, IPlayerInput
{
    public float Move => Input.GetAxisRaw("Horizontal");
    public bool JumpPressed => Input.GetButtonDown("Jump");
    public bool JumpReleased => Input.GetButtonUp("Jump");
    public bool FirePressed => Input.GetButtonDown("Fire1");
}
