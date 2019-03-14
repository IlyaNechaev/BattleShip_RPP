using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Mine : IShip
    {
        ICell _cell;
        public int Size { get; }
        public bool IsKilled { get; }
        public bool NeedToBeKilled { get; }

        public Mine()
        {
            Size = 1;
            IsKilled = false;
            NeedToBeKilled = false;
        }

        public ICell GetCell(int index)
        {
            return _cell;
        }
    }
}
