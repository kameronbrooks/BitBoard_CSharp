using System.Drawing;
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
        uint[] _potentialCaptures;                          // List of bitmasks that represents the captures resulting from each potential move

        bool _isGameOver;                                   // Is the game over?
        int _gameWinner;                                    // The winner of the game 0 for PLAYER_1, 1 for PLAYER_2, and -1 for a draw

        Image _checkerIcon;                                 // The icon for normal checkers
        Image _kingIcon;                                    // The icon for a king
        Image _moveIcon;                                    // The icon that shows potential moves
        ColorMatrix[] _pieceColorMatrices;                  // Matricies used to store the colors for rendering pieces
        ColorMatrix _uiColorMatrix;                         // The matrix for the UI color

        public delegate void BoardChangeCallback(uint player1, uint player2);
        public BoardChangeCallback? onBoardChange;           // A callback that is called to update UI if the board changes

        public delegate void TurnChangeCallback(int onTurnChange);
        public TurnChangeCallback onTurnChange;              // A callback that is called when the turn changes

        public delegate void GameOverCallback(int gameState);
        public GameOverCallback onGameOver;                  // A callback that is called when the game is over

        /// <summary>
        /// Player 1 Bit Board bits
        /// </summary>
        public uint player1BitBoard
        {
            get
            {
                return _pieces[0];
            }
        }

        /// <summary>
        /// Player 2 Bit Board bits
        /// </summary>
        public uint player2BitBoard
        {
            get
            {
                return _pieces[1];
            }
        }

        /// <summary>
        /// The bit mask that represents the current cursor position
        /// </summary>
        public uint hoverBitMask
        {
            get
            {
                return _hoverBitMask;
            }
        }

        public bool isGameOver()
        {
            return _isGameOver;
        }

        public bool isGameActive()
        {
            return !_isGameOver;
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
                return BitUtility.ShiftLeft(1u, numerator / 2);
            }
            return 0;

        }

        public BitBoardGame()
        {
            _pieces = new uint[2];
            _potentialCaptures = new uint[sizeof(uint) * 8];
            _currentTurn = -1;
            _hoverCellIndex = [-1, -1];
            _selectedCellIndex = [-1, -1];
            _hoverBitMask = 0u;
            _selectedBitMask = 0u;
            _kingStatusMask = 0u;
            onBoardChange = null;
            _pieceColorMatrices = new ColorMatrix[2];
            _gameWinner = -1;
            _isGameOver = false;
            
        }

        /// <summary>
        /// Initialize the resources that the game needs for rendering
        /// </summary>
        private void InitializeResources()
        {
            _pieceColorMatrices[0] = RenderingUtility.GetColorMatrix(Color.FromArgb(255, 255, 50, 50));
            _pieceColorMatrices[1] = RenderingUtility.GetColorMatrix(Color.FromArgb(255, 50, 50, 50));

            _uiColorMatrix = RenderingUtility.GetColorMatrix(Color.GreenYellow);
        }

        /// <summary>
        /// Add the image resources and initialize the other rendering resources
        /// </summary>
        /// <param name="checkerIcon"></param>
        /// <param name="kingIcon"></param>
        /// <param name="moveIcon"></param>
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

        private void IncrementTurn()
        {
            // Deselct the cell and clear the potential moves
            SetSelectedCell(-1, -1);
            ClearPotential();

            // Increment the turn
            ++_currentTurn;

            UpdateGameState();

            // Call the callback if not null to notify handler of turn change
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
            // Move the bit to the new position from the old position
            _pieces[playerID] = BitUtility.MoveBit(_pieces[playerID], fromBit, toBit);
            _kingStatusMask = BitUtility.MoveBit(_kingStatusMask, fromBit, toBit);

            // Remove pieces that have been captured
            int otherTeamID = GetOtherPlayerID(playerID);
            _pieces[otherTeamID] &= ~_potentialCaptures[toBit];
            _kingStatusMask &= ~_potentialCaptures[toBit];

            // Once the piece moves, continue on to the next turn
            UpdateUI();
            IncrementTurn();
        }


        /// <summary>
        /// Returns a string representation of the game
        /// Default is ascii
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("ascii");
        }

        /// <summary>
        /// Use the options, "b" for binary, "hex" for hex, or "ascii" for ascii
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            if(format=="b")
            {
                string player1 = BitUtility.ToBinaryString(_pieces[0]);
                string player2 = BitUtility.ToBinaryString(_pieces[0]);
                string kingData = BitUtility.ToBinaryString(_kingStatusMask);

                return "Player 1\n" + player1 + "\nPlayer 2\n" + player2 + "\nKingStatus\n" + kingData;
            }
            else if(format=="hex")
            {
                string player1 = BitUtility.ToHexString(_pieces[0]);
                string player2 = BitUtility.ToHexString(_pieces[0]);
                string kingData = BitUtility.ToHexString(_kingStatusMask);

                return "Player 1\n" + player1 + "\nPlayer 2\n" + player2 + "\nKingStatus\n" + kingData;
            }
            else
            {
                // Print the ascii representation of the game
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░
                // ░░▉▉░░▉▉░░▉▉░░▉▉
                // ▉▉░░▉▉░░▉▉░░▉▉░░

                const string DARK_CELL = "▉▉";

                string output = "";
                for(int y= (BOARD_ROWS)-1; y>=0;y--)
                {
                    for (int x = 0; x < BOARD_COLUMNS/2; x++)
                    {
                        uint mask = BitUtility.CreateBitMask((int)x + (int)y * (BOARD_COLUMNS/2));

                        string pieceChar = BitUtility.BitMatch(mask, _pieces[0]) ? "R" : BitUtility.BitMatch(mask, _pieces[1]) ? "B" : " ";
                        string kingChar = BitUtility.BitMatch(mask, _kingStatusMask) ? "K" : " ";

                        // Check to see if we are on an even row or and odd one
                        // Then show the approprate ascii representation
                        if (BitUtility.BitMatch(mask, BoardBitStates.EVEN_ROWS))
                        {
                            output += pieceChar + kingChar + DARK_CELL;
                        }
                        else
                        {
                            output += DARK_CELL + pieceChar + kingChar;
                        }
                    }
                    output += "\n";
                }

                return output;
            }
        }

        /// <summary>
        /// This function returns true if there are any moves that the player can play
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        private bool DoesPlayerHaveMoves(int playerID)
        {
            // Check all pieces to see if any can move
            for(int i = 0; i < sizeof(uint)*8; i++)
            {
                // If the player does not have a piece here, then continue
                if (BitUtility.CheckBit(_pieces[playerID], i) == 0u)
                {
                    continue;
                }
                if(CalculatePotentialMoves(BitUtility.CreateBitMask(i))) {
                    // Clear any side effects of the calculations
                    ClearPotential();
                    return true;
                }
            }
            // Clear any side effects of the calculations
            ClearPotential();
            // Return false if none of the pieces could move
            return false;
        }

        public void UpdateGameState()
        {
            // Check for game over
            // First check both teams to see if there are any that are completely out of pieces
            if (_pieces[0] == 0u)
            {
                OnGameOver(PLAYER_2);
                return;
            }
            if (_pieces[1] == 0u)
            {
                OnGameOver(PLAYER_1);
                return;
            }

            // Check to see if the current player does not have any moves to play
            if (!DoesPlayerHaveMoves(_currentTurn%2))
            {
                OnGameOver((_currentTurn+1) % 2);
                return;
            }

            // Update king status
            // King any men that have gotten to the end
            _kingStatusMask = BitUtility.CombineBits(_kingStatusMask, _pieces[0] & BoardBitStates.INITIAL_PLAYER_1_FINALROW);
            _kingStatusMask = BitUtility.CombineBits(_kingStatusMask, _pieces[1] & BoardBitStates.INITIAL_PLAYER_2_FINALROW);

        }

        private void OnGameOver(int winner)
        {
            _isGameOver = true;
            if(onGameOver != null)
            {
                onGameOver(winner);
            }
        }

        /// <summary>
        /// Clear the potentialMoveMask and the potentialBoardStates for each move
        /// </summary>
        private void ClearPotential()
        {
            // Clear the potential moves
            _potentialMoveBitMask = 0u;
            for(int i = 0; i<_potentialCaptures.Length; i++)
            {
                _potentialCaptures[i] = 0u;
            }
        }

        /// <summary>
        /// Calculate the potential moves for the piece at the given bit position in the bit mask
        /// </summary>
        /// <param name="selectedBitMask"></param>
        /// <returns></returns>
        private bool CalculatePotentialMoves(uint selectedBitMask)
        {
            // Generate the potential moves based on the selected piece
            ClearPotential();

            
            // If nothing is selected then return
            if (selectedBitMask == 0)
            {
                return false;
            }

            // figure out which pieces team is selected
            int teamIndex = -1;
            for(int i = 0; i < _pieces.Length; i++)
            {
                if(BitUtility.BitMatch(selectedBitMask, _pieces[i]))
                {
                    teamIndex = i;
                    break;
                }
            }
            // If teamIndex == -1 then there is no piece selected
            if(teamIndex == -1)
            {
                return false;
            }

            int otherTeamIndex = GetOtherPlayerID(teamIndex);
            uint myTeamBitmask = _pieces[teamIndex];
            uint otherTeamBitmask = _pieces[otherTeamIndex];
            uint allPiecesMask = BitUtility.CombineBits(_pieces[0], _pieces[1]);
            //bool isKing = ((_selectedBitMask & _kingStatusMask) != 0);
            bool isKing = BitUtility.BitMatch(selectedBitMask, _kingStatusMask);

            // Check for captures moves first since it is mandatory to capture if you can
            _potentialMoveBitMask = BitUtility.CombineBits(_potentialMoveBitMask, TestJumps(teamIndex, selectedBitMask, myTeamBitmask, otherTeamBitmask, isKing, 0u));

            if (_potentialMoveBitMask != 0u)
            {
                // Exit now, because we want the captures to be mandatory. The player cannot chose a non capture move if there is already something found
                return true;
            }

            // If the piece belongs player 1 or the piece is a king, calculate the forward moves 
            if ((teamIndex == PLAYER_1) || isKing)
            {
                // Check to see if the bit is in the player 1 jump state area
                if (BitUtility.BitMatch(BoardBitStates.PLAYER1_LMOVE_MASK, selectedBitMask))
                {
                    // Calculate the jumpOffset and captureOffset based on if the positionBit is on an even or odd row
                    int jumpBitOffset = BitUtility.BitMatch(BoardBitStates.EVEN_ROWS, selectedBitMask) ? 3 : 4;

                    uint targetBitMask = BitUtility.ShiftLeft(selectedBitMask, jumpBitOffset);

                    // If there is an enemy piece in the capture position and nothing in the jump position
                    if (BitUtility.BitMatch(targetBitMask, ~allPiecesMask))
                    {
                        _potentialMoveBitMask = BitUtility.CombineBits(_potentialMoveBitMask, targetBitMask);
                    }
                }

                // Check to see if the bit is in the player 1 jump state area
                if (BitUtility.BitMatch(BoardBitStates.PLAYER1_RMOVE_MASK, selectedBitMask))
                {
                    // Calculate the jumpOffset and captureOffset based on if the positionBit is on an even or odd row
                    int jumpBitOffset = BitUtility.BitMatch(BoardBitStates.EVEN_ROWS, selectedBitMask) ? 4 : 5;

                    uint targetBitMask = BitUtility.ShiftLeft(selectedBitMask, jumpBitOffset);

                    // If there is an enemy piece in the capture position and nothing in the jump position
                    if (BitUtility.BitMatch(targetBitMask, ~allPiecesMask))
                    {
                        _potentialMoveBitMask = BitUtility.CombineBits(_potentialMoveBitMask, targetBitMask);
                    }
                }

            }
            // If the piece belongs player 2 or the piece is a king, calculate the backward moves 
            if ((teamIndex == PLAYER_2) || isKing)
            {
                // Check to see if the bit is in the player 1 jump state area
                if (BitUtility.BitMatch(BoardBitStates.PLAYER2_LMOVE_MASK, selectedBitMask))
                {
                    // Calculate the jumpOffset and captureOffset based on if the positionBit is on an even or odd row
                   
                    int jumpBitOffset = BitUtility.BitMatch(BoardBitStates.EVEN_ROWS, selectedBitMask) ? 5 : 4;

                    uint targetBitMask = BitUtility.ShiftRight(selectedBitMask, jumpBitOffset);

                    // If there is an enemy piece in the capture position and nothing in the jump position
                    if (BitUtility.BitMatch(targetBitMask, ~allPiecesMask))
                    {
                        _potentialMoveBitMask = BitUtility.CombineBits(_potentialMoveBitMask, targetBitMask);
                    }
                }

                // Check to see if the bit is in the player 1 jump state area
                if (BitUtility.BitMatch(BoardBitStates.PLAYER2_RMOVE_MASK, selectedBitMask))
                {
                    // Calculate the jumpOffset and captureOffset based on if the positionBit is on an even or odd row

                    int jumpBitOffset = BitUtility.BitMatch(BoardBitStates.EVEN_ROWS, selectedBitMask) ? 4 : 3;
                    uint targetBitMask = BitUtility.ShiftRight(selectedBitMask, jumpBitOffset);

                    // If there is an enemy piece in the capture position and nothing in the jump position
                    if (BitUtility.BitMatch(targetBitMask, ~allPiecesMask))
                    {
                        _potentialMoveBitMask = BitUtility.CombineBits(_potentialMoveBitMask, targetBitMask);
                    }
                }

            }

            

            // Return true if there are any moves added to this potential move bitmask. If it is still 0, then there are no available moves
            return _potentialMoveBitMask != 0u;

        }

        /// <summary>
        /// Called by the Calculate potential moves method to specifically handle the jumps. This is its own function because it needs to run recursively.
        /// </summary>
        /// <param name="teamIndex"></param>
        /// <param name="positionMask"></param>
        /// <param name="myPieceMask"></param>
        /// <param name="otherPieceMask"></param>
        /// <param name="isKing"></param>
        /// <param name="captureMask"></param>
        /// <returns></returns>
        private uint TestJumps(int teamIndex, uint positionMask, uint myPieceMask, uint otherPieceMask, bool isKing, uint captureMask)
        {
            uint allPiecesMask = BitUtility.CombineBits(myPieceMask, otherPieceMask);
            uint branchMask = 0u;

            // Create a local function to better reuse the code with minimal repetition
            var _TestBranch = (uint jumpMask, int evenJumpOffset, int oddJumpOffset, int evenCaptureOffset, int oddCaptureOffset) =>
            {
                if (BitUtility.BitMatch(jumpMask, positionMask))
                {
                    // Calculate the jumpOffset and captureOffset based on if the positionBit is on an even or odd row
                    int jumpBitOffset = BitUtility.BitMatch(BoardBitStates.EVEN_ROWS, positionMask) ? evenJumpOffset : oddJumpOffset;
                    int captureBitOffset = BitUtility.BitMatch(BoardBitStates.EVEN_ROWS, positionMask) ? evenCaptureOffset : oddCaptureOffset;

                    // If the offset is negative, shift right instead of left and make the offset positive
                    uint targetBitMask = (jumpBitOffset >= 0) ? BitUtility.ShiftLeft(positionMask, jumpBitOffset) : BitUtility.ShiftRight(positionMask, -jumpBitOffset);
                    uint capturePieceBitMask = (captureBitOffset >= 0) ? BitUtility.ShiftLeft(positionMask, captureBitOffset) : BitUtility.ShiftRight(positionMask, -captureBitOffset);

                    uint branchCaptureMask = captureMask;

                    // If there is an enemy piece in the capture position and nothing in the jump position
                    if (BitUtility.BitMatch(capturePieceBitMask, otherPieceMask) && BitUtility.BitMatch(targetBitMask, ~allPiecesMask))
                    {
                        // Add the target bitmask to the potential move set
                        branchMask = BitUtility.CombineBits(branchMask, targetBitMask);
                        // Add this capture bitmask to the capture set
                        branchCaptureMask = BitUtility.CombineBits(branchCaptureMask, capturePieceBitMask);

                        // Perform a recursive call to see if there are multiple jumps that can be performed
                        branchMask = BitUtility.CombineBits(
                            branchMask,
                            TestJumps(
                                teamIndex,
                                targetBitMask,  // The new position mask for this piece after it jumps
                                BitUtility.CombineBits(BitUtility.IntersectBits(myPieceMask, ~positionMask), targetBitMask), // Our team's pieces mask with the position of this piece changed (myPieceMask & ~positionMask) | targetBitMask
                                BitUtility.IntersectBits(otherPieceMask, ~capturePieceBitMask), // Other team's pieces mask with the jumped piece gone (otherPieceMask & ~capturePieceBitMask)
                                isKing,
                                branchCaptureMask
                            )
                        );

                        // Set the potentialMoveBoardStates for the index of the jumpCell to encode the enemy piece that will be captured if that move is selected
                        _potentialCaptures[BitUtility.GetFirstBitPosition(targetBitMask)] |= branchCaptureMask;
                    }
                }
            };

            // If player 1 or the piece is a king
            if((teamIndex == PLAYER_1) || isKing)
            {
                _TestBranch(BoardBitStates.PLAYER1_LJUMP_MASK, 7, 7, 3, 4);
                _TestBranch(BoardBitStates.PLAYER1_RJUMP_MASK, 9, 9, 4, 5);
            }
            // If player 2 or the piece is a king
            if ((teamIndex == PLAYER_2) || isKing)
            {
                _TestBranch(BoardBitStates.PLAYER2_LJUMP_MASK, -9, -9, -5, -4);
                _TestBranch(BoardBitStates.PLAYER2_RJUMP_MASK, -7, -7, -4, -3);
            }

            return branchMask;
        }

        /// <summary>
        /// Update the external UI with the onBoardChange callback
        /// </summary>
        public void UpdateUI()
        {
            // Call the callback to update any UI listeners
            if (onBoardChange != null)
            {
                onBoardChange(_pieces[0], _pieces[1]);
            }
        }

        /// <summary>
        /// Render the game with the provided graphics interface
        /// </summary>
        /// <param name="graphics"></param>
        public void Render(System.Drawing.Graphics graphics)
        {
            // Clear background
            graphics.Clear(Color.Black);

            // Render all the graphics
            RenderBoard(graphics);
            RenderPieces(graphics);
            RenderUI(graphics);

            if (_isGameOver)
            {
                Brush darkBrush = new SolidBrush(Color.FromArgb(100,0,0,0));
                graphics.FillRectangle(darkBrush, new Rectangle() {X=0,Y=0, Width=CELL_SIZE*8, Height=CELL_SIZE*8});
            }

        }
        
        /// <summary>
        /// Render the board representation on screen
        /// </summary>
        /// <param name="graphics"></param>
        private void RenderBoard(System.Drawing.Graphics graphics)
        {
            Brush darkBrush = new SolidBrush(Color.Brown);
            Brush lightBrush = new SolidBrush(Color.Tan);
            // Loop through the cells and render them all
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

        /// <summary>
        /// Render a single piece
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="color"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="isSelected"></param>
        /// <param name="isKing"></param>
        private void RenderPiece(System.Drawing.Graphics graphics, ColorMatrix color, int cx, int cy, bool isSelected, bool isKing)
        {
            int size = (int)(CELL_SIZE * 0.75f);
            Rectangle rect = new Rectangle() { X = CELL_SIZE * cx + size / 5, Y = (CELL_SIZE * 7) - (CELL_SIZE * cy) + size / 5, Height = size, Width = size };

            if(isKing)
            {
                RenderingUtility.RenderImage(graphics, _kingIcon, rect, color);
            }
            else
            {
                RenderingUtility.RenderImage(graphics, _checkerIcon, rect, color);
            }
            
            // Draw the circle around the piece if it is currently selected
            if(isSelected)
            {
                Pen selectedPen = new Pen(Color.GreenYellow, 5);
                graphics.DrawEllipse(selectedPen, rect);
            }

        }

        /// <summary>
        /// Render all the pieces on the board
        /// </summary>
        /// <param name="graphics"></param>
        private void RenderPieces(System.Drawing.Graphics graphics)
        {
            for(int i = 0; i < sizeof(uint)*8; i++)
            {
                int boardIndex = i * 2;
                int y = boardIndex / BOARD_COLUMNS;
                int x = boardIndex - (y * BOARD_COLUMNS) + (y % 2);     // Get the x position based on the cell id

                // Check if this piece is selected and check to see if it is a king
                bool isSelected = BitUtility.CheckBit(_selectedBitMask, i) == 1;
                bool isKing = BitUtility.CheckBit(_kingStatusMask, i) == 1;

                // Draw player 1's pieces 
                if (BitUtility.CheckBit(_pieces[PLAYER_1], i) == 1)
                {
                    RenderPiece(graphics, _pieceColorMatrices[PLAYER_1], x, y, isSelected, isKing);
                }
                // Draw player 2's pieces
                if (BitUtility.CheckBit(_pieces[PLAYER_2], i) == 1)
                {
                    RenderPiece(graphics, _pieceColorMatrices[PLAYER_2], x, y, isSelected, isKing);
                }
            }
        }

        /// <summary>
        /// Render the UI elements
        /// </summary>
        /// <param name="graphics"></param>
        private void RenderUI(System.Drawing.Graphics graphics)
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

        /// <summary>
        /// Handle a mouse down even
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void OnMouseDown(int x, int y)
        {
            // If the game is not active then dont do anything 
            if(!isGameActive())
            {
                return;
            }

            if(IsCellSelected())
            {
                // If we clicked on the selected sell, deselect it
                if (_hoverBitMask == _selectedBitMask)
                {
                    SetSelectedCell(-1, -1);
                    _potentialMoveBitMask = 0;
                    return;
                }

                if(BitUtility.BitMatch(_hoverBitMask, _potentialMoveBitMask))
                {
                    MovePiece(_currentTurn % 2, BitUtility.GetFirstBitPosition(_selectedBitMask), BitUtility.GetFirstBitPosition(_hoverBitMask));
                    return;
                }
            }

            if (!IsHoverCellValid())
            {
                return;
            }

            // If there is a piece on the cell we are hovering on and it is the current team, select it
            if (BitUtility.BitMatch(_hoverBitMask, _pieces[_currentTurn % 2]))
            {
                SetSelectedCell();
                CalculatePotentialMoves(_selectedBitMask);
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
        /// Sets the selected cell position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetSelectedCell(int x, int y)
        {
            _selectedCellIndex[0] = x;
            _selectedCellIndex[1] = y;
            _selectedBitMask = CreateMaskFromBoardIndex(_selectedCellIndex[0], _selectedCellIndex[1]);
        }

        /// <summary>
        /// Set the cell that the cursor is on
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetHoverCell(int x, int y)
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

        /// <summary>
        /// Handle a new x,y mouse position from the window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetMousePosition(int x, int y)
        {
            // If the game is not active then dont do anything 
            if (!isGameActive())
            {
                return;
            }

            SetHoverCell(x, y);

        }

        /// <summary>
        /// Returns true if the mouse is hovering over the board
        /// </summary>
        /// <returns></returns>
        private bool IsHoverCellValid()
        {
            return (_hoverCellIndex[0] > -1) && (_hoverCellIndex[1] > -1);
        }
    }
}
