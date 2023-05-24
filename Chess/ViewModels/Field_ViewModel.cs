﻿using System;
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
using System.Security;
using System.Security.Cryptography.X509Certificates;

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
                            fRow.Add(new Field() { I = i, J = j, PieceType = Piece.Type.Empty, PieceColor = Piece.Color.Empty, TexturePath = TexturesPaths.Empty });
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
        public void ClearPoints()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].TexturePath == TexturesPaths.Point)
                    {
                        F[i][j].TexturePath = TexturesPaths.Empty;
                        F[i][j].PieceType = Piece.Type.Empty;
                        F[i][j].PieceColor = Piece.Color.Empty;
                    }
                }
            }
        }
        public void ClearCircle()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].TexturePath == TexturesPaths.Circle)
                    {
                        F[i][j].TexturePath = TexturesPaths.Empty;
                        F[i][j].PieceType = Piece.Type.Empty;
                        F[i][j].PieceColor = Piece.Color.Empty;
                    }
                }
            }
        }
        public void SetPoint(int i, int j)
        {
            if (((i >= 0 & i <= 7) & (j >= 0 & j <= 7)) && F[i][j].PieceType == Piece.Type.Empty)
            {
                F[i][j].TexturePath = TexturesPaths.Point;
            }
        }
        public void SetCircle(int i, int j)
        {
            if (((i >= 0 & i <= 7) & (j >= 0 & j <= 7)) && F[i][j].PieceType != Piece.Type.Empty & Move != F[i][j].PieceColor)
            {
                F[i][j].TexturePath = TexturesPaths.Circle;
            }
        }
        public void SetPointKnight(int a)
        {
            SetPoint(SelectedField.I + a * -2, SelectedField.J + a * -1);
            SetPoint(SelectedField.I + a * -2, SelectedField.J + a * 1);
            SetPoint(SelectedField.I + a * -1, SelectedField.J + a * 2);
            SetPoint(SelectedField.I + a * 1, SelectedField.J + a * 2);
            SetPoint(SelectedField.I + a * 2, SelectedField.J + a * 1);
            SetPoint(SelectedField.I + a * 2, SelectedField.J + a * -1);
            SetPoint(SelectedField.I + a * 1, SelectedField.J + a * -2);
            SetPoint(SelectedField.I + a * -1, SelectedField.J + a * -2);
        }
        public void SetPointKing()
        {
            SetPoint(SelectedField.I - 1, SelectedField.J);
            SetPoint(SelectedField.I - 1, SelectedField.J + 1);
            SetPoint(SelectedField.I, SelectedField.J + 1);
            SetPoint(SelectedField.I + 1, SelectedField.J + 1);
            SetPoint(SelectedField.I + 1, SelectedField.J);
            SetPoint(SelectedField.I + 1, SelectedField.J - 1);
            SetPoint(SelectedField.I, SelectedField.J - 1);
            SetPoint(SelectedField.I - 1, SelectedField.J - 1);
        }
        public bool CheckOnBoard(int i, int j)
        {
            return i >= 0 && i < 8 && j >= 0 && j < 8;
        }
        public void SetPointBishop(int a)
        {
            for (int i = 1; i <= 7; i++)
            {
                // Вверх влево
                int checkposi = SelectedField.I - 1 * a * i;
                int checkposj = SelectedField.J - 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj)) {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;
            }
            for (int i = 1; i <= 7; i++)
            {
                // Вверх вправо
                int checkposi = SelectedField.I - 1 * a * i;
                int checkposj = SelectedField.J + 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    } 
                    else break;
                }
                else break;
            }
            for (int i = 1; i <= 7; i++)
            {
                // Вниз влево
                int checkposi = SelectedField.I + 1 * a * i;
                int checkposj = SelectedField.J - 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;
            }
            for (int i = 1; i <= 7; i++)
            {
                // Вниз вправо
                int checkposi = SelectedField.I + 1 * a * i;
                int checkposj = SelectedField.J + 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;

            }
        }
        public void SetPointRook(int a)
        {
            for (int i = 1; i <= 7; i++)
            {
                // Вверх
                int checkposi = SelectedField.I - 1 * a * i;
                int checkposj = SelectedField.J;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;
            }
            for (int i = 1; i <= 7; i++)
            {
                // Вниз
                int checkposi = SelectedField.I + 1 * a * i;
                int checkposj = SelectedField.J;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;
            }
            for (int i = 1; i <= 7; i++)
            {
                // Влево
                int checkposi = SelectedField.I;
                int checkposj = SelectedField.J - 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;
            }
            for (int i = 1; i <= 7; i++)
            {
                // Вправо
                int checkposi = SelectedField.I;
                int checkposj = SelectedField.J + 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType == Piece.Type.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                    else break;
                }
                else break;

            }
        }
        public void KickPawn(int a) 
        {
            int checkposi = SelectedField.I - 1 * a;
            int checkposj = SelectedField.J + 1 * a;
            int checkposj2 = SelectedField.J - 1 * a;
            if (CheckOnBoard(checkposi, checkposj))
            {
                if (F[checkposi][checkposj].PieceType != Piece.Type.Empty)
                {
                    SetCircle(checkposi, checkposj);
                }
            }
            if (CheckOnBoard(checkposi, checkposj2))
            {
                if (F[checkposi][checkposj2].PieceType != Piece.Type.Empty)
                {
                    SetCircle(checkposi, checkposj2);
                }
            }
        }
        public void SetAllPoints() 
        {
            switch (SelectedField.PieceType)
            {
                case Piece.Type.Pawn:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        SetPoint(SelectedField.I - 1, SelectedField.J);
                        if (SelectedField.I == 6) SetPoint(SelectedField.I - 2, SelectedField.J);
                        KickPawn(1);
                        
                        
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        SetPoint(SelectedField.I + 1, SelectedField.J);
                        if (SelectedField.I == 1) SetPoint(SelectedField.I + 2, SelectedField.J);
                        KickPawn(-1);
                    }
                    break;
                case Piece.Type.Knight:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        SetPointKnight(1);
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        SetPointKnight(-1);
                    }
                    break;
                case Piece.Type.Bishop:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        SetPointBishop(1);
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        SetPointBishop(-1);
                    }
                    break;
                case Piece.Type.Rook:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        SetPointRook(1);
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        SetPointRook(-1);
                    }
                    break;
                case Piece.Type.King:
                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        SetPointKing();
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        SetPointKing();
                    }
                    break;
                case Piece.Type.Queen:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        SetPointBishop(1);
                        SetPointRook(1);
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        SetPointBishop(-1);
                        SetPointRook(-1);
                    }
                    break;
            }
        }
        public void MoveLogic(Field field, Piece.Color color, Field_ViewModel fvm)
        {

            if (field.PieceType != Piece.Type.Empty & field.PieceColor == Move)
            {
                if (field.PieceColor == Move)
                {
                    if (SelectedField == null)
                    {
                        SelectedField = field;
                        SelectedField.Selected = true;
                        SetAllPoints();
                    }
                    else
                    {
                        if (field == SelectedField)
                        {
                            SelectedField.Selected = false;
                            SelectedField = null;
                            ClearPoints();
                            ClearCircle();
                        }
                        else if (field != SelectedField & field.PieceColor == Move)
                        {
                            SelectedField.Selected = false;
                            ClearPoints();
                            SelectedField = field;
                            SelectedField.Selected = true;
                            SetAllPoints();
                        }
                    }
                }

            }
            else
            {
                if (SelectedField != null)
                {
                    if (field.TexturePath == TexturesPaths.Point)
                    {
                        Field tempField = field.Clone();
                        field.PieceColor = SelectedField.PieceColor;
                        field.PieceType = SelectedField.PieceType;
                        field.TexturePath = SelectedField.TexturePath;
                        SelectedField.PieceColor = tempField.PieceColor;
                        SelectedField.PieceType = tempField.PieceType;
                        SelectedField.TexturePath = tempField.TexturePath;
                        /*
                        if ((field.I == 0 || field.I == 7) & SelectedField.PieceType == Piece.Type.Pawn)
                        {
                            field.PieceColor = Move;
                            field.PieceType = Piece.Type.Queen;
                            field.TexturePath = TexturesPaths.Queen_Black;
                        }
                        */
                        SelectedField.Selected = false;
                        SelectedField = null;
                        ClearPoints();
                        ClearCircle(); 
                        Move = color == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;
                    } 
                     else if (field.TexturePath == TexturesPaths.Circle)
                    {
                        field.PieceColor = SelectedField.PieceColor;
                        field.PieceType = SelectedField.PieceType;
                        field.TexturePath = SelectedField.TexturePath;
                        SelectedField.PieceColor = Piece.Color.Empty;
                        SelectedField.PieceType = Piece.Type.Empty;
                        SelectedField.TexturePath = TexturesPaths.Empty;
                        SelectedField.Selected = false;
                        SelectedField = null;
                        ClearPoints();
                        ClearCircle();
                        Move = color == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;
                    }
                } 
            }
        }
        public void ClickField(Field field, Field_ViewModel fvm)
        {
            // Если ходят белые
            if (Move == Piece.Color.White)
            {
                MoveLogic(field, Move, fvm);

            }
            else
            {
                MoveLogic(field, Move, fvm);
            }
        }
        #region Starting position
        public void InitializePieces()
        {
            #region Black pieces
            // Черные фигуры
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
            // Белые
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
