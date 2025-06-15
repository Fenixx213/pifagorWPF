using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pifagorWPF
{
    public partial class MainWindow : Window
    {
        private List<TransparentShape> _pieces = new List<TransparentShape>();
        private TransparentShape _currentPiece;
        private Point _offset;
        int[] xcord = { 100, 200, 300, 400, 300,200, 100};
        int[] ycord = { 200, 100, 100, 200,  300,300, 200};
        private (Point Position, double Angle, ShapeType Type, Geometry Bounds)[] _correctPositions;

        int coors = 450;
        double tm = 2.5;


        public MainWindow()
        {
            InitializeComponent();
            InitializeTangramPieces();
            _correctPositions = new (Point, double, ShapeType, Geometry)[]
  {
        (new Point(315,166), 45, ShapeType.Rectangle, new RectangleGeometry(new Rect(0, 0, 70, 70))),

        (new Point(300,200), 90, ShapeType.Triangle, CreateTriangleGeometry(new Point(0, 0), 100)),
        (new Point(122, 133), 135, ShapeType.Triangle, CreateTriangleGeometry(new Point(0, 0), 100)),
        (new Point(122, 147), 45, ShapeType.Triangle, CreateTriangleGeometry(new Point(0, 0), 100)),
        (new Point(215, 235), 135, ShapeType.Triangle, CreateTriangleGeometry(new Point(0, 0), 100)),
        (new Point(137, 46), 315, ShapeType.Triangle, CreateTriangleGeometry(new Point(0, 0), 100)),
        (new Point(200, 200), 0, ShapeType.Parallelogram, CreateTriangleGeometry(new Point(0, 0), 100))

  };
            DrawBoundaries();
        }
        private void DrawBoundaries()
        {
            for (int i = 0; i < xcord.Length - 1; i++)
            {
                Line line = new Line
                {
                    X1 = xcord[i],
                    Y1 = ycord[i],
                    X2 = xcord[i + 1],
                    Y2 = ycord[i + 1],
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                GameCanvas.Children.Add(line);
            }
            /*  foreach (var correctPosition in _correctPositions)
              {
                  var boundary = new Path
                  {
                      Data = correctPosition.Bounds,
                      Stroke = Brushes.Black,
                      StrokeThickness = 2,
                     // Fill = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0)) // Полупрозрачный fill
                  };

                  // Применяем поворот для границы
                  var transform = new RotateTransform(correctPosition.Angle, correctPosition.Position.X, correctPosition.Position.Y);
                  boundary.RenderTransform = transform;
                  Console.WriteLine($"Position: {correctPosition.Position.X}, {correctPosition.Position.Y}");
                  if (correctPosition.Angle == 270)
                  {
                      Canvas.SetLeft(boundary, correctPosition.Position.X - 30);
                      Canvas.SetTop(boundary, correctPosition.Position.Y - 170);
                  }
                  else if (correctPosition.Angle == 180)
                  {
                      Canvas.SetLeft(boundary, correctPosition.Position.X);
                      Canvas.SetTop(boundary, correctPosition.Position.Y - 140);
                  }
                  else if (correctPosition.Angle == 90) {
                      Canvas.SetLeft(boundary, correctPosition.Position.X - 30);
                      Canvas.SetTop(boundary, correctPosition.Position.Y - 30);
                  }
                  else
                  {
                      Canvas.SetLeft(boundary, correctPosition.Position.X);
                      Canvas.SetTop(boundary, correctPosition.Position.Y);
                  }
                      GameCanvas.Children.Add(boundary);
              }*/
        }
        private Geometry CreateTriangleGeometry(Point position, double size)
        {
            Point p1 = new Point(position.X + size / 2, position.Y + 50);
            Point p2 = new Point(position.X, position.Y + size);
            Point p3 = new Point(position.X + size, position.Y + size);

            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(p1, isFilled: true, isClosed: true);
                ctx.LineTo(p2, isStroked: true, isSmoothJoin: true);
                ctx.LineTo(p3, isStroked: true, isSmoothJoin: true);
            }

            geometry.Freeze();
            return geometry;
        }
      
        private void Shape_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var shape = sender as TransparentShape;
            if (shape != null)
            {
                // Получаем позицию мыши относительно холста
                var mousePosition = e.GetPosition(GameCanvas);

                // Устанавливаем точку вращения на позицию мыши относительно фигуры
                shape.RenderTransformOrigin = new Point(mousePosition.X / shape.ActualWidth, mousePosition.Y / shape.ActualHeight);

                // Выполняем вращение
                shape.Rotate();

                // Получаем текущие координаты фигуры
                double left = Canvas.GetLeft(shape);
                double top = Canvas.GetTop(shape);

                // Пересчитываем позицию фигуры на холсте так, чтобы она оставалась на месте относительно мыши
                double newLeft = left + mousePosition.X - (left + shape.ActualWidth / 2);
                double newTop = top + mousePosition.Y - (top + shape.ActualHeight / 2);

                // Устанавливаем новую позицию фигуры
                Canvas.SetLeft(shape, newLeft);
                Canvas.SetTop(shape, newTop);
            }
        }

        private void InitializeTangramPieces()
        {
            // Прямоугольник
            var rectPiece = new TransparentShape
            {
                Shape = ShapeType.Rectangle,
                ShapeColor = Colors.Blue,
                Alpha = 0.5,
                ShapeSize = new Size(70, 70)
            };
            tm++;
            rectPiece.UpdateShape();
            AddShape(rectPiece);
            coors += 80;
            tm = 0;
            // Треугольники
            for (int i = 0; i < 1; i++)
            {
                var trianglePiece = new TransparentShape
                {
                    Shape = ShapeType.Triangle,
                    ShapeColor = Colors.Blue,
                    Alpha = 0.5,
                    ShapeSize = new Size(100, 100)
                };
                tm++;
                trianglePiece.UpdateShape();
                AddShape(trianglePiece);
            }
            tm = -1;
            coors += 110;
            for (int i = 0; i < 4; i++)
            {
                var trianglePiece = new TransparentShape
                {
                    Shape = ShapeType.Triangle,
                    ShapeColor = Colors.Blue,
                    Alpha = 0.5,
                    ShapeSize = new Size(140, 120)
                };
                tm++;
                trianglePiece.UpdateShape();
                AddShape(trianglePiece);
            }
            tm = 5;
            coors += 150;

            var parallelogramPiece = new TransparentShape
            {
                Shape = ShapeType.Parallelogram,
                ShapeColor = Colors.Blue,
                Alpha = 0.5,
                ShapeSize = new Size(150, 100)
            };
            tm++;
            parallelogramPiece.UpdateShape();
            AddShape(parallelogramPiece);
         
            // Pentagon
            /* var pentagonPiece = new TransparentShape
             {
                 Shape = ShapeType.Pentagon,
                 ShapeColor = Colors.Yellow,
                 Alpha = 0.8,
                 ShapeSize = new Size(120, 120)
             };
             pentagonPiece.UpdateShape();
             AddShape(pentagonPiece);*/


        }


        private void AddShape(TransparentShape shape)
        {
            _pieces.Add(shape);
            GameCanvas.Children.Add(shape);
            Canvas.SetLeft(shape, coors);
            Canvas.SetTop(shape, 50 + tm * 20);
            shape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            shape.MouseMove += Shape_MouseMove;
            shape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            shape.MouseRightButtonDown += Shape_MouseRightButtonDown;
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPiece = sender as TransparentShape;
            if (_currentPiece != null)
            {
                _offset = e.GetPosition(_currentPiece);
                _currentPiece.CaptureMouse();
            }
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentPiece != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(GameCanvas);
                Canvas.SetLeft(_currentPiece, position.X - _offset.X);
                Canvas.SetTop(_currentPiece, position.Y - _offset.Y);
            }
        }

      
        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentPiece != null)
            {
                _currentPiece.ReleaseMouseCapture();
                double pieceLeft = Canvas.GetLeft(_currentPiece);
                double pieceTop = Canvas.GetTop(_currentPiece);
                Rect pieceRect = new Rect(pieceLeft, pieceTop, _currentPiece.ActualWidth, _currentPiece.ActualHeight);

                // Погрешность для проверки
                const double Tolerance = 5.0;

                // Проверка на попадание в область
                for (int i = 0; i < _correctPositions.Length; i++)
                {
                    var correctPosition = _correctPositions[i];

                    // Проверяем, попала ли фигура в область (с использованием погрешности)
                    if (Math.Abs(pieceLeft - correctPosition.Position.X) <= Tolerance &&
                        Math.Abs(pieceTop - correctPosition.Position.Y) <= Tolerance &&
                        _currentPiece.Shape == correctPosition.Type)
                    {
                        // Проверка угла поворота
                        if (Math.Abs(_currentPiece.RotationAngle - correctPosition.Angle) <= 10)
                        {
                            // Фиксируем фигуру на нужной позиции
                            Canvas.SetLeft(_currentPiece, correctPosition.Position.X);
                            Canvas.SetTop(_currentPiece, correctPosition.Position.Y);
                            _currentPiece.RotationAngle = correctPosition.Angle; // Устанавливаем правильный угол

                           

                            break; // Выход из цикла после успешного размещения
                        }
                    }
                }

                _currentPiece = null; // Очистка ссылки на текущую фигуру после проверки
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            var usedPositions = new HashSet<int>();
            var misplacedPieces = new List<TransparentShape>();

            foreach (var piece in _pieces)
            {
                Console.WriteLine($"Checking piece: {piece.Shape}");
                double pieceLeft = Canvas.GetLeft(piece);
                double pieceTop = Canvas.GetTop(piece);
                var pieceBounds = new Rect(pieceLeft, pieceTop, piece.ActualWidth, piece.ActualHeight);

                Console.WriteLine($"Piece Position: {pieceLeft}, {pieceTop}");

                Console.WriteLine($"Piece Rotation: {piece.RotationAngle}");

                bool isPlacedCorrectly = false;

                for (int i = 0; i < _correctPositions.Length; i++)
                {
                    if (usedPositions.Contains(i)) continue;
                    var correctPosition = _correctPositions[i];

                    Console.WriteLine($"Checking against position {i}:");
                    Console.WriteLine($"Expected Shape: {correctPosition.Type}");
                    Console.WriteLine($"Expected Position: {correctPosition.Position.X}, {correctPosition.Position.Y}");
                    Console.WriteLine($"Expected Rotation: {correctPosition.Angle}");

                    if (piece.Shape != correctPosition.Type)
                    {
                        Console.WriteLine("Shape type mismatch.");
                        continue;
                    }

                    if (Math.Abs(pieceLeft - correctPosition.Position.X) > 10 ||
                         Math.Abs(pieceTop - correctPosition.Position.Y) > 10)
                    {
                        Console.WriteLine("Position mismatch.");
                        continue;
                    }

                    if (Math.Abs(piece.RotationAngle - correctPosition.Angle) > 5)
                    {
                        Console.WriteLine("Angle mismatch.");
                        continue;
                    }

               

                    Console.WriteLine("Piece placed correctly!");
                    isPlacedCorrectly = true;
                    usedPositions.Add(i);
                    break;
                }

                if (!isPlacedCorrectly)
                {
                    misplacedPieces.Add(piece);
                }
            }

            foreach (var piece in _pieces)
            {
                if (misplacedPieces.Contains(piece))
                {
                    piece.BorderBrush = Brushes.Red;
                    piece.BorderThickness = 3;
                }
                else
                {
                    piece.BorderBrush = Brushes.Transparent;
                    piece.BorderThickness = 0;
                }
            }

            if (misplacedPieces.Count == 0)
            {
                MessageBox.Show("Все фигуры правильно расположены!");
            }
            else
            {
                MessageBox.Show($"Есть ошибки! Неверно расположено фигур: {misplacedPieces.Count}");
            }
        }

    }
}
