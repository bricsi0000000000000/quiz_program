using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private bool wrongs = false;
        private int actualQuestionNumber = 0;
        private int actualWrongQuestionNumber = 0;

        private List<Answer> actualAnswers = new List<Answer>();
        private List<Question> actualQuestions = new List<Question>();
        private List<int> alreadyAddedRandomQuestionIDs = new List<int>();

        private readonly Snackbar errorSnackbar;

        public QuestionsContent(ref Snackbar errorSnackbar)
        {
            InitializeComponent();

            InOrderRadioButton.IsChecked = inOrder;
            RandomOrderRadioButton.IsChecked = !inOrder;
            QuestionNumberInputTextBox.IsEnabled = inOrder;

            this.errorSnackbar = errorSnackbar;
        }

        public void AddNewQuestion()
        {
            if (actualQuestionNumber > QuestionManager.Questions.Count)
            {
                return;
            }

            actualAnswers.Clear();
            AnswersStackPanel.Children.Clear();
            GoodAnswerCountsLabel.Content = string.Empty;
            QuestionNumberLabel.Content = string.Empty;
            QuestionTextBlock.Text = string.Empty;

            var question = QuestionManager.GetQuestion(actualQuestionNumber);
            if (question == null)
            {
                return;
            }

            QuestionNumberLabel.Content = $"{question.ID + 1}. kérdés";
            QuestionTextBlock.Text = question.Name;

            QuestionNumberInputTextBox.Text = (actualQuestionNumber + 1).ToString();

            var rand = new Random();
            var shuffledAnswers = new List<Answer>(question.Answers);
            shuffledAnswers = shuffledAnswers.OrderBy(x => rand.Next()).ToList();
            foreach (var answer in shuffledAnswers)
            {
                AnswersStackPanel.Children.Add(new AnswerContent(answer));

                AddAnswer(answer.ID, answer.Name, answer.Right);
            }

            actualQuestions.Add(question);

            CheckAnswersButton.IsEnabled = true;
            NextQuestionButton.IsEnabled = false;
            PrevQuestionButton.IsEnabled = false;
        }

        private void AddAnswer(int id, string name, bool picked)
        {
            actualAnswers.Add(new Answer(id, name, picked));
        }

        private void CheckAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            var question = new Question(actualQuestionNumber, QuestionTextBlock.Text);
            int pickedGoodAnswers = 0;
            int goodAnswers = 0;
            int picks = 0;
            foreach (AnswerContent answerContent in AnswersStackPanel.Children)
            {
                question.Answers.Add(new Answer(answerContent.AnswerID)
                {
                    Name = answerContent.AnswerName,
                    Right = answerContent.IsChecked
                });

                var getGoodAnswer = actualAnswers.Find(x => x.Name.Equals(answerContent.AnswerName));
                if (getGoodAnswer.Right == true)
                {
                    answerContent.ChangeAnswerWeightToColor("#56c649", change: true);
                    goodAnswers++;
                }
                else
                {
                    if (answerContent.IsChecked)
                    {
                        answerContent.ChangeAnswerWeightToColor("#dd2727", change: true);
                    }
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

            if (!(goodAnswers == pickedGoodAnswers && picks == pickedGoodAnswers))
            {
                if (QuestionManager.WrongAnsweredQuestions.Find(x => x.ID == question.ID) == null)
                {
                    QuestionManager.WrongAnsweredQuestions.Add(question);
                }
            }
            else
            {
                if (QuestionManager.GetWrongQuestion(question.ID) != null)
                {
                    QuestionManager.RemoveWrongQuestion(question.ID);
                }
            }

            CheckAnswersButton.IsEnabled = false;
            NextQuestionButton.IsEnabled = true;
            PrevQuestionButton.IsEnabled = true;

            GoodAnswerCountsLabel.Foreground = goodAnswers == pickedGoodAnswers && picks == pickedGoodAnswers ? Brushes.Green : Brushes.Red;
            GoodAnswerCountsLabel.Content = $"{picks}/{pickedGoodAnswers}/{actualAnswers.Count}";

            ((CheckAnswersContent)TabManager.GetTab("Wrong answers").Content).UpdateQuestions();
        }

        private void PrevQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (wrongs)
            {
                if (inOrder)
                {
                    if (actualWrongQuestionNumber - 1 >= 0)
                    {
                        actualWrongQuestionNumber--;
                        actualQuestionNumber = QuestionManager.WrongAnsweredQuestions[actualWrongQuestionNumber].ID;
                        AddNewQuestion();
                    }
                    else
                    {
                        errorSnackbar.MessageQueue.Enqueue("This is the first question.", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }
                }
                else
                {
                    AddRandomWrongQuestion();
                }
            }
            else
            {
                if (inOrder)
                {
                    if (actualQuestionNumber - 1 >= 0)
                    {
                        actualQuestionNumber--;
                        AddNewQuestion();
                    }
                    else
                    {
                        errorSnackbar.MessageQueue.Enqueue("This is the first question.", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }
                }
                else
                {
                    AddRandomQuestion();
                }
            }
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (wrongs)
            {
                if (inOrder)
                {
                    if (actualWrongQuestionNumber + 1 < QuestionManager.WrongAnsweredQuestions.Count)
                    {
                        actualWrongQuestionNumber++;
                        actualQuestionNumber = QuestionManager.WrongAnsweredQuestions[actualWrongQuestionNumber].ID;
                        AddNewQuestion();
                    }
                    else
                    {
                        errorSnackbar.MessageQueue.Enqueue("No more questions.", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }
                }
                else
                {
                    AddRandomWrongQuestion();
                }
            }
            else
            {
                if (inOrder)
                {
                    if (actualQuestionNumber + 1 < QuestionManager.Questions.Count)
                    {
                        actualQuestionNumber++;
                        AddNewQuestion();
                    }
                    else
                    {
                        errorSnackbar.MessageQueue.Enqueue("No more questions.", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }
                }
                else
                {
                    AddRandomQuestion();
                }
            }
        }

        private void AddRandomQuestion()
        {
            var rand = new Random();

            if (alreadyAddedRandomQuestionIDs.Count == 0)
            {
                FillAlreadyAddedRandomQuestionIDs();
            }

            actualQuestionNumber = alreadyAddedRandomQuestionIDs[rand.Next(0, alreadyAddedRandomQuestionIDs.Count - 1)];
            alreadyAddedRandomQuestionIDs.Remove(actualQuestionNumber);

            AddNewQuestion();
        }

        private void AddRandomWrongQuestion()
        {
            var rand = new Random();

            if (alreadyAddedRandomQuestionIDs.Count == 0)
            {
                FillAlreadyAddedRandomWrongQuestionIDs();
            }

            actualQuestionNumber = alreadyAddedRandomQuestionIDs[rand.Next(0, alreadyAddedRandomQuestionIDs.Count - 1)];
            alreadyAddedRandomQuestionIDs.Remove(actualQuestionNumber);

            AddNewQuestion();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            actualWrongQuestionNumber = 0;
            actualQuestions.Clear();

            if (inOrder)
            {
                if (Regex.IsMatch(QuestionNumberInputTextBox.Text, @"^[0-9]+$"))
                {
                    actualQuestionNumber = int.Parse(QuestionNumberInputTextBox.Text) - 1;
                }
                else
                {
                    errorSnackbar.MessageQueue.Enqueue("Order number can only contains numbers.", null, null, null, false, true, TimeSpan.FromSeconds(4));
                    return;
                }
            }

            AddNewQuestion();

            ((CheckAnswersContent)TabManager.GetTab("Wrong answers").Content).ClearQuestionsStackPanelChildren();
            QuestionManager.WrongAnsweredQuestions.Clear();
        }

        private void OrderRadioButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            inOrder = !(bool)InOrderRadioButton.IsChecked;

            QuestionNumberInputTextBox.IsEnabled = inOrder;

            if (!inOrder)
            {
                if (wrongs)
                {
                    FillAlreadyAddedRandomWrongQuestionIDs();
                }
                else
                {
                    FillAlreadyAddedRandomQuestionIDs();
                }
            }
        }

        private void FillAlreadyAddedRandomQuestionIDs()
        {
            alreadyAddedRandomQuestionIDs.Clear();
            foreach (var item in QuestionManager.Questions)
            {
                alreadyAddedRandomQuestionIDs.Add(item.ID);
            }
        }

        private void FillAlreadyAddedRandomWrongQuestionIDs()
        {
            alreadyAddedRandomQuestionIDs.Clear();
            foreach (var item in QuestionManager.WrongAnsweredQuestions)
            {
                alreadyAddedRandomQuestionIDs.Add(item.ID);
            }
        }

        private void WrongOrderCheckBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            wrongs = !(bool)WrongOrderCheckBox.IsChecked;
        }
    }
}
