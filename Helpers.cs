using System.Collections.Generic;

namespace Tetris
{
    public static class Helpers
    {
        public static Dictionary<PieceType, int> PieceRotations = new Dictionary<PieceType, int> {
        { PieceType.I, 2 },
        { PieceType.O, 1 },
        { PieceType.J, 4 },
        { PieceType.L, 4 },
        { PieceType.S, 2 },
        { PieceType.Z, 2 },
        { PieceType.T, 4 },};


        public static Dictionary<PieceType, List<List<List<char>>>> _BitsDict = WarmUpBits();

        public static List<List<char>> GetBits(PieceType p, int r)
        {
            return _BitsDict[p][r];
        }

        private static Dictionary<PieceType, List<List<List<char>>>> WarmUpBits()
        {
            _BitsDict = new Dictionary<PieceType, List<List<List<char>>>>();
            _BitsDict[PieceType.I] = new List<List<List<char>>>() { CreateBits("I", "1,1,1,1"), CreateBits("I", "1111") };
            _BitsDict[PieceType.O] = new List<List<List<char>>>() { CreateBits("O", "11,11") };
            _BitsDict[PieceType.J] = new List<List<List<char>>>() { CreateBits("J", "11,1,1"), CreateBits("J", "111,001"), CreateBits("J", "01,01,11"), CreateBits("J", "1,111") };
            _BitsDict[PieceType.L] = new List<List<List<char>>>() { CreateBits("L", "1,1,11"), CreateBits("L", "111,1"), CreateBits("L", "11,01,01"), CreateBits("L", "001,111") };
            _BitsDict[PieceType.S] = new List<List<List<char>>>() { CreateBits("S", "1,11,01"), CreateBits("S", "011,11") };
            _BitsDict[PieceType.T] = new List<List<List<char>>>() { CreateBits("T", "1,11,1"), CreateBits("T", "111,01"), CreateBits("T", "01,11,01"), CreateBits("T", "01,111") };
            _BitsDict[PieceType.Z] = new List<List<List<char>>>() { CreateBits("Z", "01,11,1"), CreateBits("Z", "11,011") };
            return _BitsDict;
        }

        private static List<List<char>> CreateBits(string key, string format)
        {
            var parts = format.Split(',');
            var res = new List<List<char>>();
            foreach (var part in parts)
            {
                var row = new List<char>();
                foreach (var c in part)
                {
                    if (c == '0')
                    {
                        row.Add(c);
                    }
                    else
                    {
                        row.Add(key[0]);
                    }
                }
                res.Add(row);
            }
            return res;
        }
    }
}