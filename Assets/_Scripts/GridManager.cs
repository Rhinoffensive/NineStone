using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] public int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    //[SerializeField] private GridManager _board;
    [SerializeField] private Transform _cam;
    [SerializeField] private Piece white_piece;
    [SerializeField] private Piece black_piece;



    private Dictionary<Vector2, Tile> _tiles;
    private List<Piece> _pieces;


    private void Start()
    {
        GenerateGrid();

        GeneratePieces();

        
    }

    private void FixedUpdate()
    {      
        foreach(Piece piece in _pieces){
            var piece_pos = piece.transform.position;  
           
            piece.transform.position = new Vector3(Mathf.RoundToInt(piece_pos.x),Mathf.RoundToInt(piece_pos.y));
            var tile = GetTileAtPositon(new Vector2(Mathf.RoundToInt(piece_pos.x),Mathf.RoundToInt(piece_pos.y)));
            if(tile!=null)
                piece.transform.SetParent(tile.transform);
         
        }
    }



   
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 1), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, x, y);

                _tiles[new Vector2(x, y)] = spawnedTile;

                spawnedTile.transform.SetParent(this.transform);
              

            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10f);
    }

    void GeneratePieces()
    {

        _pieces = new List<Piece>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                var tile = GetTileAtPositon(new Vector2(x, y));
                var piece = Instantiate(white_piece, new Vector3(x, y), Quaternion.identity);                
                piece.transform.SetParent(tile.transform);
                _pieces.Add(piece);
            }
        }

        for (int x = 7; x > 4; x--)
        {
            for (int y = 7; y > 4; y--)
            {
                var tile = GetTileAtPositon(new Vector2(x, y));
                var piece = Instantiate(black_piece, new Vector3(x, y), Quaternion.identity);
                piece.transform.SetParent(tile.transform);
                _pieces.Add(piece);
            }
        }


    }
    public Tile GetTileAtPositon(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }




}
