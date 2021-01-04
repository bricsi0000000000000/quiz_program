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

        public AnswerContent(string answer)
        {
            InitializeComponent();

            AnswerTextBlock.Text = answer;
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

        public string AnswerName => AnswerTextBlock.Text;

        public void ChangeAnswerWeightToBold()
        {
            AnswerTextBlock.FontWeight = FontWeights.Bold;
        }

        public void Disable()
        {
            CheckCheckBox.IsEnabled = false;
            disable = true;
        }
    }
}
