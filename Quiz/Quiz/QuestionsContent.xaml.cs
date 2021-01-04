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
    /// <summary>
    /// Interaction logic for QuestionsContent.xaml
    /// </summary>
    public partial class QuestionsContent : UserControl
    {
        /// <summary>
        /// True if in order, false if random
        /// </summary>
        private bool inOrder = true;
        private int actualQuestionNumber = 1;

        private List<Answer> actualAnswers = new List<Answer>();
        private List<Question> actualQuestions = new List<Question>();

        public QuestionsContent()
        {
            InitializeComponent();

            QuestionReader.ReadFile("questions.json");

            InOrderRadioButton.IsChecked = inOrder;
            RandomOrderRadioButton.IsChecked = !inOrder;
            QuestionNumberInputTextBox.IsEnabled = inOrder;

            AddNewQuestion();
        }

        private void AddNewQuestion()
        {
            if (actualQuestionNumber > QuestionManager.Questions.Count)
            {
                return;
            }

            actualAnswers.Clear();
            AnswersStackPanel.Children.Clear();
            GoodAnswerCountsLabel.Content = string.Empty;

            var question = QuestionManager.GetQuestion(actualQuestionNumber);

            QuestionNumberLabel.Content = $"{question.ID}. kérdés";
            QuestionTextBlock.Text = question.Name;

            QuestionNumberInputTextBox.Text = actualQuestionNumber.ToString();

            foreach (var answer in question.Answers)
            {
                AnswersStackPanel.Children.Add(new AnswerContent(answer.Name));

                AddAnswer(answer.Name, answer.Right);
            }

            actualQuestions.Add(question);
        }

        private void AddAnswer(string name, bool picked)
        {
            actualAnswers.Add(new Answer(name, picked));
        }

        private void CheckAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            int pickedGoodAnswers = 0;
            int goodAnswers = 0;
            int picks = 0;
            foreach (AnswerContent answerContent in AnswersStackPanel.Children)
            {
                var getGoodAnswer = actualAnswers.Find(x => x.Name.Equals(answerContent.AnswerName));
                if (getGoodAnswer.Right == true)
                {
                    answerContent.ChangeAnswerWeightToBold();
                    goodAnswers++;
                }

                if (answerContent.IsChecked == true)
                {
                    picks++;

                    if (answerContent.IsChecked == getGoodAnswer.Right)
                    {
                        pickedGoodAnswers++;
                    }
                }

                answerContent.Disable();
            }

            GoodAnswerCountsLabel.Foreground = goodAnswers == pickedGoodAnswers && picks == pickedGoodAnswers ? Brushes.Green : Brushes.Red;
            GoodAnswerCountsLabel.Content = $"{picks}/{pickedGoodAnswers}/{actualAnswers.Count}";
        }

        private void PrevQuestionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (inOrder)
            {
                actualQuestionNumber++;
            }

            AddNewQuestion();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            actualQuestions.Clear();
        }

        private void OrderRadioButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            inOrder = (bool)InOrderRadioButton.IsChecked;

            QuestionNumberInputTextBox.IsEnabled = !inOrder;
        }
    }
}
