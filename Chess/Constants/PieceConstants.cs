using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Constants
{
    class Piece
    {
        #region Enum PieceType
        public enum Type
        {
            Pawn,
            Rook,
            Knight,
            Bishop,
            Queen,
            King,
            Empty
        }
        #endregion

        #region Enum PieceColor
        public enum Color
        {
            White,
            Black,
            Empty
        }
        #endregion
    }
}
