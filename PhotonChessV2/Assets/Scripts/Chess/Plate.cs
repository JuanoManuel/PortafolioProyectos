using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public enum PlateTypes { Move, Attack, Promote, Castling, Previous, EnPassant} //type of plates enum

    public PlateTypes plateType;

    [Header("Debuging Fields")]
    public GameObject ownerPiece;//reference to the piece that creates this plate
    public GameObject pieceReference;//reference to an extra piece, used when clastling or En Passant move

    private int row, col; //Row and Col where the plate is located
    private Camera mainCamera;//reference to the main camera
    private GameMaster gameMaster;
    private int platesLayer = 8;//unity layer number of the plates
    private float offset = 0.1254f;//offset between squares on the board
    private void Start()
    {
        //obtain a reference to the main camera
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //reference to the game controller
        gameMaster = GameMaster.master;
    }
    // Update is called once per frame
    void Update()
    {
        //Touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject,platesLayer))
                {
                    if (hitObject.collider == gameObject.GetComponent<Collider>())
                    {
                        PlateOnClickAction(ownerPiece.GetComponent<Chessman>());//Passing chessman script of the referenced piece
                    }
                }
            }
        }
        //Mouse imput
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObject;
            if (Physics.Raycast(ray, out hitObject,platesLayer))
            {
                if (hitObject.collider == gameObject.GetComponent<Collider>())
                {
                    PlateOnClickAction(ownerPiece.GetComponent<Chessman>());//Passing chessman script of the referenced piece
                }
            }
        }
    }

    //Initialize the plate
    public void Activate(GameObject reference,int row,int col,GameObject rook = null)
    {
        ownerPiece = reference;
        this.row = row;
        this.col = col;
        pieceReference = rook;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(col * offset, 0.001f, row * offset);
    }

    //Implements the behaviour of the plates when clicked
    private void PlateOnClickAction(Chessman cm)
    {
        //Depending of the type of plate the behaviour will be different
        switch (plateType)
        {
            case PlateTypes.Move://Move the referenced piece to the position of the plate
            case PlateTypes.Attack:
                //Moving the local piece
                gameMaster.AddMoveToReplay(cm.row, cm.col, row, col);
                gameMaster.MovePiece(cm.row, cm.col, row, col,false);
                break;
            case PlateTypes.Castling://Making a castling move
                if(pieceReference != null)//if the castling reference isn't null
                {
                    Chessman rookChessman = pieceReference.GetComponent<Chessman>();
                    //determine if it's a left or right castling move
                    if(col == 2)//is a left move
                    {
                        //Creating the castling move
                        gameMaster.SetClastling(cm.row, cm.col, row, col, rookChessman.row, rookChessman.col, row, col + 1);
                    }
                    else//is a right move
                    {
                        //Creating the castling move
                        gameMaster.SetClastling(cm.row, cm.col, row, col, rookChessman.row, rookChessman.col, row, col - 1);
                    }
                }
                break;
            case PlateTypes.Promote://Promote move
                //show the promote menu pasing the position of this plate
                gameMaster.ShowPromotePanel(cm.row,cm.col,row, col,cm.player);
                break;
            case PlateTypes.EnPassant://EnPassantMove
                Chessman c = pieceReference.GetComponent<Chessman>();//enemy pawn script reference
                gameMaster.SetEnPassant(cm.row, cm.col, row, col, c.row, c.col,true);//Creating an EnPassant move
                break;
        }

        //finally destroy all the plates
        gameMaster.DestroyAllPlates();
    }
}
