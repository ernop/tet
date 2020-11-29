using System;
using System.Collections.Generic;

using static Tetris.Helpers;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = new Board();
            var seen = new HashSet<string>();
            Recurse(b, seen);
        }

        public static void Recurse(Board b, HashSet<string> seen)
        {
            foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
            {
                var location = 0;
                while (location < 10)
                {
                    for (var rot = 0; rot < PieceRotations[pieceType]; rot++)
                    {
                        var newBoard = b.PlayPiece(pieceType, location, rot);
                        if (newBoard == null)
                        {
                            continue;
                        }


                        if (seen.Contains(newBoard.ToString()))
                        {
                            continue;
                        }
                        if (seen.Count % 25000 == 0)
                        {
                            Console.WriteLine($"Seen: {seen.Count}");
                            newBoard.Draw();
                        }

                        seen.Add(newBoard.ToString());
                        Recurse(newBoard, seen);
                    }
                    location++;
                }
            }

        }
    }
}