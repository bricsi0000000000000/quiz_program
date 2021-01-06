using System.Windows.Controls;
using System.Windows.Media;

namespace Quiz
{
    /// <summary>
    /// Interaction logic for CheckAnswerItem.xaml
    /// </summary>
    public partial class CheckAnswerItem : UserControl
    {
        public CheckAnswerItem(Answer userAnswer, Answer goodAnswer)
        {
            InitializeComponent();

            AnswerNameTextBox.Text = userAnswer.Name;

            var converter = new BrushConverter();
            if (goodAnswer.Right)
            {
                BackgroundCard.Background = goodAnswer.Right ? (Brush)converter.ConvertFromString("#56c649") : Brushes.White;
            }
            else
            {
                BackgroundCard.Background = userAnswer.Right ? (Brush)converter.ConvertFromString("#dd2727") : Brushes.White;
            }

            AnswerNameTextBox.Foreground = goodAnswer.Right || userAnswer.Right ? Brushes.White : (Brush)converter.ConvertFromString("#4d4d4d");

            CheckCheckBox.IsChecked = userAnswer.Right;
        }
    }
}
