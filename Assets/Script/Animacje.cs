using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacje : MonoBehaviour
{
    public Animator animator;
    public void EndOfMeleeAttack()
    {
        animator.SetBool("MeleeAttack", false);
    }
}
