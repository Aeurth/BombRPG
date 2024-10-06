using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour

{
    public enum Type{ 
        ExplosionRadius,
        PlayerSpeed,
        BombCount,
    }
    public Type type;

   
    // Start is called before the first frame update
    void Start()
    {
        // setting the rotation so the sporte 2D always spawns looking to the main camera
        transform.LookAt(Camera.main.transform.position);

    }
    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag != "Player")
        { return; }

        switch (type)
        {
            case Type.ExplosionRadius:
                other.gameObject.GetComponent<BombManager>().explosionRange++;
                Destroy(this.gameObject);
                break;
            case Type.PlayerSpeed:
                other.gameObject.GetComponent<PlayerController>().moveSpeed++;
                Destroy(this.gameObject);
                break;
            case Type.BombCount:
                int bombCount = ++other.GetComponent<BombManager>().max_bombs;
                UIManager.Instance.UpdateMaxBombsCount(bombCount);
                Destroy(this.gameObject);
                break;
        }
    }







}
