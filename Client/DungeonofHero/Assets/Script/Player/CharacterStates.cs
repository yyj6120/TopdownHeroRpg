using UnityEngine;
using System.Collections;

public class CharacterStates
{

    public enum CharacterConditions
    {
        Normal,
        ControlledMovement,
        Frozen,
        Paused,
        Dead
    }

    public enum MovementStates
    {
        Idle,
        Walking,
        Falling,
        Running,
        Crouching,
        Crawling,
        Dashing,
        LookingUp,
        WallClinging,
        Jetpacking,
        Diving,
        Gripping,
        Dangling,
        Jumping,
        Pushing,
        DoubleJumping,
        WallJumping,
        LadderClimbing
    }
}