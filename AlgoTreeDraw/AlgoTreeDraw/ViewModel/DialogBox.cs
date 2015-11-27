using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.ViewModel
{
    //commit test, ignorer denne kommentar
    public class DialogBox
    {
        private static OpenFileDialog openDialog = new OpenFileDialog() { Title = "Open Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CheckFileExists = true };
        private static SaveFileDialog saveDialog = new SaveFileDialog() { Title = "Save Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
        private static SaveFileDialog saveImageDialog = new SaveFileDialog() { Title = "Export as PNG", Filter = "png Document (.png)|*.png", DefaultExt = "png", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };

        public bool ShowNew() =>
            MessageBox.Show("Are you sure want to open a new file without saving?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        public string ShowOpen() => openDialog.ShowDialog() == true ? openDialog.FileName : null;

        public string ShowSave() => saveDialog.ShowDialog() == true ? saveDialog.FileName : null;

        public string ShowImageSave() => saveImageDialog.ShowDialog() == true ? saveImageDialog.FileName : null;
    }
}
