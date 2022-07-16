using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public PlayerController player;
    public void ResetRotation()
    {
        player.ResetRotation();
    }
    public void EnemyDie()
    {
        player.EnemyDieForAnim();
    }
}
