using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    // Ячейка
    interface ICell
    {
        public int X { get; }
        public int Y { get; }
        public bool IsEmpty { get; }
        public bool IsHitted { get; set; }
        public int Size { get; }
        public IShip Ship { get; set; }
    }
    
    // Корабль
    interface IShip
    {
        public int Size { get; }
        public ICell GetCell(int index);
        public bool IsKilled { get; }
        public bool NeedToBeKilled { get; }
        //TODO: Hit event 
    }

    // Поле
    interface IField
    {
        public int Width { get; }
        public int Height { get; }
        public IEnumerable<IShip> Ships { get; }
        public ICell this[int x, int y] { get; }
    }
}
