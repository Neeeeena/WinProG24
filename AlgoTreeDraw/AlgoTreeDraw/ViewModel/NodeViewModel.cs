using AlgoTreeDraw.Model;
using System;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using AlgoTreeDraw.Command;

namespace AlgoTreeDraw.ViewModel
{
    public class NodeViewModel : ViewModelBase
    {
        Boolean isEditing =false;
        public ObservableCollection<BST> BSTNodes {get; set; }

        private Point initialMousePosition;
        private Point initialNodePosition;

        public ICommand MouseDownNodeCommand { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseUpNodeCommand { get; }


        public ObservableCollection<RBT> RBTNodes { get; set; }
        public NodeViewModel(LineViewModel lvm)
        {
            BSTNodes = new ObservableCollection<BST>() {
                new BST() { X = -225, Y = 20, diameter = 50},
                new BST() { X = 300, Y = 100, diameter = 50}
            };
            
            RBTNodes = new ObservableCollection<RBT>()
            {
                new RBT() {X = 60, Y=90, diameter=50 },
                new RBT() {X= 120, Y=50,diameter=50 }
            };
            
            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
        }

        public void edit()
        {
            isEditing = true;
        }

        public void AddBstNode()
        {
            BSTNodes.Add(new BST { X = -225, Y = 20, diameter=50});
        }

        private void MouseDownNode(MouseButtonEventArgs e)
        {
            // Checks that a line is not being drawn.

            //if (!isAddingLine)
            //{
            // The Shape is gotten from the mouse event.
            var shape = TargetShape(e);
            // The mouse position relative to the target of the mouse event.
            var mousePosition = RelativeMousePosition(e);

            // When the shape is moved with the mouse, the MouseMoveShape method is called many times, 
            //  for each part of the movement.
            // Therefore to only have 1 Undo/Redo command saved for the whole movement, the initial position is saved, 
            //  during the start of the movement, so that it together with the final position, 
            //  from when the mouse is released, can become one Undo/Redo command.
            // The initial shape position is saved to calculate the offset that the shape should be moved.
            initialMousePosition = mousePosition;
            initialNodePosition = new Point(shape.X, shape.Y);

            // The mouse is captured, so the current shape will always be the target of the mouse events, 
            //  even if the mouse is outside the application window.
            e.MouseDevice.Target.CaptureMouse();
            //}
        }

        private void MouseUpNode(MouseButtonEventArgs e)
        {
            /* Used for adding a Line.
            if (isAddingLine)
            {
                // Because a MouseUp event has happened and a Line is currently being drawn, 
                //  the Shape that the Line is drawn from or to has been selected, and is here retrieved from the event parameters.
                var shape = TargetShape(e);
                // This checks if this is the first Shape chosen during the Line adding operation, 
                //  by looking at the addingLineFrom variable, which is empty when no Shapes have previously been choosen.
                // If this is the first Shape choosen, and if so, the Shape is saved in the AddingLineFrom variable.
                //  Also the Shape is set as selected, to make it look different visually.
                if (addingLineFrom == null) { addingLineFrom = shape; addingLineFrom.IsSelected = true; }
                // If this is not the first Shape choosen, and therefore the second, 
                //  it is checked that the first and second Shape are different.
                else if (addingLineFrom.Number != shape.Number)
                {
                    // Now that it has been established that the Line adding operation has been completed succesfully by the user, 
                    //  a Line is added using an 'AddLineCommand', with a new Line given between the two shapes chosen.
                    undoRedoController.AddAndExecute(new AddLineCommand(Lines, new Line() { From = addingLineFrom, To = shape }));
                    // The property used for visually indicating that a Line is being Drawn is cleared, 
                    //  so the View can return to its original and default apperance.
                    addingLineFrom.IsSelected = false;
                    // The 'isAddingLine' and 'addingLineFrom' variables are cleared, 
                    //  so the MainViewModel is ready for another Line adding operation.
                    isAddingLine = false;
                    addingLineFrom = null;
                    // The property used for visually indicating which Shape has already chosen are choosen is cleared, 
                    //  so the View can return to its original and default apperance.
                    RaisePropertyChanged(() => ModeOpacity);
                }
            }
            // Used for moving a Shape.
            
            e
            {*/
            // The Shape is gotten from the mouse event.
            var node = TargetShape(e);
                // The mouse position relative to the target of the mouse event.
                //var mousePosition = RelativeMousePosition(e);

                //// The Shape is moved back to its original position, so the offset given to the move command works.
                //shape.X = initialNodePosition.X;
                //shape.Y = initialNodePosition.Y;

            // Now that the Move Shape operation is over, the Shape is moved to the final position, 
            //  by using a MoveNodeCommand to move it.
            // The MoveNodeCommand is given the offset that it should be moved relative to its original position, 
            //  and with respect to the Undo/Redo functionality the Shape has only been moved once, with this Command.
            //undoRedoController.AddAndExecute(new MoveShapeCommand(shape, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));
            //new MoveNodeCommand(shape, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y);
                // The mouse is released, as the move operation is done, so it can be used by other controls.
                e.MouseDevice.Target.ReleaseMouseCapture();
            if (node.X < 0 || node.Y < 0)
            {
                MessageBox.Show("wtf");
                node.X = initialNodePosition.X;
                node.Y = initialNodePosition.Y;
            }
            //}
        
        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            // Checks that the mouse is captured and that a line is not being drawn.
            if (Mouse.Captured != null) //&& !isAddingLine)
            {
                // The Shape is gotten from the mouse event.
                var node = TargetShape(e);
                // The mouse position relative to the target of the mouse event.
                var mousePosition = RelativeMousePosition(e);

                // The Shape is moved by the offset between the original and current mouse position.
                // The View (GUI) is then notified by the Shape, that its properties have changed.
                var tempX = initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                var tempY = initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
                //if(tempX>0 && tempY > 0)
                //{
                    node.X = tempX;
                    node.Y = tempY;
              //  }
                
            }
        }

        private Node TargetShape(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var nodeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.

            return (Node)nodeVisualElement.DataContext;
        }

        private Point RelativeMousePosition(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // The canvas holding the shapes visual element, is found by searching up the tree of visual elements.
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            // The mouse position relative to the canvas is gotten here.
            return Mouse.GetPosition(canvas);
        }

        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }

    }
}