using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    internal class PawnModel : PieceModel
    {
        public static int[,] walk = new[,]
        {
           {0, -1}
       };
        public static int[,] kill = new[,]
        {
            {-1, -1},
            {1, -1},
        };
        
    }
}
