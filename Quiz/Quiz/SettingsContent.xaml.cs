using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Quiz
{
    public partial class SettingsContent : UserControl
    {
        private string questionsListName;
        private List<QuestionItem> questionItems = new List<QuestionItem>();
        private List<Answer> actualAnswers = new List<Answer>();

        int actualAnswersID = 0;
        int questionsID = 0;
        int actualQuestionID = 0;

        bool editMode = false;

        bool loadedQuestionsList = false;

        private string filePath;

        private readonly Snackbar errorSnackbar;

        public SettingsContent(ref Snackbar errorSnackbar)
        {
            InitializeComponent();

            this.errorSnackbar = errorSnackbar;

            UpdateEnabledElements();
        }

        private void UpdateEnabledElements()
        {
            QuestionNameTextBox.IsEnabled = loadedQuestionsList;
            AddAnswerButton.IsEnabled = loadedQuestionsList;
            AddQuestionButton.IsEnabled = loadedQuestionsList;
            QuestionsListNameTextBox.IsEnabled = loadedQuestionsList;
        }

        private void AddNewQuestionsListButton_Click(object sender, RoutedEventArgs e)
        {
            string listName = AddNewQuestionsListNameTextBox.Text;
            if (Regex.IsMatch(listName, @"^[a-zA-Z0-9_]+$"))
            {
                questionItems.Clear();
                questionsListName = AddNewQuestionsListNameTextBox.Text;
                UpdateQuestionsListNameTextBox();

                loadedQuestionsList = true;

                UpdateEnabledElements();

                AddNewQuestionsListNameTextBox.Text = string.Empty;

                var saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = $"Save {questionsListName}",
                    CheckPathExists = true,
                    DefaultExt = "json",
                    Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    filePath = saveFileDialog.FileName;
                    SaveQuestions();
                    QuestionManager.ClearQuestions();
                }
            }
            else
            {
                errorSnackbar.MessageQueue.Enqueue("Name can only contains letters, numbers and underscore.", null, null, null, false, true, TimeSpan.FromSeconds(4));
            }
        }

        private void ImportFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Read file",
                DefaultExt = ".json",
                Multiselect = false,
                Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                QuestionReader.ReadFile(filePath);
            }
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionNameTextBox.Text.Equals(string.Empty))
            {
                return;
            }

            actualAnswers.RemoveAll(x => x.Name == null);

            if (actualAnswers.Count == 0)
            {
                return;
            }

            if (editMode)
            {
                editMode = false;

                var questionItem = questionItems.Find(x => x.ID == actualQuestionID);
                var question = new Question(questionsID, QuestionNameTextBox.Text, new List<Answer>(actualAnswers));
                QuestionManager.SetQuestion(question);
                questionItem.SetQuestion(question);
            }
            else
            {
                actualAnswersID = 0;

                var question = new Question(questionsID, QuestionNameTextBox.Text, new List<Answer>(actualAnswers));
                QuestionManager.AddQuestion(question);

                var questionItem = new QuestionItem(question);
                QuestionListStackPanel.Children.Add(questionItem);
                questionItems.Add(questionItem);

                questionsID++;
            }

            QuestionNameTextBox.Text = string.Empty;
            AnswersStackPanel.Children.Clear();
            actualAnswers.Clear();

            ChangeAddOrEditQuestionIcon();

            SaveQuestions();
        }

        private void AddAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualAnswers.Count > 0)
            {
                if (actualAnswers.Last().Name == null)
                {
                    return;
                }
            }

            AnswersStackPanel.Children.Add(new AnswerItem(actualAnswersID));
            actualAnswers.Add(new Answer(actualAnswersID));
            actualAnswersID++;

            SaveQuestions();
        }

        private void SaveQuestions()
        {
            var questions = new List<Question>();
            foreach (var item in questionItems)
            {
                questions.Add(item.Question);
            }

            using (var writer = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, questions);
            }
        }

        private void UpdateQuestionsListNameTextBox()
        {
            QuestionsListNameTextBox.Text = questionsListName;
        }

        private void QuestionsListNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            questionsListName = QuestionsListNameTextBox.Text;
        }

        private Answer GetActualAnswer(int id)
        {
            return actualAnswers.Find(x => x.ID == id);
        }

        public void ChangeActualAnswerName(int id, string newName)
        {
            var answer = GetActualAnswer(id);
            if (answer != null)
            {
                answer.Name = newName;
            }
        }

        public void ChangeActualAnswerIsChecked(int id, bool right)
        {
            var answer = GetActualAnswer(id);
            if (answer != null)
            {
                GetActualAnswer(id).Right = right;
            }
        }

        public void ChangeActiveQuestion(Question question)
        {
            editMode = true;
            ChangeAddOrEditQuestionIcon();

            actualQuestionID = question.ID;

            QuestionNameTextBox.Text = question.Name;

            AnswersStackPanel.Children.Clear();
            actualAnswers.Clear();

            foreach (Answer item in question.Answers)
            {
                actualAnswers.Add(item);
                AnswersStackPanel.Children.Add(new AnswerItem(item));
            }

            actualAnswersID = actualAnswers.Count;
        }

        private void ChangeAddOrEditQuestionIcon()
        {
            AddQuestionButtonIcon.Kind = editMode ? PackIconKind.Edit : PackIconKind.PlaylistAdd;
        }

        public void DeleteQuestion(int id)
        {
            questionItems.RemoveAt(questionItems.FindIndex(x => x.ID == id));

            InitQuestionItems();
        }

        private void InitQuestionItems()
        {
            QuestionListStackPanel.Children.Clear();

            foreach (QuestionItem item in questionItems)
            {
                var questionItem = new QuestionItem(item);
                QuestionListStackPanel.Children.Add(questionItem);
            }

            QuestionNameTextBox.Text = string.Empty;

            AnswersStackPanel.Children.Clear();
            actualAnswers.Clear();

            loadedQuestionsList = true;

            UpdateEnabledElements();
        }

        public void UpdateQuestionItems(string fileName)
        {
            actualAnswersID = 0;
            questionsID = 0;

            editMode = false;

            loadedQuestionsList = true;

            questionItems.Clear();

            foreach (var question in QuestionManager.Questions)
            {
                questionItems.Add(new QuestionItem(question));
            }

            questionsID = questionItems.Count - 1;

            QuestionNameTextBox.Text = fileName;

            UpdateEnabledElements();
            InitQuestionItems();
        }
    }
}
