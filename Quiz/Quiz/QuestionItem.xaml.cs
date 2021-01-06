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
    public partial class QuestionItem : UserControl
    {
        public Question Question { get; private set; }
        public int ID => Question.ID;
        public QuestionItem(Question question)
        {
            InitializeComponent();

            Question = question;

            QuestionNumberLabel.Content = $"{question.ID + 1}.";
            QuestionNameLabel.Content = question.Name;
        }

        public QuestionItem(QuestionItem item)
        {
            InitializeComponent();

            Question = item.Question;

            QuestionNumberLabel.Content = $"{Question.ID + 1}.";
            QuestionNameLabel.Content = Question.Name;
        }

        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((SettingsContent)TabManager.GetTab("Settings").Content).ChangeActiveQuestion(Question);
        }

        private void DeleteQuestionItemButton_Click(object sender, RoutedEventArgs e)
        {
            ((SettingsContent)TabManager.GetTab("Settings").Content).DeleteQuestion(ID);
        }

        public void SetQuestion(Question question)
        {
            Question = question;
            QuestionNameLabel.Content = Question.Name;
        }

        public void ChangeColor(bool active = false)
        {
            var converter = new BrushConverter();
            BackgroundCard.Background = active ? (Brush)converter.ConvertFromString("#FF303030") : Brushes.White;
            QuestionNameLabel.Foreground = active ? Brushes.White : (Brush)converter.ConvertFromString("#FF303030");
            QuestionNumberLabel.Foreground = active ? Brushes.White : (Brush)converter.ConvertFromString("#FF303030");
        }

        private void BackgroundCard_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void BackgroundCard_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
