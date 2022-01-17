using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GridManager : MonoBehaviour
{

    [SerializeField] public int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    //[SerializeField] private GridManager _board;
    [SerializeField] private Transform _cam;
    [SerializeField] private Piece white_piece;
    [SerializeField] private Piece black_piece;

    private int[,,] grid;



    public Dictionary<Vector2, Tile> _tiles;
    private List<Piece> _pieces;


    private void Start()
    {
        GenerateGrid();

        GeneratePieces();

       

    }

    private void Update()
    {
        //UpdateBoard();
        //UpdatePiece();
       

    }

    public void UpdateBoard()
    {
        foreach (var item in _tiles)
        {
            var piece = item.Value.GetComponentInChildren<Piece>();
            if (piece == null)
            {
                grid[item.Value.row, item.Value.col, 0] = 0;
            }
            else if (piece.color == PieceColor.white)
            {
                grid[item.Value.row, item.Value.col, 0] = 1;
            }
            else
            {
                grid[item.Value.row, item.Value.col, 0] = 2;
            }
        }
       
    }

    public void UpdatePiece()
    {
        foreach (Piece piece in _pieces)
        {
            var piece_pos = piece.transform.position;

            piece.transform.position = new Vector3(Mathf.RoundToInt(piece_pos.x), Mathf.RoundToInt(piece_pos.y));
            var tile = GetTileAtPositon(new Vector2(Mathf.RoundToInt(piece_pos.x), Mathf.RoundToInt(piece_pos.y)));
            if (tile != null)
                piece.transform.SetParent(tile.transform);

        }
        
    }


    public int EvaluateBoard()
    {
        int evalScore = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var val = grid[i, j, 0];
                if (val == (int)PieceColor.white)
                {
                    evalScore -= ((7 - i) + (7 - j));
                }
                else if (val == (int)PieceColor.black)
                {
                    evalScore += i + j;
                }
            }
        }

        print(evalScore);
        return evalScore;

    }

    public List<int> GetPieceMoves(int row, int col)
    {
        var moves = new List<int>();
        var selectedTile = grid[row, col, 0];
        if (selectedTile == 0)
            return moves;

        // 1-length moves
        var temp = new int[_width, _height, 1];
        temp = (int[,,])grid.Clone();
        // West
        if ((row - 1 >= 0) && grid[row - 1, col, 0] == 0)
        {
            temp[row - 1, col, 0] = 11;
            moves.Add((row - 1) * 10 + col);
        }
        // East
        if ((row + 1 < _width) && grid[row + 1, col, 0] == 0)
        {
            temp[row + 1, col, 0] = 11;
            moves.Add((row + 1) * 10 + col);
        }
        // North
        if ((col + 1 < _height) && grid[row, col + 1, 0] == 0)
        {
            temp[row, col + 1, 0] = 11;
            moves.Add((row * 10) + (col + 1));
        }
        // South
        if ((col - 1 >= 0) && grid[row, col - 1, 0] == 0)
        {
            temp[row, col - 1, 0] = 11;
            moves.Add((row * 10) + (col - 1));
        }

        // 2-length moves
        JumpMove(row, col, moves);

        return moves;



    }

    void JumpMove(int row, int col, List<int> moves)
    {

        // West
        if ((row - 2 >= 0) && grid[row - 2, col, 0] == 0 && grid[row - 1, col, 0] != 0)
        {
            var to = (row - 2) * 10 + col;
            if (!moves.Contains(to))
            {
                //temp[row - 1, col, 0] = 11;
                moves.Add((row - 2) * 10 + col);
                JumpMove(row - 2, col, moves);
            }
        }
        // East
        if ((row + 2 < _width) && grid[row + 2, col, 0] == 0 && grid[row + 1, col, 0] != 0)
        {
            var to = (row + 2) * 10 + col;
            //temp[row + 1, col, 0] = 11;
            if (!moves.Contains(to))
            {
                moves.Add((row + 2) * 10 + col);
                JumpMove(row + 2, col, moves);
            }
        }
        // North
        if ((col + 2 < _height) && grid[row, col + 2, 0] == 0 && grid[row, col + 1, 0] != 0)
        {
            var to = (row * 10) + (col + 2);
            //temp[row, col + 1, 0] = 11;
            if (!moves.Contains(to))
            {
                moves.Add((row * 10) + (col + 2));
                JumpMove(row, col + 2, moves);
            }
        }
        // South
        if ((col - 2 >= 0) && grid[row, col - 2, 0] == 0 && grid[row, col - 1, 0] != 0)
        {
            //temp[row, col - 1, 0] = 11;
            var to = (row * 10) + (col - 2);
            if (!moves.Contains(to))
            {
                moves.Add((row * 10) + (col - 2));
                JumpMove(row, col - 2, moves);
            }
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
        grid = new int[8, 8, 1];
        _pieces = new List<Piece>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                var tile = GetTileAtPositon(new Vector2(x, y));
                var piece = Instantiate(white_piece, new Vector3(x, y), Quaternion.identity);
                piece.transform.SetParent(tile.transform);
                _pieces.Add(piece);
                grid[x, y, 0] = 1;
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
                grid[x, y, 0] = 2;
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
