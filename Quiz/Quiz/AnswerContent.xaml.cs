using System;
using System.Collections.Generic;
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

namespace Quiz
{
    public partial class AnswerContent : UserControl
    {
        private bool isChecked = false;
        private bool disable = false;
        private Answer answer;
        public AnswerContent(Answer answer)
        {
            InitializeComponent();

            this.answer = answer;
            AnswerTextBlock.Text = answer.Name;
        }

        private void CheckCheckBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!disable)
            {
                isChecked = !isChecked;
                CheckCheckBox.IsChecked = isChecked;
            }
        }

        public bool IsChecked => isChecked;

        public string AnswerName => answer.Name;
        public int AnswerID => answer.ID;

        public void ChangeAnswerWeightToColor(string hex, bool change = false)
        {
            var converter = new BrushConverter();
            BackgroundCard.Background = change ? (Brush)converter.ConvertFromString(hex) : Brushes.White;
            AnswerTextBlock.Foreground = change ? Brushes.White : (Brush)converter.ConvertFromString("#4d4d4d");
        }

        public void Disable()
        {
            CheckCheckBox.IsEnabled = false;
            disable = true;
        }
    }
}
