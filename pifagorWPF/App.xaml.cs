using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
public class TransparentShape : UserControl
{
    private Path _path;
    private RotateTransform _rotateTransform = new RotateTransform(0);
    public Color ShapeColor { get; set; } = Colors.Green;
    public double Alpha { get; set; } = 1; // Прозрачность (0-1)
    public ShapeType Shape { get; set; } = ShapeType.Rectangle;
    public Size ShapeSize { get; set; } = new Size(100, 100);
    public Color BorderColor { get; set; } = Colors.Black; // Цвет обводки
    public double BorderThickness { get; set; } = 1;
    public double RotationAngle { get; set; } = 0; // Начальный угол 0

    public TransparentShape()
    {
        _path = new Path
        {
            RenderTransform = _rotateTransform,
            RenderTransformOrigin = new Point(0.5, 0.5) // Центр вращения
        };

        Content = _path;
        UpdateShape(); // Установка фигуры
    }
    public void Rotate()
    {
        _rotateTransform.Angle += 45;
        if (_rotateTransform.Angle >= 360)
        {
            _rotateTransform.Angle = 0; // Сбрасываем на полный оборот

        }
        RotationAngle = _rotateTransform.Angle;
    }

    public void UpdateShape()
    {
        // Устанавливаем геометрию
        Geometry geometry = Shape switch
        {
            ShapeType.Rectangle => new RectangleGeometry(new Rect(0, 0, ShapeSize.Width, ShapeSize.Height)),
            ShapeType.Ellipse => new EllipseGeometry(new Rect(0, 0, ShapeSize.Width, ShapeSize.Height)),
            ShapeType.Triangle => CreateTriangleGeometry(),
            ShapeType.Parallelogram => CreateParallelogramGeometry(),
            ShapeType.Pentagon => CreatePentagonGeometry(),
            _ => throw new NotImplementedException()
        };

        // Применяем стиль
        _path.Data = geometry;
        _path.Fill = new SolidColorBrush(Color.FromArgb(
            (byte)(Alpha * 255), ShapeColor.R, ShapeColor.G, ShapeColor.B));
        _path.Stroke = new SolidColorBrush(BorderColor);
        _path.StrokeThickness = BorderThickness;
    }
     private Geometry CreateParallelogramGeometry()
     {


         // Координаты углов параллелограмма
         double width = ShapeSize.Width;
         double height = ShapeSize.Height;
         double skew = width * 0.3; // Смещение (наклон) параллелограмма

         // Координаты углов параллелограмма
         Point p1 = new Point(0, height);           // Левый нижний угол
         Point p2 = new Point(0, 0);             // Левый верхний угол (со смещением)
         Point p3 = new Point(width-width/3, -height);     // Правый верхний угол
         Point p4 = new Point(width, height-150);

         StreamGeometry geometry = new StreamGeometry();
         using (StreamGeometryContext ctx = geometry.Open())
         {
             ctx.BeginFigure(p1, isFilled: true, isClosed: true);
             ctx.LineTo(p2, isStroked: true, isSmoothJoin: true);
             ctx.LineTo(p3, isStroked: true, isSmoothJoin: true);
             ctx.LineTo(p4, isStroked: true, isSmoothJoin: true);
         }

         geometry.Freeze();
         return geometry;
     }
    
    private Geometry CreatePentagonGeometry()
    {
        double centerX = ShapeSize.Width / 2;
        double centerY = ShapeSize.Height / 2;
        double radius = Math.Min(centerX, centerY);

        Point[] points = new Point[5];
        for (int i = 0; i < 5; i++)
        {
            double angle = Math.PI / 2 + i * 2 * Math.PI / 5;
            points[i] = new Point(
                centerX + radius * Math.Cos(angle),
                centerY - radius * Math.Sin(angle)
            );
        }

        StreamGeometry geometry = new StreamGeometry();
        using (StreamGeometryContext ctx = geometry.Open())
        {
            ctx.BeginFigure(points[0], isFilled: true, isClosed: true);
            for (int i = 1; i < 5; i++)
            {
                ctx.LineTo(points[i], isStroked: true, isSmoothJoin: true);
            }
        }

        geometry.Freeze();
        return geometry;
    }

    private Geometry CreateTriangleGeometry()
    {
        Point p1 = new Point(ShapeSize.Width / 2, 50);
        Point p2 = new Point(0, ShapeSize.Height);
        Point p3 = new Point(ShapeSize.Width, ShapeSize.Height);

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
    private Geometry CreateEllipseGeometry(Point position, Size size)
    {
        return new EllipseGeometry(new Rect(position.X, position.Y, size.Width, size.Height));
    }
}

public enum ShapeType
{
    Rectangle,
    Ellipse,
    Triangle,
    Parallelogram,
    Pentagon
}