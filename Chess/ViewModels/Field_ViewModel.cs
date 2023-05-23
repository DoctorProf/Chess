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

		#region arrays
		public int[,] walkPawn = new[,]
		{
			{-1, 0},
			{-2, 0}
		};
		public int[,] walkKnight = new[,]
		{
			{-2, -1},
			{-2, 1},
			{-1, 2},
			{1, 2},
			{2, 1},
			{2, -1},
			{1, -2},
			{-1, -2}
		};
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
		public void SetPointKnight(int a)
		{
            SetPoint(SelectedField.I + a * walkKnight[0, 0], SelectedField.J + walkKnight[0, 1]);
            SetPoint(SelectedField.I + a * walkKnight[1, 0], SelectedField.J + walkKnight[1, 1]);
            SetPoint(SelectedField.I + a * walkKnight[2, 0], SelectedField.J + walkKnight[2, 1]);
            SetPoint(SelectedField.I + a * walkKnight[3, 0], SelectedField.J + walkKnight[3, 1]);
            SetPoint(SelectedField.I + a * walkKnight[4, 0], SelectedField.J + walkKnight[4, 1]);
            SetPoint(SelectedField.I + a * walkKnight[5, 0], SelectedField.J + walkKnight[5, 1]);
            SetPoint(SelectedField.I + a * walkKnight[6, 0], SelectedField.J + walkKnight[6, 1]);
            SetPoint(SelectedField.I + a * walkKnight[7, 0], SelectedField.J + walkKnight[7, 1]);
        }
		public void MoveLogic(Field field, Piece.Color color, Field_ViewModel fvm)
		{

			if (field.PieceType != Piece.Type.Empty)
			{
				if (field.PieceColor == Move)
				{
					if (SelectedField == null)
					{
						SelectedField = field;
						SelectedField.Selected = true;
						switch (SelectedField.PieceType)
						{
							case Piece.Type.Pawn:

								if (SelectedField.PieceColor == Piece.Color.White)
								{
									SetPoint(SelectedField.I + walkPawn[0, 0], SelectedField.J);
									if (SelectedField.I == 6) SetPoint(SelectedField.I + walkPawn[1, 0], SelectedField.J);
								}
								else if (SelectedField.PieceColor == Piece.Color.Black)
								{
									SetPoint(SelectedField.I - walkPawn[0, 0], SelectedField.J);
									if (SelectedField.I == 1) SetPoint(SelectedField.I - walkPawn[1, 0], SelectedField.J);
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
								break;
							case Piece.Type.Rook:
								break;
							case Piece.Type.King:
								break;
							case Piece.Type.Queen:
								break;
						}
					}
					else
					{
						if (field == SelectedField)
						{
							SelectedField.Selected = false;
							SelectedField = null;
							ClearPoints();
						}
						else if (field != SelectedField & field.PieceColor == Move)
						{
							SelectedField.Selected = false;
							ClearPoints();
							SelectedField = field;
							SelectedField.Selected = true;
							/*
							if (SelectedField.PieceType == Piece.Type.Pawn & SelectedField.PieceColor == Piece.Color.White)
							{
								SetPoint(SelectedField.I + PawnModel.walk[0, 0], SelectedField.J);
								if (SelectedField.I == 6) SetPoint(SelectedField.I + PawnModel.walk[0, 0] * 2, SelectedField.J);
							}
							else if (SelectedField.PieceType == Piece.Type.Pawn & SelectedField.PieceColor == Piece.Color.Black)
							{
								SetPoint(SelectedField.I - PawnModel.walk[0, 0], SelectedField.J);
								if (SelectedField.I == 1) SetPoint(SelectedField.I - PawnModel.walk[0, 0] * 2, SelectedField.J);
							}
							*/

						}
					}
				}

			}
			else
			{
				if (SelectedField != null & field.TexturePath == TexturesPaths.Point)
				{

					Field tempField = field.Clone();
					field.PieceColor = SelectedField.PieceColor;
					field.PieceType = SelectedField.PieceType;
					field.TexturePath = SelectedField.TexturePath;
					SelectedField.PieceColor = tempField.PieceColor;
					SelectedField.PieceType = tempField.PieceType;
					SelectedField.TexturePath = tempField.TexturePath;
					SelectedField.Selected = false;
					SelectedField = null;
					ClearPoints();
					Move = color == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;

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
