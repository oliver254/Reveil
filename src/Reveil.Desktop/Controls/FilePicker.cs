using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Reveil.Controls
{
    [TemplatePart(Name = FilePathPart, Type = typeof(TextBox)),
    TemplatePart(Name = OpenPart, Type = typeof(Button))]
    public class FilePicker : Control
    {

        #region Champs
        private const string FilePathPart = "PART_FilePath";
        private const string OpenPart = "PART_Open";

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register(nameof(FilePath),
                typeof(string),
                typeof(FilePicker),
                new UIPropertyMetadata());

        private TextBox _pathTextBox;
        private Button _openButton;
        #endregion

        #region Constructeurs
        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePicker),
                new FrameworkPropertyMetadata(typeof(FilePicker)));
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit le chemin du fichier.
        /// </summary>
        public string FilePath
        {
            get
            {
                return (string)GetValue(FilePathProperty);
            }
            set
            {
                SetValue(FilePathProperty, value);
            }
        }
        #endregion

        #region Méthodes
        public override void OnApplyTemplate()
        {
            _pathTextBox = (TextBox)GetTemplateChild(FilePathPart);
            _pathTextBox.IsReadOnly = true;

            _openButton = (Button)GetTemplateChild(OpenPart);
            _openButton.Click += OpenButton_Click;

        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Musiques|*.mp3|Tous les fichiers|*.*";
            bool? bresult = dlg.ShowDialog(Window.GetWindow(this));

            if (bresult == null || !bresult.Value)
            {
                return;
            }
            FilePath = dlg.FileName;
        }
        #endregion
    }
}
