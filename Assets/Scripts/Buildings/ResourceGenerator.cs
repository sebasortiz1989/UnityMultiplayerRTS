using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : NetworkBehaviour
{
    [SerializeField] Health health = null;
    [SerializeField] int resourcesPerInterval = 10;
    [SerializeField] float interval = 2f;

    private float timer;
    private RTSPlayer player;

    public override void OnStartServer()
    {
        timer = interval;
        player = connectionToClient.identity.GetComponent<RTSPlayer>();

        health.ServerOnDie += ServerHandleDie;
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            player.SetResources(player.GetResources() + resourcesPerInterval);
            timer += interval;
        }
    }

    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void ServerHandleGameOver()
    {
        enabled = false;
    }
}
