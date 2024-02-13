using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToWorldTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ScreenMouseRay();
        }
            
    }
    public void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
 
        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v, LayerMask.NameToLayer("Player"));
 
            if(col.Length > 0){
                foreach(Collider2D c in col)
                {
                    Debug.Log("Collided with: " + c.gameObject.name);

                
                    //targetPos = c.gameObject.transform.position;
                }
            }
    }
    
}
