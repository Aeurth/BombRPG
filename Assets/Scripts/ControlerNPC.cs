using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlerNPC : MonoBehaviour
{
    NavMeshAgent agent;
    private void Awake()
    {
        PlayerController.PlayerDataChanged += SetDirection;
    }
    void SetDirection(PlayerData data)
    {
        if (agent == null)
        {
            Debug.Log($"Nav Agent is null for {gameObject.name}");
            return;
        }
        Debug.Log($"agent reference:{agent.gameObject.name}");
        Debug.Log($"player position:{data.position}");
        agent.destination = data.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            agent = this.gameObject.AddComponent<NavMeshAgent>();
            Debug.Log($"Nav Agent added to {agent.gameObject.name} after colliding with the ground");
        }
       
    }
}
