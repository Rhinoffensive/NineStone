using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceColor
{
    white,
    black
}
public class Piece : MonoBehaviour
{
    public PieceColor color;
    [SerializeField] private GameObject highLight;

    private GridManager gridManager;
    private bool _dragging;
    private SpriteRenderer _renderer;
    private Vector2 _previousLocation;

    
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (color == PieceColor.white)
            _renderer.color = Color.white;
        else
            _renderer.color = Color.black;

        gridManager =  FindObjectOfType<GridManager>();
        


    }

    private void FixedUpdate() {
        if(!_dragging) return;
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);       
        transform.position = mousePosition;    
              
    }

    private void OnMouseDown() {
        _previousLocation = gameObject.transform.position;
        _dragging = true;
        ValidMoves();
    }

    private void OnMouseUp() {
        _dragging = false;
        
        
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        if(mousePosition.x > gridManager._width -1 || mousePosition.x < 0 || mousePosition.y > gridManager._height-1 || mousePosition.y < 0 ){
            transform.position = _previousLocation;
        }  

    }


    void ValidMoves(){
        var tiles = gridManager.gameObject.GetComponentsInChildren<Tile>();
        foreach(Tile tile in tiles){
            print(tile.name);
        }
    }

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
