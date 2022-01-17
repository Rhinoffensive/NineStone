using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceColor
{
    white = 1,
    black = 2
}
public class Piece : MonoBehaviour
{
    public PieceColor color;
    [SerializeField] private GameObject highLight;


    private GridManager gridManager;
    private bool _dragging;
    private SpriteRenderer _renderer;
    private Vector2 _previousLocation;
    private List<int> possibleLocations;

    private int _x, _y;


    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (color == PieceColor.white)
            _renderer.color = Color.white;
        else
            _renderer.color = Color.black;

        gridManager = FindObjectOfType<GridManager>();
        

    }

    private void Update()
    {
      if(!_dragging) return;
      Grab();
      
    }

    private void UpdatePosition()
    {
        var parrentTile = GetComponentInParent<Tile>();
        _x = parrentTile.row;
        _y = parrentTile.col;
        
    }

 

    private void Grab()
    {
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    private void OnMouseDown()
    {
        _previousLocation = gameObject.transform.position;
        _dragging = true;
        
       
        possibleLocations = gridManager.GetPieceMoves(_x, _y);
        var validMoves = possibleLocations;
        transform.localScale *= 1.1f;
        foreach (int mv in validMoves)
        {
            var tile = gridManager.GetTileAtPositon(new Vector2(mv % 10, mv / 10));
            tile.LitTile();
        }
    }

    private void OnMouseUp()
    {
        

        _dragging = false;


        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        transform.localScale *= 0.91f;
        if (mousePosition.x > gridManager._width - 1 || mousePosition.x < 0 || mousePosition.y > gridManager._height - 1 || mousePosition.y < 0 )
        {
            transform.position = _previousLocation;
        }

        bool isInPosible = false;
        foreach(int posVal in possibleLocations){
            int x,y = 0;
            x = posVal%10;
            y = posVal/10;
            isInPosible |= Mathf.RoundToInt(mousePosition.x) == x && Mathf.RoundToInt(mousePosition.y) == y;
        }
        if(!isInPosible){
            transform.position = _previousLocation;
        }

        

        foreach (var tile in gridManager._tiles)
        {
            tile.Value.UnLitTile();
        }

        gridManager.UpdateBoard();
        gridManager.UpdatePiece();
        UpdatePosition();
        //gridManager.EvaluateBoard();
        
        
    }


    // void ValidMoves(){
    //     var tiles = gridManager.gameObject.GetComponentsInChildren<Tile>();
    //     foreach(Tile tile in tiles){
    //         print(tile.name);
    //     }
    // }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     if(other.gameObject.tag == "Tile")
    //     {
    //         print(other.gameObject.name);
    //     }
    // }
    // private void OnMouseEnter()
    // {
    //     highLight.SetActive(true);
    // }

    // private void OnMouseExit()
    // {
    //     highLight.SetActive(false);
    // }




}
