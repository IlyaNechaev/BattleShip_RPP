using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Cell : ICell
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsEmpty { get; private set; }
        public bool IsHitted { get; set; }
        public int Size { get; }

        // Конструктор с координатами
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            IsEmpty = true;
            IsHitted = false;
        }

        public Cell(int x, int y, int size)
        {
            X = x;
            Y = y;
            Size = size;
            IsEmpty = true;
            IsHitted = false;
        }

        // Установить корабль в ячейку
        public void SetShip(IShip ship) 
        { 
            this.ship = ship;
            IsEmpty = false;
        }
        // Получить корабль, содержащийся в ячейке
        public IShip GetShip() { return ship; }
        // Ударить по ячейке
        public void Hit() 
        { 
            IsHitted = true;
        }

        // 
        public bool GetIsHitted() { return IsHitted; }
        public bool GetIsEmpty() { return IsEmpty; }
    }
}
