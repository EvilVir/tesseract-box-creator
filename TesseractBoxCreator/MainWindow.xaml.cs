using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TesseractBoxCreator.Model;

namespace TesseractBoxCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Flags]
        protected enum CatchMode
        {
            None = 0,
            ResizeLeft = 1,
            ResizeRight = 2,
            ResizeTop = 4,
            ResizeBottom = 8,
            Move = ResizeLeft | ResizeRight | ResizeTop | ResizeBottom,
        }

        protected class Offsets
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double X2 { get; set; }
            public double Y2 { get; set; }

            public override string ToString()
            {
                return String.Format("X: {0}; Y: {1}; X2: {2}; Y2: {3}", X, Y, X2, Y2);
            }
        }

        protected const double ResizeMargin = 5d;

        protected MainWindowViewModel viewModel = new MainWindowViewModel();

        protected Offsets mouseCatchOffset;
        protected BoxItem mouseCatchItem = null;
        protected CatchMode mouseCatchMode;

        public MainWindow()
        {
            InitializeComponent();
            viewModel.Initialize();
            DataContext = viewModel;
        }

        protected void ItemsControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(pageBoxes);

            FrameworkElement element = pageBoxes.InputHitTest(p) as FrameworkElement;
            BoxItem item = null;

            while (item == null && element != null)
            {
                item = element.Tag as BoxItem;
                if (item == null)
                {
                    element = element.Parent as FrameworkElement;
                }
            }

            if (element != null && item != null)
            {
                mouseCatchItem = item;
                viewModel.CurrentBoxes.SelectedBox = item;

                Point offsetP = e.GetPosition(element);

                mouseCatchOffset = new Offsets()
                {
                    X = offsetP.X,
                    Y = mouseCatchItem.Height - offsetP.Y,
                    X2 = mouseCatchItem.Width - offsetP.X,
                    Y2 = offsetP.Y,
                };

                // Determine mode
                {
                    mouseCatchMode = CatchMode.None;

                    if (mouseCatchOffset.X <= ResizeMargin) { mouseCatchMode |= CatchMode.ResizeLeft; }
                    if (mouseCatchOffset.Y <= ResizeMargin) { mouseCatchMode |= CatchMode.ResizeBottom; }
                    if (mouseCatchOffset.X2 <= ResizeMargin) { mouseCatchMode |= CatchMode.ResizeRight; }
                    if (mouseCatchOffset.Y2 <= ResizeMargin) { mouseCatchMode |= CatchMode.ResizeTop; }

                    if (mouseCatchMode == CatchMode.None) { mouseCatchMode = CatchMode.Move; }
                }
            }
            else
            {
                mouseCatchItem = new BoxItem() { Letter = '?', X = (int)p.X, Y = (int)(pageBoxes.Height - p.Y), X2 = (int)p.X, Y2 = (int)(pageBoxes.Height - p.Y) };
                viewModel.CurrentBoxes.CurrentPageBoxes.Add(mouseCatchItem);
                viewModel.CurrentBoxes.SelectedBox = mouseCatchItem;
                mouseCatchMode = CatchMode.ResizeRight | CatchMode.ResizeBottom;
                mouseCatchOffset = new Offsets();
            }
        }

        protected void ItemsControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseCatchItem = null;
        }

        protected void ItemsControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(pageBoxes);

            if (mouseCatchItem != null)
            {
                int w = mouseCatchItem.Width;
                int h = mouseCatchItem.Height;

                mouseCatchItem.NotifyPropertyChangedEventDisabled = true;

                if (mouseCatchMode == CatchMode.Move)
                {
                    mouseCatchItem.X = (int)((p.X) - mouseCatchOffset.X);
                    mouseCatchItem.Y = (int)((pageBoxes.Height - p.Y) - mouseCatchOffset.Y);
                    mouseCatchItem.Width = w;
                    mouseCatchItem.Height = h;
                }
                else
                {
                    if ((mouseCatchMode & CatchMode.ResizeLeft) == CatchMode.ResizeLeft)
                    {
                        mouseCatchItem.X = (int)((p.X) - mouseCatchOffset.X);
                    }

                    if ((mouseCatchMode & CatchMode.ResizeRight) == CatchMode.ResizeRight)
                    {
                        mouseCatchItem.X2 = (int)((p.X) + mouseCatchOffset.X2);
                    }

                    if ((mouseCatchMode & CatchMode.ResizeTop) == CatchMode.ResizeTop)
                    {
                        mouseCatchItem.Y2 = (int)((pageBoxes.Height - p.Y) - mouseCatchOffset.Y2);
                    }

                    if ((mouseCatchMode & CatchMode.ResizeBottom) == CatchMode.ResizeBottom)
                    {
                        mouseCatchItem.Y = (int)((pageBoxes.Height - p.Y));
                    }
                }

                mouseCatchItem.NotifyPropertyChangedEventDisabled = false;
            }
        }

        private void PageScrollViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && viewModel.CurrentBoxes != null && viewModel.CurrentBoxes.SelectedBox != null)
            {
                viewModel.CurrentBoxes.CurrentPageBoxes.Remove(viewModel.CurrentBoxes.SelectedBox);
                viewModel.CurrentBoxes.SelectedBox = null;
                e.Handled = true;
            }
        }
    }
}
