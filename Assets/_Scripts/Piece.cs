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
        _dragging = true;
    }

    private void OnMouseUp() {
        _dragging = false;
        
        
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

    
        

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
