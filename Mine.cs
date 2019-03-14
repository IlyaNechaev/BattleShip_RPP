using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Mine : IShip

    {
        public ICell _cell { get; }
        public int Size { get; }
        public bool IsKilled { get; private set; }
        public bool NeedToBeKilled { get; }

        public Mine(ICell cell)
        {
            Size = 1;
            IsKilled = false;
            NeedToBeKilled = false;
            _cell = cell;
        }

        public ICell GetCell(int index = 0)
        {
            return _cell;
        }

        public void Blow()
        {
            IsKilled = true;
        }
    }
}
