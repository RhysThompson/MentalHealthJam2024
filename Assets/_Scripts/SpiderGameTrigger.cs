using Cinemachine;
using StarterAssets;
using System.Collections;
using UnityEngine;

public class SpiderGameTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera spiderCam;
    public Transform PlayerStartTransform;
    private GameObject Player;
    private StarterAssetsInputs playerInputs;
    public Spider spider;
    public bool inMinigame = false;
    void Start()
    {
        spiderCam.enabled = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInputs = Player.GetComponent<StarterAssetsInputs>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //start minigame
            Debug.Log("Spider Game Triggered");
            StartCoroutine(FocusOnSpider());
            inMinigame = true;
            spider.inMinigame = inMinigame;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInputs.horizontal_move_disabled = false;
            playerInputs.forced_move = Vector2.zero;
            playerInputs.force_sprint = false;
            inMinigame = false;
            spider.inMinigame = inMinigame;
        }
    }
    private IEnumerator FocusOnSpider()
    {
        spiderCam.enabled = true;
        GameManager.Instance.DisableMovement();

        yield return new WaitForSeconds(3);

        Player.transform.position = PlayerStartTransform.position;
        Player.transform.rotation = PlayerStartTransform.rotation;
        GameManager.Instance.EnableMovement();
        playerInputs.horizontal_move_disabled = true;
        spiderCam.enabled = false;
    }

}
