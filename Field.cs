using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Field : IField
    {        

        ICell[,] cells;
        public int Height; // Высота поля
        public int Width; // Ширина поля

        // Конструктор с высотой и шириной поля
        public Field(int Height, int Width, int cellSize = 48)
        {
            this.Height = Height;
            this.Width = Width;
            createField(cellSize);
        }

        public Field()
        {
            Height = 10;
            Width = 10;
        }

        // Создание поля на основе размера ячейки
        public void createField(int cellSize)
        {
            cells = new Cell[Width, Height]; // Инициализация массива ячеек по размерам поля

            for (int x = 0, i = 0; i < Width; x += cellSize) // Создание ячеек с координатами
            {
                for (int y = 0, j = 0; j < Height; y += cellSize)
                {
                    cells[i, j] = new Cell(x, y);
                }

            }
        }
    }
}
