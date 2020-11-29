using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Board
    {
        /// <summary>
        /// 0,0 is LL corner
        /// 0,1 is leftmost column going upwards.
        /// 1,2 is 2nd column, 2nd cell up.
        /// last element has to be true; later entries assumed empty
        /// </summary>
        public List<List<char>> Columns { get; set; } = new List<List<char>>();
        public int Lines { get; set; }

        public int GetNonZero(List<char> row)
        {
            for (var ii = 0; ; ii++)
            {
                if (row[ii] != '0')
                {
                    return ii;
                }
            }
            throw new Exception("");
        }

        private int BoardWidth { get; set; } = 4;
        private int BoardHeight { get; set; } = 4;

        public Board()
        {
            for (var ii = 0; ii < BoardWidth; ii++)
            {
                Columns.Add(new List<char>());
            }
        }

        /// <summary>
        /// Only drops for now
        /// </summary>
        public Board PlayPiece(PieceType p, int globalPieceX, int r)
        {
            if (Lines > 100)
            {
                return null;
            }
            //project downwards looking for intersections.
            //when intersection, modify board to contain the new squares filled in
            var bits = Helpers.GetBits(p, r);

            //so for T0 we have bits=([1],[1,1],[1])
            //width is len(bits)
            //first figure out if it's illegal (width > width of board)
            var width = bits.Count;
            if (width + globalPieceX > BoardWidth)
            {
                return null;
            }

            //then figure out the contact point (which col of the piece hits)
            //the global height at which the piece contacts the board
            var globalPieceContactHeight = 0;

            var pieceCol = 0;
            foreach (var col in bits)
            {
                var actualLocation = globalPieceX + pieceCol;

                var pieceFirstNonZero = GetNonZero(col);
                var candidate = Columns[actualLocation].Count - pieceFirstNonZero;
                globalPieceContactHeight = Math.Max(candidate, globalPieceContactHeight);
                pieceCol++;
            }

            if (globalPieceContactHeight >= BoardHeight)
            {
                return null;
            }

            var copy = this.Copy();

            //then adjust board
            var pieceCol2 = 0;
            foreach (var col in bits)
            {
                var globalColumn = pieceCol2 + globalPieceX;
                var diff = copy.Columns[globalColumn].Count - globalPieceContactHeight;
                if (diff < 0)
                {
                    for (var ii = 0; ii < -diff; ii++)
                    {
                        copy.Columns[globalColumn].Add('0');
                    }
                }
                copy.Columns[globalColumn].AddRange(col.Skip(diff));
                pieceCol2++;
            }

            //then remove lines
            var clearedLineHeights = new List<int>();
            for (var globalRow = globalPieceContactHeight; globalRow < globalPieceContactHeight + 4; globalRow++)
            {
                if (copy.RowIsFull(globalRow))
                {
                    clearedLineHeights.Add(globalRow);
                }
            }

            //remove from the top down
            clearedLineHeights.Reverse();

            if (clearedLineHeights.Any())
            {
                foreach (var col in copy.Columns)
                {
                    //delete entries at clearedLH;
                    foreach (var removed in clearedLineHeights)
                    {
                        col.RemoveAt(removed);
                    }
                }
                copy.Lines += clearedLineHeights.Count;
                //Console.WriteLine($"Cleared Lines {string.Join(',', clearedLineHeights)}");
            }

            //then check for bubbles and fail out

            return copy;

        }

        private bool RowIsFull(int globalRow)
        {
            return Columns.All(el => el.Count > globalRow && el[globalRow] != '0');
        }

        private Board Copy()
        {
            var b = new Board();
            b.Columns = this.Columns.Select(el => el.GetRange(0, el.Count)).ToList();
            b.Lines = Lines;
            return b;
        }

        public override string ToString()
        {
            //TODO fix this
            return string.Join(",", Columns.Select(el => string.Join("", el.Select(el2 => el2 == '0' ? "0" : "1"))));
        }

        public void Draw()
        {
            Console.WriteLine("--------------------");
            foreach (var col in Columns)
            {
                foreach (var c in col)
                {
                    var thechar = c == '0' ? ' ' : c;
                    Console.Write(thechar);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("--------------------");
            Console.WriteLine("Lines:" + Lines);
        }
    }
}
