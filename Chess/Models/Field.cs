using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Commands.Base;
using Chess.Commands;
using Chess.ViewModels.Base;
using Chess.Constants;
using Chess.ViewModels;
using System.Windows.Markup.Localizer;
using System.Windows;

namespace Chess.Models
{
    internal class Field : ViewModel
    {
        #region Background Color
        private string backgroundColor;
        public string BackgroundColor { get => backgroundColor; set => Set(ref backgroundColor, value); }
        #endregion

        #region PieceType
        private Piece.Type pieceType;
        public Piece.Type PieceType { get => pieceType; set => Set(ref pieceType, value); }
        #endregion

        #region PieceColor
        private Piece.Color pieceColor;
        public Piece.Color PieceColor { get => pieceColor; set => Set(ref pieceColor, value); }
        #endregion

        #region TexturePath
        private string texturePath = TexturesPaths.Empty;
        public string TexturePath { get => texturePath; set {
        
                Set(ref texturePath, value);
        } }
        #endregion

        #region Parent
        private Field_ViewModel parent;
        public Field_ViewModel Parent { get => parent; set => Set(ref parent, value); } 
        #endregion

        #region I
        private int i;
        public int I { get => i; set => Set(ref i, value); }
        #endregion

        #region J
        private int j;
        public int J { get => j; set => Set(ref j, value); }
        #endregion
    }
}
