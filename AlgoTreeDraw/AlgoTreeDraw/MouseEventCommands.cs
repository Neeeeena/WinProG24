using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using AlgoTreeDraw.Model;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlgoTreeDraw
{
    class MouseEventCommands
    {
        public static Point initialMousePosition { get; set; }
        public static Point initialNodePosition { get; set; }
        private static Boolean moved = true;

        public Node MouseDownNode(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialNodePosition = new Point(node.X, node.Y);

            e.MouseDevice.Target.CaptureMouse();
            return node;
        }

        public Node MouseUpNode(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);

            e.MouseDevice.Target.ReleaseMouseCapture();


            if (node.X < 0 || node.Y < 0)
            {
                MessageBox.Show("wtf");
                node.X = initialNodePosition.X;
                node.Y = initialNodePosition.Y;
                moved = false;
            }
            if (moved && initialNodePosition.X == node.initialX && initialNodePosition.Y == node.initialY)
            {
                ///AddNode(node);
            }
            moved = true;
            return node;
        }

        public Node MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {

                var node = TargetShape(e);

                var mousePosition = RelativeMousePosition(e);

                var tempX = initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                var tempY = initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
                if ((initialNodePosition.X == node.initialX && initialNodePosition.Y == node.initialY) || !(tempX < 0 || tempY < 0))
                {
                    node.X = tempX;
                    node.Y = tempY;
                }
                return node;
            }
            return null;
        }

        //Non important functions
        private Node TargetShape(MouseEventArgs e)
        {
            var nodeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            return (Node)nodeVisualElement.DataContext;
        }

        private Point RelativeMousePosition(MouseEventArgs e)
        {
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            return Mouse.GetPosition(canvas);
        }

        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
    }
}
