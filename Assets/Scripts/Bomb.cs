using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    Collider objectCollider;
    int explosionLayerMask; // which items to destroy
    int explosionRange = 1;
    private void Awake()
    {
        AnimationEvents.DetonateEvent += OnDetonate;
        Debug.Log("subscribed to detonate event");
    }
    private void Start()
    {
        explosionLayerMask = LayerMask.GetMask("Explosion");
        objectCollider = GetComponent<Collider>();
        objectCollider.isTrigger = true; // set the bomb to be walkable through and reactive to triggers
    }
    private void ExplodeToDirection(Vector3 direction, Vector3 currentPosition, int range)
    {
        Debug.Log($"Exploding to direction:{direction}");
        //create position where object collision will be tested
        Vector3 position = currentPosition + direction;
        Vector2 position2D = new Vector2(position.x, position.z);

        // check if target position is in grid bounds
        Rect gridBounds = new Rect(1, 1, GridManager.Instance.gridSizeX - 1, GridManager.Instance.gridSizeY - 1);
        // if (gridBounds.Contains(position2D))
        //  Debug.Log($"Position: {position2D}");



        if (range < explosionRange)
        {
            //creating a collider in the target position to check if occupied by an object from explosion layer
            Collider[] colliders = Physics.OverlapBox(position, Vector3.one / 2f, Quaternion.identity, explosionLayerMask);


            //calls self recursively until finds an object
            if (colliders.Length == 0)
            {

                Instantiate(explosionEffect, new Vector3(position.x, 1 / 2f, position.z), Quaternion.identity);

                int updatedRange = range + 1;
                ExplodeToDirection(direction, position, updatedRange);
            }
            else
            {// destroys object at the end of recursive function if there is an object to be destroyed
                if (colliders[0].CompareTag("Destructible"))
                {
                    GameObject destructible = colliders[0].gameObject;
                    GridManager.Instance.ClearGridCell(destructible.transform.position);
                    Destroy(destructible);
                    //OnDestructibleDestroyed?.Invoke();
                }
                else if (colliders[0].CompareTag("Player"))
                {
                    Instantiate(explosionEffect, new Vector3(position.x, 1 / 2f, position.z), Quaternion.identity);

                    int updatedRange = range + 1;
                    ExplodeToDirection(direction, position, updatedRange);
                }


            }
        }

        return;
    }
    private void ClearGridCell()
    {
        GridManager.Instance.ClearGridCell(gameObject.transform.position);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectCollider.isTrigger = false; // Become solid after the player leaves
        }
    }
    public void OnDetonate(int range)
    {
        //unsubcribe so the method wouldnt be call while grabage collector is deleting this object
        AnimationEvents.DetonateEvent -= OnDetonate;
        Debug.Log("exlpotion in bomb");
        explosionRange = range;// set the explotion range
        Vector3 position = gameObject.transform.position;

        //create effect and explode to each direction

        ExplodeToDirection(Vector3.forward, position, 0);
        ExplodeToDirection(Vector3.back, position, 0);
        ExplodeToDirection(Vector3.right, position, 0);
        ExplodeToDirection(Vector3.left, position, 0);
        // explode gameobject
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        //clear itself from grid array
        ClearGridCell();

        //destroy this game object
        Destroy(gameObject);

    }
}
