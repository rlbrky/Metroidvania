using UnityEngine;

public class MantarAnimControl : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void PlayMushroomAnim()
    {
        animator.Play("JumpPlayer");
    }
}
