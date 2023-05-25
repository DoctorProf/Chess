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
using System.Configuration;

namespace Chess.Models
{
    internal class Field : ViewModel
    {
        #region Background Color
        private string backgroundColor;
        public string BackgroundColor { get => backgroundColor; set => Set(ref backgroundColor, value); }
        #endregion

        #region SelectedBackgroundColor
        private string selectedBackgroundColor;
        public string SelectedBackgroundColor {
            get {
                UpdateBackground();
                return selectedBackgroundColor;
            }
            set => Set(ref selectedBackgroundColor, value);
        
        }
        public void UpdateBackground() {
            SelectedBackgroundColor = selected ? "LightGray" : BackgroundColor;

        }
        public void UpdateTexture()
        {
            if (PieceType != Piece.Type.Empty && PieceColor != Piece.Color.Empty)
            {
                TexturePath = TexturesPaths.ImageFolder + pieceType.ToString() + "_" + pieceColor.ToString()+".png";
            } else
            {
                TexturePath = TexturesPaths.Empty;
            }
        }
        #endregion

        #region PieceType
        private Piece.Type pieceType = Piece.Type.Empty;
        public Piece.Type PieceType
        {
            get => pieceType;
            set
            {
                Set(ref pieceType, value);
                UpdateTexture();
            }
        }
        #endregion

        #region PieceColor
        private Piece.Color pieceColor = Piece.Color.Empty;
        public Piece.Color PieceColor
        {
            get => pieceColor; 
            set
            {
                Set(ref pieceColor, value);
                UpdateTexture();
            }
        }
        #endregion

        #region TexturePath
        private string texturePath = TexturesPaths.Empty;
        public string TexturePath { get => texturePath; set {
        
                Set(ref texturePath, value);
        } }
        #endregion

        #region CircleTexture
        private string circleTexture = TexturesPaths.Empty;
        public string CircleTexture { get => circleTexture; set => Set(ref circleTexture, value); }
        #endregion

        #region Selected
        private bool selected;
        public bool Selected { get => selected; set {
                Set(ref selected, value);
                UpdateBackground();
            }
        }
        #endregion

        #region Defended
        private bool defended = false;
        public bool Defended { get => defended; set => Set(ref  defended, value); }
        #endregion

        #region King Move
        private int kingMove;
        public int KingMove { get => kingMove; set => Set(ref kingMove, value); }
        #endregion

        #region I
        private int i;
        public int I { get => i; set => Set(ref i, value); }
        #endregion

        #region J
        private int j;
        public int J { get => j; set => Set(ref j, value); }
        #endregion

        #region Clone
        public Field Clone()
        {
            return new Field() { TexturePath = texturePath, PieceType = pieceType,
                PieceColor = pieceColor, I = i, J = j, Selected = selected, BackgroundColor = backgroundColor, SelectedBackgroundColor = selectedBackgroundColor};
        }
        #endregion
    }
}
