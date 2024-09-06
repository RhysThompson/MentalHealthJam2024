using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spider : MonoBehaviour
{
    public bool isMoving;
    private Animator animator;
    private StarterAssetsInputs playerInputs;
    public bool inMinigame = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>();
        StartCoroutine("switchState");
    }

    private IEnumerator switchState()
    {
        while (true)
        {
            int random = Random.Range(2, 5);
            yield return new WaitForSeconds(random);
            isMoving = !isMoving;
            animator.SetBool("isMoving", isMoving);
        }
    }
    private void FixedUpdate()
    {
        if (inMinigame && isMoving && playerInputs.isMoving())
        {
            print("Running away");
            playerInputs.forced_move = new Vector2(0, -1);
            playerInputs.force_sprint = true;
        }
        if(!isMoving)
        {
            playerInputs.forced_move = Vector2.zero;
            playerInputs.force_sprint = false;
        }
    }

    public void giveItem()
    {
        GameManager.Instance.keyItems.Add("SpiderGameKey");
        playerInputs.horizontal_move_disabled = false;
        playerInputs.forced_move = Vector2.zero;
        playerInputs.force_sprint = false;
    }
}
