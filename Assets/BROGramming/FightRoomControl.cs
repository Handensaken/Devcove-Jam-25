using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FightRoomControl : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera fightCam;

    [SerializeField]
    private Transform CameraLocator;

    [SerializeField]
    private GameObject EndMarker;

    [SerializeField]
    private EnemySpawner enemySpawner;

    void Start()
    {
        //        Debug.Log(GameEventManager.instance);
        GameEventManager.instance.OnFightEnd += Exit;
        GameEventManager.instance.OnResumeCamControl += ToPlayer;
    }

    void OnDestroy()
    {
        GameEventManager.instance.OnResumeCamControl -= ToPlayer;
    }

    private void ToPlayer()
    {
        fightCam.gameObject.SetActive(false);
        EndMarker.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                fightCam.transform.position = CameraLocator.position;
                fightCam.gameObject.SetActive(true);
                GetComponent<BoxCollider2D>().isTrigger = false;
                GameEventManager.instance.StopPlayerInput();
                Invoke("HahaOOgerBooger", 2f);
            }
        }
    }

    void HahaOOgerBooger()
    {
        enemySpawner.Activate();
    }

    private void Exit()
    {
        Destroy(EndMarker);
        fightCam.gameObject.SetActive(false);
    }
}
