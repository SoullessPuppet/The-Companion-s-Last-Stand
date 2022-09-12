using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject selectedObj;
    int selectMask;

    void Start()
    {
        selectMask = LayerMask.GetMask("SelectHitbox");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 0f, selectMask);

            if (hit)
            {
                //Disable selected status of last selectedObj
                if (selectedObj != null)
                    selectedObj.GetComponentInChildren<SelectHitbox>().selected = false;

                //Get new selectedObj
                if (hit.collider.gameObject.CompareTag("SelectHitbox"))
                {
                    GameObject hitboxGameObject = hit.collider.gameObject;
                    selectedObj = hitboxGameObject.transform.parent.gameObject;

                    hitboxGameObject.GetComponent<SelectHitbox>().selected = true;
                    selectedObj.transform.Find("AttackRange").GetComponent<SpriteRenderer>().enabled = true;
                    if(selectedObj.transform.Find("GatherPoint") != null)
                        selectedObj.transform.Find("GatherPoint").GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            else if (selectedObj != null)
            {
                selectedObj.GetComponentInChildren<SelectHitbox>().selected = false;
                selectedObj.transform.Find("AttackRange").GetComponent<SpriteRenderer>().enabled = false;
                if (selectedObj.transform.Find("GatherPoint") != null)
                    selectedObj.transform.Find("GatherPoint").GetComponent<SpriteRenderer>().enabled = false;
                selectedObj = null;
            }
        }
    }
}
