using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Transform prefab;
    [SerializeField] Transform p1Token;
    [SerializeField] Transform p2Token;
    [SerializeField] float offset = .25f;
    [SerializeField] int height = 3;
    [SerializeField] int width = 3;
    Vector3 worldPoint;
    RaycastHit2D hit;
    bool p1Turn=true;
    // Start is called before the first frame update
    void Start()
    {
        float sizeX = prefab.GetComponent<Renderer>().bounds.size.x;
        float sizeZ = prefab.GetComponent<Renderer>().bounds.size.y;
        float totalWidth = width * (sizeX + offset);
        float totalHeight = height * (sizeZ + offset);

        Vector3 startPosition = transform.position - new Vector3((totalWidth - (sizeX + offset)) /2 , 0, (totalHeight - (sizeZ + offset)) /2);
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = startPosition + new Vector3(i * (sizeX+offset), 0, j * (sizeZ+offset));
                Transform t = Instantiate(prefab, position, Quaternion.Euler(90, 0, 0), transform);
                t.name = $"tile {i}-{j}";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {     
        onClick();
    }
    void onClick(){
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                if (p1Turn){
                    hit.transform.GetComponent<PlaceOnTile>().SpawnToken(p1Token);
                    AmIWinner(hit.transform, p1Turn);
                    p1Turn = false;
                }
                else{
                    hit.transform.GetComponent<PlaceOnTile>().SpawnToken(p2Token);
                    AmIWinner(hit.transform, p1Turn);
                    p1Turn = true;
                }
            }
        }    
    }
    void AmIWinner(Transform tile, bool p1Turn){
        string player = "p2";
        if(p1Turn){
            player = "p1";
        }
        //recuperer la position dans le tableau de la tuile via son nom
        int posX = Convert.ToInt32(tile.name.Substring(5, 1));
        int posY = Convert.ToInt32(tile.name.Substring(7, 1));
        //verifier si le même token present dans les 8 cases adjacentes
        //Check en haut à gauche (X-1, Y+1)
        if(posX-1<0 || posY+1>=height){ //test si pas hors tableau
            Debug.Log("-1,+1 out of bound");
            return;
        }
        else{
            GameObject target = GameObject.Find($"tile {posX-1}-{posY+1}");
            //check if target as a token (child)
            if(target.transform.childCount>0){
                if(target.transform.GetChild(0).tag==player){ //check if token on tile is same token as the player who just played
                    Debug.Log("child found in -1, +1!");
                    //test si placement 3e en diagonale haut gauche
                    if(posX-2<0 || posY+2>=height){ //test si pas hors tableau
                        Debug.Log(" -2, +2 out of bound");
                    }
                    else{
                        GameObject target2 = GameObject.Find($"tile {posX-2}-{posY+2}");
                        Debug.Log(target2.name);
                        //check if target as a token (child)
                        if(target.transform.childCount>0){
                            if(target.transform.GetChild(0).tag==player){ //check if token on tile is same token as the player who just played
                                Debug.Log(player +" win !");
                            }
                            else{
                                Debug.Log("Child -2,+2 not of" + player);
                            }
                        }
                        else{
                            Debug.Log("No child in -2, +2");
                        }
                    }
                }
                else{
                    Debug.Log("Child of -1,+1 not of" + player);
                }
            }
            else{
                Debug.Log("No child in -1, +1");
            }
        }
    }
}