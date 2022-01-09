using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceColor{
    white,
    black
}
public class Piece : MonoBehaviour
{
    public PieceColor color;
    [SerializeField]private GameObject highLight;
    private bool isHighLighted = false;
    private SpriteRenderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if(color == PieceColor.white)
            _renderer.color = Color.white;
        else
            _renderer.color = Color.black;


    }

    private void OnMouseEnter()
    {
        isHighLighted = true;
        if(!isHighLighted)
            highLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        
        isHighLighted = false;
        if(isHighLighted)
            highLight.SetActive(false);
    }




}
