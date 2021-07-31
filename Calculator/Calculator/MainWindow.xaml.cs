using Calculator.Model;
using DemoCalculator.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DemoCalculator
{

    public partial class MainWindow : Window
    {
        Double Input1 = 0;
        Double memoryValue = 0;
        List<Double> listMemoryValue = new List<Double>();

        bool isOperationSelected = false;

        List<CalcuHistoryModel> lstCalHistModel = new List<CalcuHistoryModel>();

        string prevAction = String.Empty;
        string prevValue = String.Empty;
        string errorMessage = String.Empty;

        Operators currentOperationType = new Operators();


        public MainWindow()
        {
            DataAccess da = new DataAccess();
            InitializeComponent();

            da.CreateXMLDatabase();

        }

        private void handleNumberClick(object sender, RoutedEventArgs e)
        {

            if (isOperationSelected)
            {
                txtValue.Text = "0";
                isOperationSelected = false;
            }

            Button btn = (Button)sender;
            txtValue.Text = (txtValue.Text == "0" ? btn.Content.ToString() : txtValue.Text + btn.Content.ToString());
        }

        private void handleDecimalClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            txtValue.Text = (!txtValue.Text.Contains(".") ? txtValue.Text + btn.Content.ToString() : txtValue.Text);

            addListHistory(Calculator.Model.Action.Decimal.ToString(), txtValue.Text);
        }

        private void handleSignClick(object sender, RoutedEventArgs e)
        {

            if (Convert.ToDouble(txtValue.Text) != 0)
            {
                txtValue.Text = (txtValue.Text.Contains("-") ? txtValue.Text.Remove(0, 1) : "-" + txtValue.Text);
            }

            addListHistory(Calculator.Model.Action.Sign.ToString(), txtValue.Text);
        }

        private void handleClearClick(object sender, RoutedEventArgs e)
        {
            Input1 = 0;
            txtValue.Text = "0";
            isOperationSelected = false;

            prevAction = String.Empty;
            prevValue = "0";

            addListHistory(Calculator.Model.Action.Clear.ToString(), txtValue.Text);
            updateGridview();
        }

        private void handleDeleteClick(object sender, RoutedEventArgs e)
        {
            txtValue.Text = (txtValue.Text.Length == 1 || txtValue.Text == "-" ? "0" : txtValue.Text.Remove(txtValue.Text.Length - 1, 1));
        }

        private void handleSelectOperation(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            ComboBoxItem cboxItem = (ComboBoxItem)cbox.SelectedItem;

            Enum.TryParse(cboxItem.Content.ToString(), out Operators opType);

            double currentInput;

            if (!isOperationSelected)
            {
                prevValue = txtValue.Text;
                addListHistory(prevAction, prevValue);
            }

            currentInput = Convert.ToDouble(txtValue.Text);

            if (!isOperationSelected && Input1 != 0)
            {
                Input1 = computeValue(Input1, currentInput, currentOperationType);
            }
            else
            {
                Input1 = currentInput;
            }

            prevAction = opType.ToString();

            txtValue.Text = Input1.ToString();

            currentOperationType = opType;

            isOperationSelected = true;

        }

        private void handleEqualsClick(object sender, RoutedEventArgs e)
        {
            double result = 0;
            double intpu2 = Convert.ToDouble(txtValue.Text);

            if (!string.IsNullOrEmpty(prevAction))
            {
                addListHistory(prevAction, txtValue.Text);
            }

            result = computeValue(Input1, intpu2, currentOperationType);
            Input1 = 0;
            isOperationSelected = false;
            txtValue.Text = result.ToString();

            addListHistory(Calculator.Model.Action.Equal.ToString(), string.IsNullOrEmpty(errorMessage) ? 
                            result.ToString() : ConfigurationManager.AppSettings["OperationError"].ToString());

            updateGridview();
        }

        private void handleMemoryAddClick(object sender, RoutedEventArgs e)
        {
            memoryValue += Convert.ToDouble(txtValue.Text);
            listMemoryValue.Add(memoryValue);
            updateListMemory();
        }

        private void handleMemoryMinusClick(object sender, RoutedEventArgs e)
        {
            memoryValue -= Convert.ToDouble(txtValue.Text);
            listMemoryValue.Add(memoryValue);
            updateListMemory();
        }

        private void handleMemoryRecallClick(object sender, RoutedEventArgs e)
        {
            txtValue.Text = memoryValue.ToString();
        }

        private void handleMemoryClearClick(object sender, RoutedEventArgs e)
        {
            memoryValue = 0;
            listMemoryValue.Add(memoryValue);
            updateListMemory();
        }

        private void KeyBoardPreviewKeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                    btn0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D1:
                case Key.NumPad1:
                    btn1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D2:
                case Key.NumPad2:
                    btn2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D3:
                case Key.NumPad3:
                    btn3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.D4:
                case Key.NumPad4:
                    btn4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.D5:
                case Key.NumPad5:
                    btn5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.D6:
                case Key.NumPad6:
                    btn6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.D7:
                case Key.NumPad7:
                    btn7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.D8:
                case Key.NumPad8:
                    btn8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.D9:
                case Key.NumPad9:
                    btn9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    btnDecimal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.Delete:
                case Key.Escape:
                    btnClear.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.Back:
                    btnDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;

                case Key.Add:
                    cboxOperation.SelectedIndex = 0;
                    cboxOperation.IsDropDownOpen = true;
                    cboxOperation.IsDropDownOpen = false;
                    break;
                case Key.Subtract:
                    cboxOperation.SelectedIndex = 1;
                    cboxOperation.IsDropDownOpen = true;
                    cboxOperation.IsDropDownOpen = false;
                    break;
                case Key.Multiply:
                    cboxOperation.SelectedIndex = 2;
                    cboxOperation.IsDropDownOpen = true;
                    cboxOperation.IsDropDownOpen = false;
                    break;
                case Key.Divide:
                    cboxOperation.SelectedIndex = 3;
                    cboxOperation.IsDropDownOpen = true;
                    cboxOperation.IsDropDownOpen = false;
                    break;

                case Key.Enter:
                    btnEquals.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
            }
        }

        private void handleExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void handleCopyClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtValue.Text);
        }

        private void handlePasteClick(object sender, RoutedEventArgs e)
        {
            double value;
            if (Double.TryParse(Clipboard.GetText(), out value))
            {
                txtValue.Text = value.ToString();
            }
        }

        private void handleExportHistClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            DataAccess da = new DataAccess();
            List<CalcuHistoryModel> list = new List<CalcuHistoryModel>();
            String text = String.Empty;

            saveFileDialog.FileName = ConfigurationManager.AppSettings["SaveFileName"].ToString();
            saveFileDialog.DefaultExt = ".text";
            saveFileDialog.Filter = "Text documents (.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                list = da.GetData();

                text += ConfigurationManager.AppSettings["FileHeader"].ToString() + "\n";

                foreach (CalcuHistoryModel item in list)
                {
                    text += string.Format(item.Hist_ID.ToString() + "\t" + item.Hist_Action + "\t" + item.Hist_Value + "\n");
                }
                File.WriteAllText(saveFileDialog.FileName, text);

                MessageBox.Show(ConfigurationManager.AppSettings["ExportSuccessMessage"].ToString(), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void handleImportHistClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            string line;
            List<CalcuHistoryModel> list = new List<CalcuHistoryModel>();
            bool isFirstRow = true;

            openFileDialog.DefaultExt = ".text";
            openFileDialog.Filter = "Text documents (.txt)|*.txt";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (isFirstRow)
                            {
                                if (line == ConfigurationManager.AppSettings["FileHeader"].ToString())
                                {
                                    isFirstRow = false;
                                    continue;
                                }
                                else
                                {
                                    throw new Exception(ConfigurationManager.AppSettings["ImportErrorMessage"].ToString());
                                }
                            }
                            list.Add(validateImport(line));
                        }

                        foreach (CalcuHistoryModel item in list)
                        {
                            addListHistory(item.Hist_Action, item.Hist_Value);
                        }
                        MessageBox.Show(ConfigurationManager.AppSettings["ImportSuccessMessage"].ToString(), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        updateGridview();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private CalcuHistoryModel validateImport(String line)
        {
            CalcuHistoryModel calcuHistoryModel = new CalcuHistoryModel();

            Double num = 0;
            int id = 0;
            bool withError = false;

            string[] item = line.Split('\t');


            if (item.Length == 3)
            {
                if (!Int32.TryParse(item[0], out id))
                {
                    withError = true;
                }

                if (!string.IsNullOrEmpty(item[1]))
                {
                    if (!Enum.IsDefined(typeof(Operators), item[1]) && !Enum.IsDefined(typeof(Calculator.Model.Action), item[1]))
                    {
                        withError = true;
                    }
                }

                if (!Double.TryParse(item[2], out num))
                {
                    if (!(item[2] == ConfigurationManager.AppSettings["OperationError"].ToString()))
                    {
                        withError = true;
                    }
                }
            }
            else
            {
                withError = true;
            }

            if (withError)
            {
                throw new Exception(ConfigurationManager.AppSettings["ImportErrorMessage"].ToString());
            }

            calcuHistoryModel.Hist_Action = item[1];
            calcuHistoryModel.Hist_Value = item[2];

            return calcuHistoryModel;
        }

        private double computeValue(Double Input1, Double currentInput, Operators currentOperationType)
        {

            CalcuHistoryModel calcuHistoryModel = new CalcuHistoryModel();
            double result = new double();
            try
            {
                result = calcuHistoryModel.computeValue(Input1, currentInput, currentOperationType);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                errorMessage = e.Message;
            }
            prevAction = string.Empty;

            return result;
        }

        private void updateListMemory()
        {
            lstMemory.Items.Clear();
            foreach (double mem in  listMemoryValue)
            {
                lstMemory.Items.Add(mem.ToString());
            }
        }

        private void addListHistory(String action, String result)
        {
            DataAccess da = new DataAccess();
            da.InsertData(new CalcuHistoryModel { Hist_Action = action, Hist_Value = result });
        }

        private void updateGridview()
        {
            DataAccess da = new DataAccess();
            listView.Items.Clear();

            foreach (CalcuHistoryModel item in da.GetData())
            {
                listView.Items.Add(item);
            }

        }

    }
}

