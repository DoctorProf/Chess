using System.Collections.ObjectModel;
using Chess.ViewModels.Base;
using Chess.Models;
using Chess.Constants;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;

namespace Chess.ViewModels
{
    internal class Field_ViewModel : ViewModel
    {
        #region Constructor
        public Field_ViewModel()
        {
            StartGame();
        }
        #endregion

        #region Start Game
        public void StartGame()
        {
            GenerateField();
            InitializePieces();
        }
        #endregion

        #region Properties

        int kingMoveBlack = 0;
        int kingMoveWhite = 0;

        #region Selected Field
        private Field selectedField;
        public Field SelectedField { get => selectedField; set => Set(ref selectedField, value); }
        #endregion

        #region Move
        private Piece.Color move = Piece.Color.White;
        public Piece.Color Move { get => move; set => Set(ref move, value); }
        #endregion

        #region Field
        ObservableCollection<ObservableCollection<Field>> f = new();
        public ObservableCollection<ObservableCollection<Field>> F
        {
            get => f;
            set => Set(ref f, value);
        }
        #endregion

        #endregion

        #region GenerateField
        public void GenerateField()
        {
            f.Clear();
            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<Field> fRow = new();
                for (int j = 0; j < 8; j++)
                {
                    string bgc = "";
                    if ((i + j) % 2 != 0)
                    {
                        bgc = "#769655";
                    }
                    else
                    {
                        bgc = "#EEEED2";
                    }
                    fRow.Add(new Field() { I = i, J = j, BackgroundColor = bgc });
                }
                f.Add(fRow);
            }
        }
        #endregion

