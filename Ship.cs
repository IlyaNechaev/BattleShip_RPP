using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Ship : IShip
    {
        private ICell[] _cells;
        public int Size { get; }
        public bool IsKilled { get; }
        public bool NeedToBeKilled { get; }

        public Ship(int size, ICell[] cells)
        {
            if (size > 4 || size < 1)
                ;
            Size = size;
            IsKilled = false;
            NeedToBeKilled = true;
            _cells = new ICell[size];
            cells.CopyTo(_cells, 0);
        }

        public ICell GetCell(int index)
        {
            return _cells[index];
        }

        public GetHit()
        {
            for (int i = 0; i < Size; i++)
            {
                if 
            }
        }
    }
}
