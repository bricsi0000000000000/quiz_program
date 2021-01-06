using System.Windows.Controls;

namespace Quiz
{
    /// <summary>
    /// Interaction logic for CheckAnswerItem.xaml
    /// </summary>
    public partial class CheckQuestionItem : UserControl
    {
        public CheckQuestionItem(Question userQuestion, Question goodQuestion)
        {
            InitializeComponent();

            QuestionNumberLabel.Content = $"{userQuestion.ID + 1}. kérdés";
            QuestionNameTextBlock.Text = userQuestion.Name;

            for (int i = 0; i < userQuestion.Answers.Count; i++)
            {
                AnswersStackPanel.Children.Add(new CheckAnswerItem(userQuestion.Answers[i], goodQuestion.Answers[i]));
            }
        }
    }
}