        #region Clear and Set (Points And Circle And Defended)
        public void ClearPoints()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].TexturePath == TexturesPaths.Point)
                    {
                        F[i][j].PieceType = Piece.Type.Empty;
                        F[i][j].PieceColor = Piece.Color.Empty;
                    }
                }
            }
        }

        public void SetLogic()
        {
            ClearDefended();
            SetDefended();
            Checkmate();
            ClearPoints();
            ClearCircle();
            ClearBlocked();
            Move = Move == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;
        }

        public void ClearBlocked()
        {

        }


        public void ClearCircle()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    F[i][j].CircleTexture = TexturesPaths.Empty;
                    F[i][j].Blocked = false;
                }
            }
        }
        public void ClearDefended()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].DefendedBlack || F[i][j].DefendedWhite)
                    {
                        F[i][j].DefendedBlack = false;
                        F[i][j].DefendedWhite = false;
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
                F[i][j].CircleTexture = TexturesPaths.Circle;
            }
        }
        public void SetDefended()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].PieceType == Piece.Type.Pawn)
                    {
                        if (F[i][j].PieceColor == Piece.Color.White)
                        {
                            if (CheckOnBoard(i - 1, j - 1))
                            {
                                F[i - 1][j - 1].DefendedWhite = true;

                            }
                            if (CheckOnBoard(i - 1, j + 1))
                            {
                                F[i - 1][j + 1].DefendedWhite = true;
                            }
                        }
                        else if (F[i][j].PieceColor == Piece.Color.Black)
                        {
                            if (CheckOnBoard(i + 1, j - 1))
                            {
                                F[i + 1][j - 1].DefendedBlack = true;

                            }
                            if (CheckOnBoard(i + 1, j + 1))
                            {
                                F[i + 1][j + 1].DefendedBlack = true;
                            }
                        }
                    }
                    if (F[i][j].PieceType == Piece.Type.Knight)
                    {
                        if (F[i][j].PieceColor == Piece.Color.White)
                        {
                            if (CheckOnBoard(i - 2, j - 1))
                            {
                                F[i - 2][j - 1].DefendedWhite = true;

                            }
                            if (CheckOnBoard(i - 2, j + 1))
                            {
                                F[i - 2][j + 1].DefendedWhite = true;
                            }
                            if (CheckOnBoard(i - 1, j + 2))
                            {
                                F[i - 1][j + 2].DefendedWhite = true;

                            }
                            if (CheckOnBoard(i + 1, j + 2))
                            {
                                F[i + 1][j + 2].DefendedWhite = true;
                            }
                            if (CheckOnBoard(i + 2, j + 1))
                            {
                                F[i + 2][j + 1].DefendedWhite = true;

                            }
                            if (CheckOnBoard(i + 2, j - 1))
                            {
                                F[i + 2][j - 1].DefendedWhite = true;
                            }
                            if (CheckOnBoard(i + 1, j - 2))
                            {
                                F[i + 1][j - 2].DefendedWhite = true;

                            }
                            if (CheckOnBoard(i - 1, j - 2))
                            {
                                F[i - 1][j - 2].DefendedWhite = true;
                            }
                        }
                        else if (F[i][j].PieceColor == Piece.Color.Black)
                        {
                            if (CheckOnBoard(i - 2, j - 1))
                            {
                                F[i - 2][j - 1].DefendedBlack = true;

                            }
                            if (CheckOnBoard(i - 2, j + 1))
                            {
                                F[i - 2][j + 1].DefendedBlack = true;
                            }
                            if (CheckOnBoard(i - 1, j + 2))
                            {
                                F[i - 1][j + 2].DefendedBlack = true;

                            }
                            if (CheckOnBoard(i + 1, j + 2))
                            {
                                F[i + 1][j + 2].DefendedBlack = true;
                            }
                            if (CheckOnBoard(i + 2, j + 1))
                            {
                                F[i + 2][j + 1].DefendedBlack = true;

                            }
                            if (CheckOnBoard(i + 2, j - 1))
                            {
                                F[i + 2][j - 1].DefendedBlack = true;
                            }
                            if (CheckOnBoard(i + 1, j - 2))
                            {
                                F[i + 1][j - 2].DefendedBlack = true;

                            }
                            if (CheckOnBoard(i - 1, j - 2))
                            {
                                F[i - 1][j - 2].DefendedBlack = true;
                            }
                        }
                    }
                    if (F[i][j].PieceType == Piece.Type.Bishop)
                    {
                        DefendedBishop(i, j, F[i][j].PieceColor);
                    }
                    if (F[i][j].PieceType == Piece.Type.Rook)
                    {
                        DefendedRook(i, j, F[i][j].PieceColor);
                    }
                    if (F[i][j].PieceType == Piece.Type.Queen)
                    {
                        if (F[i][j].PieceColor == Piece.Color.White)
                        {
                            DefendedBishop(i, j, F[i][j].PieceColor);
                            DefendedRook(i, j, Piece.Color.White);
                        }
                        else if (F[i][j].PieceColor == Piece.Color.Black)
                        {
                            DefendedBishop(i, j, F[i][j].PieceColor);
                            DefendedRook(i, j, Piece.Color.Black);
                        }
                    }
                    if (F[i][j].PieceType == Piece.Type.King)
                    {
                        if (F[i][j].PieceColor == Piece.Color.White)
                        {
                            for (int addi = -1; addi < 1; addi++)
                            {
                                for (int addj = -1; addj < 1; addj++)
                                {
                                    if (addi == 1 && addj == 1) continue;
                                    if (CheckOnBoard(i + addi, j + addj)) F[i + addi][j + addj].DefendedWhite = true;
                                }
                            }
                        }
                        else if (F[i][j].PieceColor == Piece.Color.Black)
                        {
                            for (int addi = -1; addi < 1; addi++)
                            {
                                for (int addj = -1; addj < 1; addj++)
                                {
                                    if (addi == 1 && addj == 1) continue;
                                    if (CheckOnBoard(i + addi, j + addj)) F[i + addi][j + addj].DefendedBlack = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        #region DefendedBishop
        public void DefendedBishop(int i, int j, Piece.Color c)
        {
            int k;
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i - k, j - k))
                {
                    if (c == Piece.Color.White) F[i - k][j - k].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i - k][j - k].DefendedBlack = true;

                    if (F[i - k][j - k].PieceType != Piece.Type.Empty) break;
                }
            }
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i - k, j + k))
                {
                    if (c == Piece.Color.White) F[i - k][j + k].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i - k][j + k].DefendedBlack = true;

                    if (F[i - k][j + k].PieceType != Piece.Type.Empty) break;
                }
            }
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i + k, j + k))
                {
                    if (c == Piece.Color.White) F[i + k][j + k].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i + k][j + k].DefendedBlack = true;

                    if (F[i + k][j + k].PieceType != Piece.Type.Empty) break;
                }
            }
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i + k, j - k))
                {
                    if (c == Piece.Color.White) F[i + k][j - k].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i + k][j - k].DefendedBlack = true;

                    if (F[i + k][j - k].PieceType != Piece.Type.Empty) break;
                }
            }
        }
        #endregion

        #region DefendedRook
        public void DefendedRook(int i, int j, Piece.Color c)
        {
            int k;
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i - k, j))
                {
                    if (c == Piece.Color.White) F[i - k][j].DefendedWhite = true;
                    else if(c == Piece.Color.Black) F[i - k][j].DefendedBlack = true;

                    if (F[i - k][j].PieceType != Piece.Type.Empty) break;
                }
            }
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i + k, j))
                {
                    if (c == Piece.Color.White) F[i + k][j].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i + k][j].DefendedBlack = true;

                    if (F[i + k][j].PieceType != Piece.Type.Empty) break;
                }
            }
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i, j + k))
                {
                    if (c == Piece.Color.White) F[i][j + k].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i][j + k].DefendedBlack = true;

                    if (F[i][j + k].PieceType != Piece.Type.Empty) break;
                }
            }
            for (k = 1; k < 8; k++)
            {
                if (CheckOnBoard(i, j - k))
                {
                    if (c == Piece.Color.White) F[i][j - k].DefendedWhite = true;
                    else if (c == Piece.Color.Black) F[i][j - k].DefendedBlack = true;

                    if (F[i][j - k].PieceType != Piece.Type.Empty) break;
                }
            }
        }
        #endregion

        #endregion

        #region Pawn On Queen
        public void PawnOnQueen(Field field)
        {
            if ((field.I == 0 || field.I == 7) & SelectedField.PieceType == Piece.Type.Pawn)
            {
                field.PieceType = Piece.Type.Queen;
            }
        }
        #endregion

        #region KickAndWalk Knight
        public void KickAndWalkKnight(int checkposi, int checkposj)
        {
            if (CheckOnBoard(checkposi, checkposj))
            {
                if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                {
                    SetCircle(checkposi, checkposj);
                }
                else
                {
                    SetPoint(checkposi, checkposj);
                }
            }
        }
        #endregion

        #region Kick And Walk King
        public void KickAndWalkKing(int checkposi, int checkposj)
        {
            if (CheckOnBoard(checkposi, checkposj))
            {
                if (SelectedField.PieceColor == Piece.Color.White && !F[checkposi][checkposj].DefendedBlack)
                {
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty && F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                    }
                    else
                    {
                        SetPoint(checkposi, checkposj);
                    }
                }
                if (SelectedField.PieceColor == Piece.Color.Black && !F[checkposi][checkposj].DefendedWhite)
                {
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty && F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                    }
                    else
                    {
                        SetPoint(checkposi, checkposj);
                    }
                }
            }
        }

        #endregion

        #region Check Castling
        public void CheckCastling()
        {
                if (F[7][4].PieceType == Piece.Type.King && F[7][4].PieceColor == Piece.Color.White && kingMoveWhite == 0 && Move == Piece.Color.White)
                {
                    if (F[7][5].PieceType == Piece.Type.Empty && F[7][6].PieceType == Piece.Type.Empty && F[7][7].PieceType == Piece.Type.Rook
                        && !F[7][4].DefendedBlack && !F[7][5].DefendedBlack && !F[7][6].DefendedBlack)
                    {
                        SetPoint(7, 6);
                    }
                    if (F[7][3].PieceType == Piece.Type.Empty && F[7][2].PieceType == Piece.Type.Empty && F[7][1].PieceType == Piece.Type.Empty && F[7][0].PieceType == Piece.Type.Rook
                        && !F[7][4].DefendedBlack && !F[7][3].DefendedBlack && !F[7][2].DefendedBlack && !F[7][1].DefendedBlack)
                    {
                        SetPoint(7, 2);
                    }
                }
                if (F[0][4].PieceType == Piece.Type.King && F[0][4].PieceColor == Piece.Color.Black && kingMoveBlack == 0 && Move == Piece.Color.Black)
                {
                    if (F[0][5].PieceType == Piece.Type.Empty && F[0][6].PieceType == Piece.Type.Empty && F[0][7].PieceType == Piece.Type.Rook
                        && !F[0][4].DefendedWhite && !F[0][5].DefendedWhite && !F[0][6].DefendedWhite)
                    {
                        SetPoint(0, 6);
                    }
                    if (F[0][3].PieceType == Piece.Type.Empty && F[0][2].PieceType == Piece.Type.Empty && F[0][1].PieceType == Piece.Type.Empty && F[0][0].PieceType == Piece.Type.Rook
                        && !F[0][4].DefendedWhite && !F[0][3].DefendedWhite && !F[0][2].DefendedWhite && !F[0][1].DefendedWhite)
                    {
                        SetPoint(0, 2);
                    }
                }
        }
        #endregion

        #region All Knight
        public void AllKnight(int a)
        {
            int checkposi = SelectedField.I + a * -2;
            int checkposi1 = SelectedField.I + a * -1;
            int checkposi2 = SelectedField.I + a * 1;
            int checkposi3 = SelectedField.I + a * 2;

            int checkposj = SelectedField.J + a * -1;
            int checkposj1 = SelectedField.J + a * 1;
            int checkposj2 = SelectedField.J + a * 2;
            int checkposj3 = SelectedField.J + a * -2;

            KickAndWalkKnight(checkposi, checkposj);
            KickAndWalkKnight(checkposi, checkposj1);
            KickAndWalkKnight(checkposi1, checkposj2);
            KickAndWalkKnight(checkposi2, checkposj2);
            KickAndWalkKnight(checkposi3, checkposj1);
            KickAndWalkKnight(checkposi3, checkposj);
            KickAndWalkKnight(checkposi2, checkposj3);
            KickAndWalkKnight(checkposi1, checkposj3);
        }
        #endregion

        #region All King
        public void AllKing()
        {
            int checkposi = SelectedField.I;
            int checkposi1 = SelectedField.I + 1;
            int checkposi2 = SelectedField.I - 1;

            int checkposj = SelectedField.J;
            int checkposj1 = SelectedField.J + 1;
            int checkposj2 = SelectedField.J - 1;

            KickAndWalkKing(checkposi2, checkposj);
            KickAndWalkKing(checkposi2, checkposj1);
            KickAndWalkKing(checkposi, checkposj1);
            KickAndWalkKing(checkposi1, checkposj1);
            KickAndWalkKing(checkposi1, checkposj);
            KickAndWalkKing(checkposi1, checkposj2);
            KickAndWalkKing(checkposi, checkposj2);
            KickAndWalkKing(checkposi2, checkposj2);
            CheckCastling();
        }
        #endregion

        #region CheckOnBoard
        public static bool CheckOnBoard(int i, int j)
        {
            return i >= 0 && i < 8 && j >= 0 && j < 8;
        }
        #endregion

        #region SetPoint Bishop and Rook
        public void SetPointBishop(int a)
        {
            for (int i = 1; i <= 7; i++)
            {
                // Вверх влево
                int checkposi = SelectedField.I - 1 * a * i;
                int checkposj = SelectedField.J - 1 * a * i;

                if (CheckOnBoard(checkposi, checkposj))
                {
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
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
                    if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                    {
                        SetCircle(checkposi, checkposj);
                        break;
                    }
                    else
                    {
                        if (F[checkposi][checkposj].PieceType == Piece.Type.King)
                        {
                            break;
                        }
                        SetPoint(checkposi, checkposj);
                    }
                }
                else break;

            }
        }
        #endregion

        #region KickPawn
        public void KickPawn(int a)
        {
            int checkposi = SelectedField.I - 1 * a;
            int checkposj = SelectedField.J - 1 * a;
            int checkposj2 = SelectedField.J + 1 * a;
            if (CheckOnBoard(checkposi, checkposj))
            {
                if (F[checkposi][checkposj].PieceType != Piece.Type.Empty & F[checkposi][checkposj].PieceType != Piece.Type.King)
                {
                    SetCircle(checkposi, checkposj);
                }
            }
            if (CheckOnBoard(checkposi, checkposj2))
            {
                if (F[checkposi][checkposj2].PieceType != Piece.Type.Empty & F[checkposi][checkposj2].PieceType != Piece.Type.King)
                {
                    SetCircle(checkposi, checkposj2);
                }
            }
        }
        #endregion

        #region Reverse Figures
        public void ReverseFigures(Field field)
        {
            Field tempField = field.Clone();
            field.PieceColor = SelectedField.PieceColor;
            field.PieceType = SelectedField.PieceType;
            PawnOnQueen(field);
            SelectedField.PieceColor = tempField.PieceColor;
            SelectedField.PieceType = tempField.PieceType;
            SelectedField.Selected = false;
            SelectedField = null;
            SetLogic();
        }
        #endregion

        #region SetAllPoints
        public void SetAllPoints()
        {
            switch (SelectedField.PieceType)
            {
                case Piece.Type.Pawn:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        if (F[SelectedField.I - 1][SelectedField.J].PieceType == Piece.Type.Empty)
                        {
                            SetPoint(SelectedField.I - 1, SelectedField.J);
                            if (SelectedField.I == 6) SetPoint(SelectedField.I - 2, SelectedField.J);

                        }
                        KickPawn(1);

                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        if (F[SelectedField.I + 1][SelectedField.J].PieceType == Piece.Type.Empty)
                        {
                            SetPoint(SelectedField.I + 1, SelectedField.J);
                            if (SelectedField.I == 1) SetPoint(SelectedField.I + 2, SelectedField.J);

                        }
                        KickPawn(-1);
                    }
                    break;
                case Piece.Type.Knight:

                    if (SelectedField.PieceColor == Piece.Color.White)
                    {
                        AllKnight(1);
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        AllKnight(-1);
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
                        AllKing();
                    }
                    else if (SelectedField.PieceColor == Piece.Color.Black)
                    {
                        AllKing();
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
        #endregion

        #region Castling
        public void Castling(Field field, int i, int j, int j1)
        {
            field.PieceColor = SelectedField.PieceColor;
            field.PieceType = SelectedField.PieceType;
            SelectedField.PieceType = Piece.Type.Empty;
            SelectedField.PieceColor = Piece.Color.Empty;
            SelectedField.Selected = false;
            F[i][j].PieceType = Piece.Type.Rook;
            F[i][j].PieceColor = Move;
            F[i][j1].PieceType = Piece.Type.Empty;
            F[i][j1].PieceColor = Piece.Color.Empty;
            ClearPoints();
            ClearCircle();
            Move = Move == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;
        }
        #endregion

        #region GoToMenu

        public void GoToMenu()
        {
            Frame frame = (Frame)Application.Current.MainWindow.Content;
            NavigationService.GetNavigationService((Page)frame.Content).Navigate(new Uri("View/Pages/menu.xaml", UriKind.RelativeOrAbsolute));
        }

        #endregion

        #region CheckMate
        public void Checkmate()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (f[i][j].PieceType == Piece.Type.King)
                    {
                        if (F[i][j].PieceColor == Piece.Color.White)
                        {
                            if (F[i][j].DefendedBlack)
                            {
                                if ((!CheckOnBoard(i - 1, j - 1) || F[i - 1][j - 1].DefendedBlack || (F[i - 1][j - 1].PieceType != Piece.Type.Empty & F[i - 1][j - 1].PieceColor == Piece.Color.White)) && (!CheckOnBoard(i, j + 1) || F[i][j + 1].DefendedBlack || (F[i][j + 1].PieceType != Piece.Type.Empty & F[i][j + 1].PieceColor == Piece.Color.White)) &&
                                    (!CheckOnBoard(i - 1, j) || F[i - 1][j].DefendedBlack || (F[i - 1][j].PieceType != Piece.Type.Empty & F[i - 1][j].PieceColor == Piece.Color.White)) && (!CheckOnBoard(i + 1, j - 1) || F[i + 1][j - 1].DefendedBlack || (F[i + 1][j - 1].PieceType != Piece.Type.Empty & F[i + 1][j - 1].PieceColor == Piece.Color.White)) &&
                                    (!CheckOnBoard(i - 1, j + 1) || F[i - 1][j + 1].DefendedBlack || (F[i - 1][j + 1].PieceType != Piece.Type.Empty & F[i - 1][j + 1].PieceColor == Piece.Color.White)) && (!CheckOnBoard(i + 1, j) || F[i + 1][j].DefendedBlack || (F[i + 1][j].PieceType != Piece.Type.Empty & F[i + 1][j].PieceColor == Piece.Color.White)) &&
                                    (!CheckOnBoard(i, j - 1) || F[i][j - 1].DefendedBlack || (F[i][j - 1].PieceType != Piece.Type.Empty & F[i][j - 1].PieceColor == Piece.Color.White)) && (!CheckOnBoard(i + 1, j + 1) || F[i + 1][j + 1].DefendedBlack || (F[i + 1][j + 1].PieceType != Piece.Type.Empty & F[i + 1][j + 1].PieceColor == Piece.Color.White)))
                                {
                                    MessageBox.Show("Выйграли черные");
                                    GoToMenu();
                                }
                            }
                        }
                        else
                        {
                            if (F[i][j].DefendedWhite)
                            {
                                if ((!CheckOnBoard(i - 1, j - 1) || F[i - 1][j - 1].DefendedWhite || (F[i - 1][j - 1].PieceType != Piece.Type.Empty & F[i - 1][j - 1].PieceColor == Piece.Color.Black)) && (!CheckOnBoard(i, j + 1) || F[i][j + 1].DefendedWhite || (F[i][j + 1].PieceType != Piece.Type.Empty & F[i][j + 1].PieceColor == Piece.Color.Black)) &&
                                    (!CheckOnBoard(i - 1, j) || F[i - 1][j].DefendedWhite || (F[i - 1][j].PieceType != Piece.Type.Empty & F[i - 1][j].PieceColor == Piece.Color.Black)) && (!CheckOnBoard(i + 1, j - 1) || F[i + 1][j - 1].DefendedWhite || (F[i + 1][j - 1].PieceType != Piece.Type.Empty & F[i + 1][j - 1].PieceColor == Piece.Color.Black)) &&
                                    (!CheckOnBoard(i - 1, j + 1) || F[i - 1][j + 1].DefendedWhite || (F[i - 1][j + 1].PieceType != Piece.Type.Empty & F[i - 1][j + 1].PieceColor == Piece.Color.Black)) && (!CheckOnBoard(i + 1, j) || F[i + 1][j].DefendedWhite || (F[i + 1][j].PieceType != Piece.Type.Empty & F[i + 1][j].PieceColor == Piece.Color.Black)) &&
                                    (!CheckOnBoard(i, j - 1) || F[i][j - 1].DefendedWhite || (F[i][j - 1].PieceType != Piece.Type.Empty & F[i][j - 1].PieceColor == Piece.Color.Black)) && (!CheckOnBoard(i + 1, j + 1) || F[i + 1][j + 1].DefendedWhite || (F[i + 1][j + 1].PieceType != Piece.Type.Empty & F[i + 1][j + 1].PieceColor == Piece.Color.Black)))
                                {
                                    MessageBox.Show("Выйграли белые");
                                    GoToMenu();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Move Logic
        public void MoveLogic(Field field)
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
                        else if (field != SelectedField && field.PieceColor == Move)
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
                        if (SelectedField.PieceType == Piece.Type.King)
                        {
                            if ((field.I == 7 && field.J == 6) && kingMoveWhite == 0)
                            {
                                Castling(field, 7, 5, 7);
                                kingMoveWhite += 1;
                            }
                            else if ((field.I == 7 && field.J == 2) && kingMoveWhite == 0)
                            {
                                Castling(field, 7, 3, 0);
                                kingMoveWhite += 1;
                            }
                            else if ((field.I == 0 && field.J == 6) && kingMoveBlack == 0)
                            {
                                Castling(field, 0, 5, 7);
                                kingMoveBlack += 1;
                            }
                            else if ((field.I == 0 && field.J == 2) && kingMoveBlack == 0)
                            {
                                Castling(field, 0, 3, 0);
                                kingMoveBlack += 1;
                            }
                            else
                            {
                                ReverseFigures(field);
                            }
                        }
                        else
                        {
                            PawnOnQueen(field);
                            if (field.PieceType == Piece.Type.King)
                            {
                                if (field.PieceColor == Piece.Color.White)
                                {
                                    kingMoveWhite += 1;
                                }
                                else
                                {
                                    kingMoveBlack += 1;
                                }
                            }
                            ReverseFigures(field);
                        }
                    }
                    else if (field.CircleTexture == TexturesPaths.Circle)
                    {
                        field.PieceColor = SelectedField.PieceColor;
                        field.PieceType = SelectedField.PieceType;
                        field.TexturePath = SelectedField.TexturePath;
                        PawnOnQueen(field);
                        if (field.PieceType == Piece.Type.King)
                        {
                            if (field.PieceColor == Piece.Color.White)
                            {
                                kingMoveWhite += 1;
                            }
                            else
                            {
                                kingMoveBlack += 1;
                            }
                        }
                        SelectedField.PieceColor = Piece.Color.Empty;
                        SelectedField.PieceType = Piece.Type.Empty;
                        SelectedField.TexturePath = TexturesPaths.Empty;
                        SelectedField.Selected = false;
                        SelectedField = null;
                        SetLogic();
                    }
                }
            }
        }
        #endregion

        #region Click field
        public void ClickField(Field field)
        {
             MoveLogic(field);
        }
        #endregion

        #region Starting position
        public void InitializePieces()
        {
            #region Black pieces
            // Черные фигуры
            f[0][0].PieceType = Piece.Type.Rook;
            f[0][0].PieceColor = Piece.Color.Black;

            
            f[0][1].PieceType = Piece.Type.Knight;
            f[0][1].PieceColor = Piece.Color.Black;

            f[0][2].PieceType = Piece.Type.Bishop;
            f[0][2].PieceColor = Piece.Color.Black;

            f[0][3].PieceType = Piece.Type.Queen;
            f[0][3].PieceColor = Piece.Color.Black;

            f[0][4].PieceType = Piece.Type.King;
            f[0][4].PieceColor = Piece.Color.Black;

            f[0][5].PieceType = Piece.Type.Bishop;
            f[0][5].PieceColor = Piece.Color.Black;

            f[0][6].PieceType = Piece.Type.Knight;
            f[0][6].PieceColor = Piece.Color.Black;

            f[0][7].PieceType = Piece.Type.Rook;
            f[0][7].PieceColor = Piece.Color.Black;

            for (int i = 0; i < 8; i++)
            {
                /*
                f[1][i].PieceType = Piece.Type.Pawn;
                f[1][i].PieceColor = Piece.Color.Black;
                */
            }
            #endregion

            #region White pieces
            // Белые
            f[7][0].PieceType = Piece.Type.Rook;
            f[7][0].PieceColor = Piece.Color.White;

            f[7][1].PieceType = Piece.Type.Knight;
            f[7][1].PieceColor = Piece.Color.White;

            f[7][2].PieceType = Piece.Type.Bishop;
            f[7][2].PieceColor = Piece.Color.White;

            f[7][3].PieceType = Piece.Type.Queen;
            f[7][3].PieceColor = Piece.Color.White;

            f[7][4].PieceType = Piece.Type.King;
            f[7][4].PieceColor = Piece.Color.White;

            f[7][5].PieceType = Piece.Type.Bishop;
            f[7][5].PieceColor = Piece.Color.White;

            f[7][6].PieceType = Piece.Type.Knight;
            f[7][6].PieceColor = Piece.Color.White;

            f[7][7].PieceType = Piece.Type.Rook;
            f[7][7].PieceColor = Piece.Color.White;

            for (int i = 0; i < 8; i++)
            {
                /*
                f[6][i].PieceType = Piece.Type.Pawn;
                f[6][i].PieceColor = Piece.Color.White;
                */
            }
            #endregion
        }
        #endregion
    }
}