using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

namespace Ahorcado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String word;
        String parsedWord;
        string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        int fallos = 0;
        int letrasAcertadas = 0;

        public MainWindow()
        {
            InitializeComponent();
            AddLetterButtons();
            StartNewGame(true);
        }
        
        private void StartNewGame(Boolean firstGame)
        {
            
            if(firstGame)
            {
                SelectRandomSet();
            }
            if (!firstGame)
            {
                DisableComboBoxWarnings();
                fallos = 0;
                hangmanImage.Source = new BitmapImage(new Uri(@"/assets/" + fallos + ".jpg", UriKind.Relative));
                letrasAcertadas = 0;
                EnableButtons(true);
                LettersStackPanel.Children.Clear();
                SurrenderButton.IsEnabled = true;
            }
            SelectWord();
            ViewBoxCheck();
            EnableComboBoxWarnings();
        }

        private void SelectRandomSet()
        {
            Random seed = new Random();
            int randomNumber = seed.Next(0, SetsComboBox.Items.Count);

            ComboBoxItem item = (ComboBoxItem)SetsComboBox.Items[randomNumber];
            item.IsSelected = true;
        }

        private void SelectWord()
        {
            word = GetSelectedSetWord().ToUpper();
            parsedWord = DeleteAccents(word);

            Border bottomBorder = new Border();
            bottomBorder.BorderThickness = new Thickness(0, 0, 0, 1);

            for (int i = 0; i < word.Length; i++)
            {
                if(word[i].ToString() == " ") letrasAcertadas++;

                TextBlock newLetter = new TextBlock
                {
                    Text = word[i].ToString() == " " ? " " : "_",
                    Tag = parsedWord[i].ToString(),
                    Margin = new Thickness(5),
                    FontSize = 90,
                    Width = 100,
                    TextAlignment = TextAlignment.Center,
                };
                LettersStackPanel.Children.Add(newLetter);
            }

        }

        private String DeleteAccents(String word)
        {
            
            String parsedWord = new String(word.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray()).Normalize(NormalizationForm.FormC);

            return parsedWord;
        }

        private void MostrarLetras()
        {
            for (int i = 0; i < word.Length; i++)
            {
                TextBlock letterBlock = (TextBlock)LettersStackPanel.Children[i];
                letterBlock.Text = word[i].ToString();
            }
        }

        private String GetSelectedSetWord()
        {
            ArrayList words = new ArrayList();

            String language = ((ComboBoxItem)LanguagesComboBox.SelectedItem).Tag.ToString();
            String selectedSet = ((ComboBoxItem)SetsComboBox.SelectedItem).Tag.ToString();

            String category = selectedSet + "_" + language;
            switch (category)
            {
                case "eidolons_ES": case "eidolons_US":
                    words.Add("Lilit");
                    words.Add("Garuda");
                    words.Add("Abe no Seimei");
                    words.Add("Bastet");
                    words.Add("Hiperion");
                    words.Add("Halloween Zashi");
                    words.Add("Otohime");
                    words.Add("Summer Persephone");
                    break;
                case "masteries_ES":
                    words.Add("Fanatismo");
                    words.Add("Romper la Defensa");
                    words.Add("Vigor");
                    break;
                case "masteries_US":
                    words.Add("Divine Power");
                    words.Add("Recapitulation of the Pistols");
                    words.Add("Theory of Ars Noxia");
                    break;
                case "items_ES":
                    words.Add("Rodajas de Sandía");
                    words.Add("Katana de Gea");
                    words.Add("Poción de Reiniciación de Gemas Secretas Superior");
                    words.Add("Pargo Rápido");
                    words.Add("Monopatín Rayo Cósmico");
                    break;
                case "items_US":
                    words.Add("Sturdy Axe");
                    words.Add("Titan Warrior Battle Boots");
                    words.Add("Purified Obsidian Bracelet");
                    words.Add("Purified Obsidian Bracelet");
                    words.Add("Beebis the Ostrich");
                    break;
                case "titles_ES":
                    words.Add("Eres adorable pero yo soy inmune");
                    words.Add("Siempre Joven");
                    words.Add("Blanco Nieve");
                    words.Add("Confidente de Skandia");
                    words.Add("Vinatero Sustituto");
                    words.Add("Caracolero");
                    break;
                case "titles_US":
                    words.Add("Avatar");
                    words.Add("Puzzle Pro");
                    words.Add("Poor Winner");
                    words.Add("Wind Walker");
                    break;
                default:
                    break;
            }

            Random seed = new Random();
            int randomNumber = seed.Next(0, words.Count);

            return words[randomNumber].ToString();
        }

        private void AddLetterButtons()
        {
            int columns = LetterButtonsUniformGrid.Columns;
            int rows = LetterButtonsUniformGrid.Rows;

            int contador = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Button letter = new Button
                    {
                        Content = letters[contador],
                        Tag = letters[contador],
                        Margin = new Thickness(5),
                    };
                    letter.Click += Letter_Click;
                    LetterButtonsUniformGrid.Children.Add(letter);

                    contador++;
                }
            }
        }

        private void ViewBoxCheck()
        {
            LettersViewbox.Child = LettersStackPanel;
            FrameworkElement child = LettersViewbox.Child as FrameworkElement;
            double childWidth = child.ActualWidth;

            if(childWidth > 2000)
            {
                MessageBox.Show("Ok");
                LettersViewbox.Child = null;
                ScrollViewer scrollviewer = new ScrollViewer();
                scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                LettersGrid.Children.Add(scrollviewer);
            }
        }

        private void CheckLetter(String letter)
        {
            Boolean isCorrect = false;
            for (int i = 0; i < word.Length; i++)
            {
                if(letter == parsedWord[i].ToString())
                {
                    TextBlock letterInTextBlock = (TextBlock)LettersStackPanel.Children[i];
                    letterInTextBlock.Text = word[i].ToString();
                    letrasAcertadas++;
                    isCorrect = true;
                }
            }

            if (isCorrect)
            {
                if(letrasAcertadas == word.Length)
                {
                    EndGame("¡Has ganado!");
                }
            }
            else
            {
                fallos++;
                hangmanImage.Source = new BitmapImage(new Uri(@"/assets/" + fallos + ".jpg", UriKind.Relative));
                if (fallos >= 7)
                {
                    EndGame("¡Has perdido! Mas suerte la próxima");
                }
            }
        }

        private void EndGame(String text)
        {
            EnableButtons(false);
            MostrarLetras();
            SurrenderButton.IsEnabled = false;
            MessageBox.Show(text, "¡Fin del juego!", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Letter_KeyDown(object sender, KeyEventArgs e)
        {    
            String letter = e.Key.ToString().ToUpper();
            if(letter == "OEM3")
            {
                letter = "Ñ";
            }
            if (letters.Contains(letter) || letter == "OEM3")
            {
                Boolean wasEnabled = DisableButton(letter);
                if(wasEnabled)
                {
                    CheckLetter(letter);
                }
            }
        }

        private Boolean DisableButton(String letter)
        {
            Boolean wasEnabled = false;
            foreach (Button button in LetterButtonsUniformGrid.Children)
            {
                if (button.Tag.ToString().ToUpper() == letter)
                {
                    wasEnabled = button.IsEnabled;
                    button.IsEnabled = false;
                }
            }

            return wasEnabled;
        }


        private void EnableButtons(Boolean enable)
        {
            foreach (Button button in LetterButtonsUniformGrid.Children)
            {
                button.IsEnabled = enable;
            }
        }

        private void Letter_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;

            CheckLetter(button.Tag.ToString().ToUpper());
            
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(false);
        }

        private void Surrender_Click(object sender, RoutedEventArgs e)
        {
            EndGame("Te has rendido...");
        }

        private void EnableComboBoxWarnings()
        {
            LanguagesComboBox.SelectionChanged += ComboBox_SelectionChanged;
            SetsComboBox.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void DisableComboBoxWarnings()
        {
            LanguagesComboBox.SelectionChanged -= ComboBox_SelectionChanged;
            SetsComboBox.SelectionChanged -= ComboBox_SelectionChanged;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("El cambio se efectuará al comenzar una nueva partida", "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
}
