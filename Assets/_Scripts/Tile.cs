using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
   [SerializeField] private Color _baseColor,_offsetColor;
   [SerializeField] private SpriteRenderer _renderer;
   [SerializeField] public int row,col;
   [SerializeField] private GameObject _highlight;

   private Color _orjColor;


   public void Init(bool isOffset, int col, int row){
       _renderer.color = isOffset ? _offsetColor:_baseColor;
       this.row = row;
       this.col = col;

   }


   public void LitTile(){       
       _highlight.SetActive(true);
   }

     public void UnLitTile(){       
       _highlight.SetActive(false);
   }

   private void OnMouseEnter() {
       //LitTile();
   }

   private void OnMouseExit() {
       //UnLitTile();
   }


   private void Start() {
       
   }

   private void Update() {
       
   }
   

}
