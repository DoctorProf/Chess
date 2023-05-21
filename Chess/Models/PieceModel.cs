using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Constants;
using Chess.ViewModels.Base;

namespace Chess.Models
{
    class PieceModel : ViewModel
    {
        #region Type
        private Piece.Type type;
        public Piece.Type Type { get => type; set => Set(ref type, value); }
        #endregion
        #region Color
        private Piece.Color color;
        public Piece.Color Color { get => color; set => Set(ref color, value); }
        #endregion
        #region Texture
        private TexturesPaths texture;
        public TexturesPaths Texture { get => texture; set => Set(ref texture, value); }
        #endregion
        #region X
        private int x;
        public int X { get => x; set => Set(ref x, value); }
        #endregion
        #region Y
        private int y;
        public int Y { get => y; set => Set(ref y, value); }
        #endregion

    }
}
