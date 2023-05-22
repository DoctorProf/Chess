using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.View.Pages;
using Chess.ViewModels.Base;
using Chess.Models;
using System.Windows.Media;
using System.ComponentModel;
using Chess.Constants;
using System.Windows;
using Microsoft.Win32;

namespace Chess.ViewModels
{
    internal class Field_ViewModel : ViewModel
    {
        public Field_ViewModel()
        {
            Size = "8";
            InitializePieces();
        }
        #region Properties
        private Field selectedField;
        public Field SelectedField { get => selectedField; set => Set(ref selectedField, value); }

        private Piece.Color move = Piece.Color.White;
        public Piece.Color Move { get => move; set => Set(ref move, value); }


        ObservableCollection<ObservableCollection<Field>> f = new();
        public ObservableCollection<ObservableCollection<Field>> F
        {
            get => f;
            set => Set(ref f, value);
        }
        DataTable field;
        public DataTable Field
        {
            get => field;
            set => Set(ref field, value);
        }
        #endregion

        #region PropertyGenerateField
        int size = 0;
        public string Size
        {
            get => size.ToString();
            set
            {
                if (int.TryParse(value, out int newSize))
                {
                    Set(ref size, newSize);

                    DataTable newField = new();
                    for (int i = 0; i < newSize; i++)
                        newField.Columns.Add();

                    for (int i = 0; i < newSize; i++)
                    {
                        object[] row = new object[newSize];
                        for (int j = 0; j < size; j++)
                            row[j] = i * 10 + j;
                        newField.Rows.Add(row);
                    }
                    Field = newField;


                    f.Clear();
                    for (int i = 0; i < newSize; i++)
                    {
                        ObservableCollection<Field> fRow = new();
                        for (int j = 0; j < size; j++)
                            fRow.Add(new Field() { I = i, J = j, PieceType = Piece.Type.Empty });
                        f.Add(fRow);
                    }
                }
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Field cell = f[i][j];
                        if ((i + j) % 2 != 0)
                        {
                            cell.BackgroundColor = "#eeeed2";
                        }
                        else
                        {
                            cell.BackgroundColor = "#769655";
                        }
                    }
                }
            }
        }
        #endregion

        public static void ClearPoints(Field_ViewModel fvm)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (fvm.F[i][j].TexturePath == TexturesPaths.Point) fvm.F[i][j].TexturePath = TexturesPaths.Empty;
                }
            }
        }
        public void ClearSelected()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    F[i][j].Selected = false;
                }
            }
        }
        public void CheckSelected(Field field, Field SelectedField, int i, int j)
        {
            if (field == SelectedField && SelectedField != null)
            {
                // Текущее поле выделено (Снимаем выделение клик по этой клетке)
                field.Selected = false;
                SelectedField = null;
            }
            else
            {
                //  Снимаем выделение клик по любой другой клетке
                field.Selected = true;
                SelectedField.Selected = false;
                SelectedField = field;
            }
        }

        public void SetPoint(int i, int j)
        {
            if (F[i][j].PieceType == Piece.Type.Empty) F[i][j].TexturePath = TexturesPaths.Point;
        }

        public void MoveLogic(Field field, Piece.Color color)
        {

        }
        public void ClickField(Field field, Field_ViewModel fvm)
        {
            // Если ходят белые
            if (Move == Piece.Color.White)
            {
                MoveLogic(field, Move);

            }
            else
            {
                MoveLogic(field, Move);
            }
        }
        #region Starting position
        public void InitializePieces()
        {
            #region Black pieces
            // Белые фигуры
            f[0][0].PieceType = Piece.Type.Rook;
            f[0][0].PieceColor = Piece.Color.Black;
            f[0][0].TexturePath = TexturesPaths.Rook_Black;

            f[0][1].PieceType = Piece.Type.Knight;
            f[0][1].PieceColor = Piece.Color.Black;
            f[0][1].TexturePath = TexturesPaths.Knight_Black;

            f[0][2].PieceType = Piece.Type.Bishop;
            f[0][2].PieceColor = Piece.Color.Black;
            f[0][2].TexturePath = TexturesPaths.Bishop_Black;

            f[0][3].PieceType = Piece.Type.Queen;
            f[0][3].PieceColor = Piece.Color.Black;
            f[0][3].TexturePath = TexturesPaths.Queen_Black;

            f[0][4].PieceType = Piece.Type.King;
            f[0][4].PieceColor = Piece.Color.Black;
            f[0][4].TexturePath = TexturesPaths.King_Black;

            f[0][5].PieceType = Piece.Type.Bishop;
            f[0][5].PieceColor = Piece.Color.Black;
            f[0][5].TexturePath = TexturesPaths.Bishop_Black;

            f[0][6].PieceType = Piece.Type.Knight;
            f[0][6].PieceColor = Piece.Color.Black;
            f[0][6].TexturePath = TexturesPaths.Knight_Black;

            f[0][7].PieceType = Piece.Type.Rook;
            f[0][7].PieceColor = Piece.Color.Black;
            f[0][7].TexturePath = TexturesPaths.Rook_Black;

            for (int i = 0; i < 8; i++)
            {
                f[1][i].PieceType = Piece.Type.Pawn;
                f[1][i].PieceColor = Piece.Color.Black;
                f[1][i].TexturePath = TexturesPaths.Pawn_Black;
            }
            #endregion

            #region White pieces
            // Черные фигуры
            f[7][0].PieceType = Piece.Type.Rook;
            f[7][0].PieceColor = Piece.Color.White;
            f[7][0].TexturePath = TexturesPaths.Rook_White;

            f[7][1].PieceType = Piece.Type.Knight;
            f[7][1].PieceColor = Piece.Color.White;
            f[7][1].TexturePath = TexturesPaths.Knight_White;


            f[7][2].PieceType = Piece.Type.Bishop;
            f[7][2].PieceColor = Piece.Color.White;
            f[7][2].TexturePath = TexturesPaths.Bishop_White;


            f[7][3].PieceType = Piece.Type.Queen;
            f[7][3].PieceColor = Piece.Color.White;
            f[7][3].TexturePath = TexturesPaths.Queen_White;


            f[7][4].PieceType = Piece.Type.King;
            f[7][4].PieceColor = Piece.Color.White;
            f[7][4].TexturePath = TexturesPaths.King_White;


            f[7][5].PieceType = Piece.Type.Bishop;
            f[7][5].PieceColor = Piece.Color.White;
            f[7][5].TexturePath = TexturesPaths.Bishop_White;


            f[7][6].PieceType = Piece.Type.Knight;
            f[7][6].PieceColor = Piece.Color.White;
            f[7][6].TexturePath = TexturesPaths.Knight_White;


            f[7][7].PieceType = Piece.Type.Rook;
            f[7][7].PieceColor = Piece.Color.White;
            f[7][7].TexturePath = TexturesPaths.Rook_White;


            for (int i = 0; i < 8; i++)
            {
                f[6][i].PieceType = Piece.Type.Pawn;
                f[6][i].PieceColor = Piece.Color.White;
                f[6][i].TexturePath = TexturesPaths.Pawn_White;

            }
            #endregion
        }
        #endregion
    }
}
