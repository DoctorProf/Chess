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
using System.Security.Policy;

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

		int kingMoveBlack = 0;
		int kingMoveWhite = 0;
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
							fRow.Add(new Field() { I = i, J = j });
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
							cell.BackgroundColor = "#769655";
						}
						else
						{
							cell.BackgroundColor = "#eeeed2";
						}
					}
				}
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
		public void ClearCircle()
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (F[i][j].CircleTexture == TexturesPaths.Circle)
					{
						F[i][j].CircleTexture = TexturesPaths.Empty;
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
		#endregion
		public void PawnOnQueen(Field field)
		{
			if ((field.I == 0 || field.I == 7) & SelectedField.PieceType == Piece.Type.Pawn)
			{
				field.PieceType = Piece.Type.Queen;
			}
		}

		#region KickANdWalk Knight and King
		public void KickANdWalkKnight(int checkposi, int checkposj)
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
		public void KickANdWalkKing(int checkposi, int checkposj)
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
		public void CheckCastling()
		{
			if (F[7][4].PieceType == Piece.Type.King && F[7][4].PieceColor == Piece.Color.White && kingMoveWhite == 0)
			{
				if (F[7][5].PieceType == Piece.Type.Empty && F[7][6].PieceType == Piece.Type.Empty && F[7][7].PieceType == Piece.Type.Rook)
				{
					SetPoint(7, 6);
				}
				if (F[7][3].PieceType == Piece.Type.Empty && F[7][2].PieceType == Piece.Type.Empty && F[7][1].PieceType == Piece.Type.Empty && F[7][0].PieceType == Piece.Type.Rook)
				{
					SetPoint(7, 2);
				}
			}
			else if (F[0][4].PieceType == Piece.Type.King && F[0][4].PieceColor == Piece.Color.Black && kingMoveBlack == 0)
			{
				if (F[0][5].PieceType == Piece.Type.Empty && F[0][6].PieceType == Piece.Type.Empty && F[0][7].PieceType == Piece.Type.Rook)
				{
					SetPoint(0, 6);
				}
				if (F[0][3].PieceType == Piece.Type.Empty && F[0][2].PieceType == Piece.Type.Empty && F[0][1].PieceType == Piece.Type.Empty && F[0][0].PieceType == Piece.Type.Rook)
				{
					SetPoint(0, 2);
				}
			}
		}

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

			KickANdWalkKnight(checkposi, checkposj);
			KickANdWalkKnight(checkposi, checkposj1);
			KickANdWalkKnight(checkposi1, checkposj2);
			KickANdWalkKnight(checkposi2, checkposj2);
			KickANdWalkKnight(checkposi3, checkposj1);
			KickANdWalkKnight(checkposi3, checkposj);
			KickANdWalkKnight(checkposi2, checkposj3);
			KickANdWalkKnight(checkposi1, checkposj3);

		}
		public void AllKing()
		{
			int checkposi = SelectedField.I;
			int checkposi1 = SelectedField.I + 1;
			int checkposi2 = SelectedField.I - 1;

			int checkposj = SelectedField.J;
			int checkposj1 = SelectedField.J + 1;
			int checkposj2 = SelectedField.J - 1;

			KickANdWalkKing(checkposi2, checkposj);
			KickANdWalkKing(checkposi2, checkposj1);
			KickANdWalkKing(checkposi, checkposj1);
			KickANdWalkKing(checkposi1, checkposj1);
			KickANdWalkKing(checkposi1, checkposj);
			KickANdWalkKing(checkposi1, checkposj2);
			KickANdWalkKing(checkposi, checkposj2);
			KickANdWalkKing(checkposi2, checkposj2);
			CheckCastling();
		}
		#endregion
		public static bool CheckOnBoard(int i, int j)
		{
			return i >= 0 && i < 8 && j >= 0 && j < 8;
		}
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
		public void ReverseFigures(Field field, Piece.Color color)
		{
			Field tempField = field.Clone();
			field.PieceColor = SelectedField.PieceColor;
			field.PieceType = SelectedField.PieceType;
			SelectedField.PieceColor = tempField.PieceColor;
			SelectedField.PieceType = tempField.PieceType;
			SelectedField.Selected = false;
			SelectedField = null;
			ClearPoints();
			ClearCircle();
			Move = color == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;
		}
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
		public void Castling(Field field, Piece.Color color,int i, int j, int j1) 
		{
            field.PieceColor = SelectedField.PieceColor;
            field.PieceType = SelectedField.PieceType;
            SelectedField.PieceType = Piece.Type.Empty;
            SelectedField.PieceColor = Piece.Color.Empty;
			SelectedField.Selected = false;
            F[i][j].PieceType = Piece.Type.Rook;
            F[i][j].PieceColor = color;
            F[i][j1].PieceType = Piece.Type.Empty;
            F[i][j1].PieceColor = Piece.Color.Empty;
            ClearPoints();
            ClearCircle();
            Move = color == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;
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
						if (SelectedField.PieceType == Piece.Type.King)
						{
							if ((field.I == 7 && field.J == 6) && kingMoveWhite == 0)
							{
								Castling(field, color, 7, 5, 7);
							}
							else if ((field.I == 7 && field.J == 2) && kingMoveWhite == 0)
							{
                                Castling(field, color, 7, 3, 0);
							}
							else if ((field.I == 0 && field.J == 6) && kingMoveBlack == 0)
							{
                                Castling(field, color, 0, 5, 7);
							}
							else if ((field.I == 0 && field.J == 2) && kingMoveBlack == 0)
							{
                                Castling(field, color, 0, 3, 0);
							}
							else
							{
								ReverseFigures(field, color);
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
							ReverseFigures(field, color);
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
				f[1][i].PieceType = Piece.Type.Pawn;
				f[1][i].PieceColor = Piece.Color.Black;
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
				f[6][i].PieceType = Piece.Type.Pawn;
				f[6][i].PieceColor = Piece.Color.White;
			}
			#endregion
		}
		#endregion
	}
}