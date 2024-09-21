using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace BitBoardCore
{
    public class BitBoardGame
    {
        

        public const int PLAYER_1 = 0;                      // Constant for the player 1 index
        public const int PLAYER_2 = 1;                      // Constant for the player 2 index

        public const int CELL_SIZE = 60;                    // Constant for the rendering cell size in pixels

        public const int BOARD_COLUMNS = 8;                 // Constant number of board columns
        public const int BOARD_ROWS = 8;                    // Constant number of board rows

        uint[] _pieces;                                     // The bitboards that represent the pieces for each player
        uint _kingStatusMask;                               // The state for each cell that represents if that piece is kinged or not
        int _currentTurn;                                   // The turn number

        int[] _hoverCellIndex;                              // A 2-element array that represents the cursor position in x,y coordinates
        int[] _selectedCellIndex;                           // A 2-element array that represents the selected cell position in x,y coordinates

        uint _hoverBitMask;                                 // The bitmask representation of the cursor
        uint _selectedBitMask;                              // The bitmask representation of the selected cell

        uint _potentialMoveBitMask;                         // The bitmask that represents the calculated potential moves for the selected piece (if there is one)

        bool _isGameOver;                                   // Is the game over?
        int _gameWinner;                                    // The winner of the game 0 for PLAYER_1, 1 for PLAYER_2, and -1 for a draw

        Image _checkerIcon;
        Image _kingIcon;
        Image _moveIcon;
        ColorMatrix[] _pieceColorMatrices;
        ColorMatrix _uiColorMatrix;

        public delegate void BoardChangeCallback(uint player1, uint player2);
        public BoardChangeCallback? onBoardChange;           // A callback that is called to update UI if the board changes

        public delegate void TurnChangeCallback(int onTurnChange);
        public TurnChangeCallback onTurnChange;

        public delegate void GameOverCallback(int gameState);
        public GameOverCallback onGameOver;

        public uint player1BitBoard
        {
            get
            {
                return _pieces[0];
            }
        }

        public uint player2BitBoard
        {
            get
            {
                return _pieces[1];
            }
        }

        public uint hoverBitMask
        {
            get
            {
                return _hoverBitMask;
            }
        }

        public bool IsCellSelected()
        {
            return _selectedCellIndex[0] > -1 && _selectedCellIndex[1] > -1;
        }

        /// <summary>
        /// Converts an x,y board index to a bit mask
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public uint CreateMaskFromBoardIndex(int x, int y)
        {
            // If the indices are out of bounds then return 0
            if(x < 0 || y < 0 || x >= BOARD_COLUMNS || y >= BOARD_ROWS)
            {
                return 0;
            }
            // Convert the x and y coordinates to the bitfield
            // If the numerator is evenly divisible by 2, then use it to leftshift 1
            int numerator = (y * BOARD_COLUMNS) + (x - (y % 2));
            if ((numerator % 2) == 0)
            {
                return 1u << (numerator / 2);
            }
            return 0;

        }

        public BitBoardGame()
        {
            _pieces = new uint[2];
            _currentTurn = -1;
            _hoverCellIndex = [-1, -1];
            _selectedCellIndex = [-1, -1];
            _hoverBitMask = 0;
            _selectedBitMask = 0;
            onBoardChange = null;
            _pieceColorMatrices = new ColorMatrix[2];
            _gameWinner = -1;
            _isGameOver = false;
            
        }

        private void InitializeResources()
        {
            _pieceColorMatrices[0] = RenderingUtility.GetColorMatrix(Color.Red);
            _pieceColorMatrices[1] = RenderingUtility.GetColorMatrix(Color.FromArgb(255,50,50,50));

            _uiColorMatrix = RenderingUtility.GetColorMatrix(Color.GreenYellow);
        }

        public void SetResources(Image checkerIcon, Image kingIcon, Image moveIcon)
        {
            _checkerIcon = checkerIcon;
            _kingIcon = kingIcon;
            _moveIcon = moveIcon;

            InitializeResources();
        }

        /// <summary>
        /// Initialize the board to the starting state
        /// </summary>
        public void InitializeBoard()
        {
            _pieces[PLAYER_1] = BoardBitStates.INITIAL_PLAYER_1_STATE;
            _pieces[PLAYER_2] = BoardBitStates.INITIAL_PLAYER_2_STATE;
            _kingStatusMask = 0u;
            UpdateUI();

            IncrementTurn();
        }

        protected void IncrementTurn()
        {
            // Deselct the cell and clear the potential moves
            SetSelectedCell(-1, -1);
            _potentialMoveBitMask = 0u;

            // Increment the turn
            ++_currentTurn;

            UpdateGameState();

            if(_isGameOver)
            {
                if(onGameOver != null)
                {
                    onGameOver(_gameWinner);
                }
            }

            if (onTurnChange != null)
            {
                onTurnChange(this._currentTurn);
            }
        }

        /// <summary>
        /// Get the other player id based on the input playerID
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public int GetOtherPlayerID(int playerID)
        {
            return ((playerID + 1) % 2);
        }

        /// <summary>
        /// Move the piece from the specified location to the specified location
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="fromBit"></param>
        /// <param name="toBit"></param>
        public void MovePiece(int playerID, int fromBit, int toBit)
        {
            Debug.Write("From: " + fromBit + " To:" + toBit);
            // Move the bit to the new position from the old position
            _pieces[playerID] = BitUtility.MoveBit(_pieces[playerID], fromBit, toBit);
            _kingStatusMask = BitUtility.MoveBit(_kingStatusMask, fromBit, toBit);
            // Get the id of the other player
            int otherPlayerID = GetOtherPlayerID(playerID);

            // Check to see if the cell is occupied by a piece on the other team
            if (BitUtility.CheckBit(_pieces[otherPlayerID], toBit) == 1)
            {
                // If so, capture it
                CapturePiece(otherPlayerID, toBit);
            }

            UpdateUI();
            IncrementTurn();
        }

        public bool IsLegalMove(int playerID, int fromBit, int toBit)
        {
            return true;
        }

        public void CapturePiece(int playerID, int bit)
        {

        }

        public override string ToString()
        {
            return ToString("b");
        }

        public string ToString(string format)
        {
            return "Board";
        }

        public void UpdateGameState()
        {

        }

        private void CalculatePotentialMoves()
        {
            // Generate the potential moves based on the selected piece
            // Clear the potential moves
            _potentialMoveBitMask = 0u;

            // If nothing is selected then return
            if (_selectedBitMask == 0)
            {
                return;
            }

            // figure out which pieces team is selected
            int teamIndex = -1;
            for(int i = 0; i < _pieces.Length; i++)
            {
                if(BitUtility.BitMatch(_selectedBitMask, _pieces[i]))
                {
                    teamIndex = i;
                    break;
                }
            }
            // If teamIndex == -1 then there is no piece selected
            if(teamIndex == -1)
            {
                return;
            }

            int otherTeamIndex = GetOtherPlayerID(teamIndex);
            uint myTeamBitmask = _pieces[teamIndex];
            uint otherTeamBitmask = _pieces[otherTeamIndex];
            uint allPiecesMask = _pieces[0] | _pieces[1];
            bool isKing = ((_selectedBitMask & _kingStatusMask) != 0);

            // If team index is 0 (player 1)
            if ((teamIndex == PLAYER_1) || isKing)
            {
                // Calculate the potential non jump moves based on the position of the checker
                // 0 8 16 24 
                if(BitUtility.BitMatch(_selectedBitMask, 0x1010101u))
                {
                    uint targetBitMask = _selectedBitMask << 4;
                    _potentialMoveBitMask |= (targetBitMask & ~allPiecesMask);    
                }
                // 28 29 30 31
                else if (BitUtility.BitMatch(_selectedBitMask, 0xF0000000u))
                {
                    // No moves
                } 
                // every other position
                else
                {
                    uint targetBitMask = (_selectedBitMask << 3) | (_selectedBitMask << 4);
                    _potentialMoveBitMask |= (targetBitMask & ~allPiecesMask);
                }

                // Calculate the potential jump moves based on the position of the checker
                _potentialMoveBitMask |= TestJumps(teamIndex, _selectedBitMask, myTeamBitmask, otherTeamBitmask, isKing);
            }

            // If team index is 1 (player 2)
            if ((teamIndex == PLAYER_2) || isKing)
            {
                // Calculate the potential non jump moves based on the position of the checker
                // 0 8 16 24 
                if (BitUtility.BitMatch(_selectedBitMask, 0x1010100u))
                {
                    uint targetBitMask = _selectedBitMask >> 3;
                    _potentialMoveBitMask |= (targetBitMask & ~allPiecesMask);
                }
                // 0 1 2 3
                else if (BitUtility.BitMatch(_selectedBitMask, 0x0000000Fu))
                {
                    // No moves
                }
                // every other position
                else
                {
                    uint targetBitMask = (_selectedBitMask >> 3) | (_selectedBitMask >> 4);
                    _potentialMoveBitMask |= (targetBitMask & ~allPiecesMask);
                }

                // Calculate the potential jump moves based on the position of the checker
                _potentialMoveBitMask |= TestJumps(teamIndex, _selectedBitMask, myTeamBitmask, otherTeamBitmask, isKing);
            }
        }

        private uint TestJumps(int teamIndex, uint positionMask, uint myPieceMask, uint otherPieceMask, bool isKing)
        {
            uint allPiecesMask = myPieceMask | otherPieceMask;
            uint branchMask = 0u;
            // If player 1 or the piece is a king
            if(teamIndex == PLAYER_1 || isKing)
            {
                // Check to see if the selected bit mask is in 0xEEEEEEu

                // ░░::░░::░░::░░::
                // ::░░::░░::░░::░░
                // ░░::░░▉▉░░▉▉░░▉▉
                // ::░░▉▉░░▉▉░░▉▉░░
                // ░░::░░▉▉░░▉▉░░▉▉
                // ::░░▉▉░░▉▉░░▉▉░░
                // ░░::░░▉▉░░▉▉░░▉▉
                // ::░░▉▉░░▉▉░░▉▉░░


                if (BitUtility.BitMatch(_selectedBitMask, 0xEEEEEEu))
                {

                    if(((positionMask << 3) & otherPieceMask) != 0)
                    {
                        uint targetBitMask = positionMask << 7;
                        if ((targetBitMask & ~allPiecesMask) != 0)
                        {
                            _potentialMoveBitMask |= targetBitMask;

                            _potentialMoveBitMask |= TestJumps(
                                teamIndex, 
                                targetBitMask, 
                                (myPieceMask & ~positionMask) | targetBitMask, 
                                otherPieceMask,
                                isKing
                                );
                        }
                            
                    }
                    
                }

                // ░░::░░::░░::░░::
                // ::░░::░░::░░::░░
                // ░░▉▉░░▉▉░░▉▉░░::
                // ▉▉░░▉▉░░▉▉░░::░░
                // ░░▉▉░░▉▉░░▉▉░░::
                // ▉▉░░▉▉░░▉▉░░::░░
                // ░░▉▉░░▉▉░░▉▉░░::
                // ▉▉░░▉▉░░▉▉░░::░░

                else if (BitUtility.BitMatch(_selectedBitMask, 0x777777u))
                {
                    if (((positionMask << 4) & otherPieceMask) != 0)
                    {
                        uint targetBitMask = positionMask << 9;
                        if ((targetBitMask & ~allPiecesMask) != 0)
                        {
                            _potentialMoveBitMask |= targetBitMask;

                            _potentialMoveBitMask |= TestJumps(
                                teamIndex,
                                targetBitMask,
                                (myPieceMask & ~positionMask) | targetBitMask,
                                otherPieceMask,
                                isKing
                                );
                        }

                    }
                }

                // ░░::░░::░░::░░::
                // ::░░::░░::░░::░░
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░
                else if (BitUtility.BitMatch(_selectedBitMask, 0xFF000000u))
                {
                    // No jumps
                }
                else
                {
                    if (((positionMask << 4) & otherPieceMask) != 0)
                    {
                        uint targetBitMask = positionMask << 9;
                        if ((targetBitMask & ~allPiecesMask) != 0)
                        {
                            _potentialMoveBitMask |= targetBitMask;

                            _potentialMoveBitMask |= TestJumps(
                                teamIndex,
                                targetBitMask,
                                (myPieceMask & ~positionMask) | targetBitMask,
                                otherPieceMask,
                                isKing
                                );
                        }

                    }
                    if (((positionMask << 3) & otherPieceMask) != 0)
                    {
                        uint targetBitMask = positionMask << 7;
                        if ((targetBitMask & ~allPiecesMask) != 0)
                        {
                            _potentialMoveBitMask |= targetBitMask;

                            _potentialMoveBitMask |= TestJumps(
                                teamIndex,
                                targetBitMask,
                                (myPieceMask & ~positionMask) | targetBitMask,
                                otherPieceMask,
                                isKing
                                );
                        }

                    }
                }
            }
            // If player 2 or the piece is a king
            if (teamIndex == PLAYER_2 || ((positionMask & _kingStatusMask) != 0))
            {

            }

            return branchMask;
        }

        public void UpdateUI()
        {
            if (onBoardChange != null)
            {
                onBoardChange(_pieces[0], _pieces[1]);
            }
        }

        public void Render(System.Drawing.Graphics graphics)
        {

            graphics.Clear(Color.Black);

            RenderBoard(graphics);

            RenderPieces(graphics);

            RenderUI(graphics);
            

            //graphics.FillRectangle(darkBrush, new Rectangle() { X = 0, Y = 0, Height = CELL_SIZE, Width = CELL_SIZE});
        }
        
        protected void RenderBoard(System.Drawing.Graphics graphics)
        {
            Brush darkBrush = new SolidBrush(Color.Brown);
            Brush lightBrush = new SolidBrush(Color.Tan);

            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (((i + j) % 2) == 1)
                    {
                        graphics.FillRectangle(darkBrush, new Rectangle() { X = CELL_SIZE * j, Y = CELL_SIZE * i, Height = CELL_SIZE, Width = CELL_SIZE });
                    }
                    else
                    {
                        graphics.FillRectangle(lightBrush, new Rectangle() { X = CELL_SIZE * j, Y = CELL_SIZE * i, Height = CELL_SIZE, Width = CELL_SIZE });
                    }
                }
            }
        }

        protected void RenderPiece(System.Drawing.Graphics graphics, ColorMatrix color, int cx, int cy, bool isSelected)
        {
            int size = (int)(CELL_SIZE * 0.75f);
            Rectangle rect = new Rectangle() { X = CELL_SIZE * cx + size / 5, Y = (CELL_SIZE * 7) - (CELL_SIZE * cy) + size / 5, Height = size, Width = size };
            //graphics.FillEllipse(brush, rect);
            RenderingUtility.RenderImage(graphics, _checkerIcon, rect, color);

            if(isSelected)
            {
                Pen selectedPen = new Pen(Color.GreenYellow, 5);
                graphics.DrawEllipse(selectedPen, rect);
            }

        }

        protected void RenderPieces(System.Drawing.Graphics graphics)
        {
            for(int i = 0; i < sizeof(uint)*8; i++)
            {
                int boardIndex = i * 2;
                int y = boardIndex / BOARD_COLUMNS;
                int x = boardIndex - (y * BOARD_COLUMNS) + (y % 2);     // Get the x position based on the cell id

                bool isSelected = _selectedCellIndex[0] == x && _selectedCellIndex[1] == y;


                if(BitUtility.CheckBit(_pieces[PLAYER_1], i) == 1)
                {
                    RenderPiece(graphics, _pieceColorMatrices[PLAYER_1], x, y, isSelected);
                }
                if (BitUtility.CheckBit(_pieces[PLAYER_2], i) == 1)
                {
                    RenderPiece(graphics, _pieceColorMatrices[PLAYER_2], x, y, isSelected);
                }
            }
        }

        protected void RenderUI(System.Drawing.Graphics graphics)
        {
            
            // Draw the potential moves
            if (_potentialMoveBitMask != 0)
            {
                for (int i = 0; i < sizeof(uint) * 8; i++)
                {
                    int boardIndex = i * 2;
                    int y = boardIndex / BOARD_COLUMNS;
                    int x = boardIndex - (y * BOARD_COLUMNS) + (y % 2);     // Get the x position based on the cell id
                    Rectangle cellRect = new Rectangle() { X = CELL_SIZE * x, Y = (CELL_SIZE * 7) - (CELL_SIZE * y), Height = CELL_SIZE, Width = CELL_SIZE };

                    if (BitUtility.CheckBit(_potentialMoveBitMask, i) != 0)
                    {
                        RenderingUtility.RenderImage(graphics, _moveIcon, cellRect, _uiColorMatrix);
                    }
                }
                
            }

            Rectangle rect = new Rectangle() { X = CELL_SIZE * _hoverCellIndex[0], Y = CELL_SIZE * (7 - _hoverCellIndex[1]), Height = CELL_SIZE, Width = CELL_SIZE };

            // Draw the cursor
            if (_hoverBitMask != 0)
            {
                Pen pen = new Pen(Color.GreenYellow, 5);
                graphics.DrawRectangle(pen, rect);
            }

            
        }

        public void OnMouseDown(int x, int y)
        {
            if(IsCellSelected())
            {
                // If we clicked on the selected sell, deselect it
                if (_hoverBitMask == _selectedBitMask)
                {
                    SetSelectedCell(-1, -1);
                    _potentialMoveBitMask = 0;
                }

                if(BitUtility.BitMatch(_hoverBitMask, _potentialMoveBitMask))
                {
                    MovePiece(_currentTurn % 2, BitUtility.GetFirstBitPosition(_selectedBitMask), BitUtility.GetFirstBitPosition(_hoverBitMask));
                }
            }
            else
            {
                if(!IsHoverCellValid())
                {
                    return;
                }

                // If there is a piece on the cell we are hovering on and it is the current team, select it
                if(BitUtility.BitMatch(_hoverBitMask, _pieces[_currentTurn%2]))
                {
                    SetSelectedCell();
                    CalculatePotentialMoves();
                }
                
                
                
            }
        }

        /// <summary>
        /// Sets the selected cell to the hover cell position
        /// </summary>
        public void SetSelectedCell()
        {
            _selectedCellIndex[0] = _hoverCellIndex[0];
            _selectedCellIndex[1] = _hoverCellIndex[1];
            _selectedBitMask = _hoverBitMask;
        }

        /// <summary>
        /// Sents the selected cell position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetSelectedCell(int x, int y)
        {
            _selectedCellIndex[0] = x;
            _selectedCellIndex[1] = y;
            _selectedBitMask = CreateMaskFromBoardIndex(_selectedCellIndex[0], _selectedCellIndex[1]);
        }

        public void SetHoverCell(int x, int y)
        {
            _hoverCellIndex[0] = x / CELL_SIZE;
            _hoverCellIndex[1] = 7 - (y / CELL_SIZE);

            if (_hoverCellIndex[0] > BOARD_COLUMNS - 1)
            {
                _hoverCellIndex[0] = -1;
            }
            if (_hoverCellIndex[1] > BOARD_ROWS - 1)
            {
                _hoverCellIndex[1] = -1;
            }
            // Get the hover bit mask
            _hoverBitMask = CreateMaskFromBoardIndex(_hoverCellIndex[0], _hoverCellIndex[1]);
        }

        public void SetMousePosition(int x, int y)
        {
            SetHoverCell(x, y);

        }

        private bool IsHoverCellValid()
        {
            return (_hoverCellIndex[0] > -1) && (_hoverCellIndex[1] > -1);
        }
    }
}
