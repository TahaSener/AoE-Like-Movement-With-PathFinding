using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public LayerMask mouseColliderMask;
    public Transform player;
    bool stillWalking = false;
    public List<Node> path;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseMove();
            stillWalking = true;
        }
        if (stillWalking && path.Count>0)
        {
            MoveToLocation(path);
            if (path.Count == 0)
            {
                stillWalking = false;
            }
        }
        
    }

    private void mouseMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, mouseColliderMask))
        {
            Vector3 mouse = raycastHit.point;
            GetComponent<PathFinder>().startMovement(mouse);
            
        }
    }
    

    public void MoveToLocation(List<Node> _path)
    {
        
            player.position = Vector3.MoveTowards(player.position, _path[0].position, 3 * Time.deltaTime);

            if (Vector3.Distance(player.position, _path[0].position)<0.001f){
            path.RemoveAt(0);
            }
        
    }
}
