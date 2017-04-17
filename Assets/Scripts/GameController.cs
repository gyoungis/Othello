using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int[,] boardState = new int[8, 8];
    public GameObject[] rowList;
    public GameObject[] columnList;
    public GameObject piece;

    private int playerSide;
    private gameBoard board;

    //void SetGameControllerReferences()
    //{
    //    for (int i = 0; i < rowList.Length; i++)
    //    {
    //        rowList[i].GetComponentInParent<gameBoard>().SetGameControllerReference(this);
    //        columnList[i].GetComponentInParent<gameBoard>().SetGameControllerReference(this);
    //    }
    //}

    //void Awake()
    //{
    //    SetGameControllerReferences();

    //}

    void Start()
    {
        board = GetComponent<gameBoard>();
        boardState = new int [8,8]{ { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0}, 
                                    { 0, 0, 0, 0, 0, 0, 0, 0}, 
                                    { 0, 0, 0, 1, 2, 0, 0, 0}, 
                                    { 0, 0, 0, 2, 1, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0} };
        Instantiate(piece, new Vector3(3f, 2f, 4f), Quaternion.identity);
        Instantiate(piece, new Vector3(4f, 2f, 3f), Quaternion.identity);
        Instantiate(piece, new Vector3(4f, 2f, 4f), Quaternion.Euler(180, 0, 0));
        Instantiate(piece, new Vector3(3f, 2f, 3f), Quaternion.Euler(180, 0, 0));
        playerSide = 1;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            float i;
            float j;
            int x;
            int y;
            Vector3 insertPoint;
            //Vector3 offset = new Vector3(0.5f, 0, 0.5f);
            //Vector3 position = Input.mousePosition;
            //position.y = 10.0f;
            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit[] hitList;

            //hitList = Physics.RaycastAll(ray, 9.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 12.0f))
            {

                i = Mathf.Floor(hit.point.x);
                j = Mathf.Floor(hit.point.z);
                x = (int)Mathf.Floor(hit.point.x);
                y = (int)Mathf.Floor(hit.point.z);

                if (i <= 7 && i >= 0 && j <= 7 && j >= 0 && boardState[x, y] == 0)
                {
                    if (playerSide == 1)
                    {
                        insertPoint = new Vector3(i, 2, j);
                        boardState[x, y] = playerSide;
                        Instantiate(piece, insertPoint, Quaternion.identity);
                        playerSide = 2;
                    }
                    else
                    {
                        insertPoint = new Vector3(i, 2, j);
                        boardState[x, y] = playerSide;
                        Instantiate(piece, insertPoint, Quaternion.Euler(180, 0, 0));
                        playerSide = 1;
                    }
                }
            }
            
            
        }
    }
}
