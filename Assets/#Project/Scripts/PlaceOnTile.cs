using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceOnTile : MonoBehaviour
{
    bool IsEmpty = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnToken(Transform token){
        if (IsEmpty){
            Vector3 position = transform.position + Vector3.up * 0.2f;
            Instantiate(token, position, Quaternion.identity, transform);
            IsEmpty = false;
        }
        return;
    }
    // private void OnMouseDown(){
    //     Debug.Log("Click!");
    // }
}
