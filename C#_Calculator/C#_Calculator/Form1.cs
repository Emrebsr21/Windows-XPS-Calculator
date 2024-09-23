using System;
using System.Data;
using System.Windows.Forms;

namespace C__Calculator
{
    public partial class Form1 : Form
    {
        private double result = 0;
        private double currentInput = 0;
        private string lastOperation = string.Empty;
        private double[] memory;
        private string pendingOperation = string.Empty;
        private bool waitForLastInput = false;
        private double displayedValue;
        private int lastOperationIndex = 0;
        private int historyPosition = 1;
        private string clipboardContent;


        public Form1()
        {
            InitializeComponent();

            zeroButton.Click += NumberButtonClick;
            oneButton.Click += NumberButtonClick;
            twoButton.Click += NumberButtonClick;
            threeButton.Click += NumberButtonClick;
            fourButton.Click += NumberButtonClick;
            fiveButton.Click += NumberButtonClick;
            sixButton.Click += NumberButtonClick;
            sevenButton.Click += NumberButtonClick;
            eightButton.Click += NumberButtonClick;
            nineButton.Click += NumberButtonClick;

            plusButton.Click += MathematicalOperationButtonClick;
            subtractButton.Click += MathematicalOperationButtonClick;
            multiplyButton.Click += MathematicalOperationButtonClick;
            divideButton.Click += MathematicalOperationButtonClick;
            sqrtButton.Click += MathematicalOperationButtonClick;
            percentButton.Click += MathematicalOperationButtonClick;
            inverseButton.Click += MathematicalOperationButtonClick;

            memoryAddButton.Click += MemoryOperationButtonClick;
            memoryClearButton.Click += MemoryOperationButtonClick;
            memoryResetButton.Click += MemoryOperationButtonClick;
            memoryStoreButton.Click += MemoryOperationButtonClick;
            historyButton.Click += MemoryOperationButtonClick;

            ceButton.Click += ClearButtonClick;
            clearButton.Click += ClearButtonClick;

            backspaceButton.Click += BackspaceButtonClick;
            decimalButton.Click += DecimalButtonClick;
            equalsButton.Click += CalculateButtonClick;
            changeSignButton.Click += ChangeSignClick;

            pasteEdit.Click += Paste;
            copyEdit.Click += Copy;

            aboutNewForm.Click += userGuideButtonClick;
            viewMenuItem.Click += viewEmre;

            memory = new double[10];
            for (int i = 0; i < memory.Length; i++)
            {
                memory[i] = 0;
                i++;
            }            
        }


        //Copy object to the clipboard.
        private void Copy(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(displayBox.Text))
            {
                clipboardContent = displayBox.Text;
                Clipboard.SetText(clipboardContent); 
            }
        }

        //Paste Clipboard to the display.
        private void Paste(object sender, EventArgs e)
        {
            string clipboardText = Clipboard.GetText(TextDataFormat.Text);

            if (!string.IsNullOrWhiteSpace(clipboardText))
            {
                displayBox.Text += clipboardText;
                if (double.TryParse(displayBox.Text, out double parsedValue))
                {
                    currentInput = parsedValue;
                }
                else
                {
                    MessageBox.Show("Invalid input.");
                    displayBox.Text = displayBox.Text.Substring(0, displayBox.Text.Length - clipboardText.Length); 
                }
            }
        }
        //Change Sign.
        private void ChangeSignClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(displayBox.Text)) 
            {
                double currentValue = double.Parse(displayBox.Text);
                double newValue = -currentValue;

                displayBox.Text = newValue.ToString();

                currentInput = newValue;
            }
        }

        //Print numbers on the screen 
        private void NumberButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string buttonText = button.Text;

            if (lastOperation == "=")
            {
                displayBox.Text = "";
                result = 0;
            }

            displayBox.Text += buttonText;
            lastOperation = string.Empty;

            // Update currentInput
            if (double.TryParse(displayBox.Text, out double number))
                currentInput = number;
        }

        //The operands calls this function, and this function Calls PerformCalculation and PerformUnique
        private void MathematicalOperationButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string operation = button.Text;

            if (lastOperation == "=")
            {
                displayBox.Text += " " + operation + " ";
            }
            else
            {
                displayBox.Text = displayBox.Text + " " + operation + " ";
            }

            lastOperation = operation;

            // Update currentInput
            if (double.TryParse(displayBox.Text, out double number))
            {
                currentInput = number;

                // If the operation is unique, perform the calculation
                if (operation == "sqrt" || operation == "1/x" || operation == "%")
                {
                    result = PerformUnique(currentInput, operation);
                    DisplayResult();
                    ClearDisplay();
                }
            }
        }


        //Equals button 
        private void CalculateButtonClick(object sender, EventArgs e)
        {
            try
            {
                // Try to parse the expression in the display box
                var expression = displayBox.Text;
                var result = new DataTable().Compute(expression, null);

                // Update displayBox.Text to show the result
                displayBox.Text = result.ToString();

                lastOperation = "=";
                pendingOperation = string.Empty;
                waitForLastInput = false;

                // Reset the history position after calculation
                historyPosition = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid expression: " + ex.Message);
            }
        }
        private double PerformCalculation(double leftOperand, double rightOperand)
        {
            double result = 0;

            switch (lastOperation)
            {
                case "+":
                    result = leftOperand + rightOperand;
                    break;
                case "-":
                    result = leftOperand - rightOperand;
                    break;
                case "*":
                    result = leftOperand * rightOperand;
                    break;
                case "/":
                    if (rightOperand != 0)
                        result = leftOperand / rightOperand;
                    else
                    {
                        MessageBox.Show("Division by 0 is not allowed.");
                        ClearAll();
                    }
                    break;
            }
            return result;
        }



        //THIS FUNCTION IS NOT WORKING (1/x , % , sqrt) is unfunctional.
        private double PerformUnique(double currentInput, string operation)
        {
            double result = 0;

            switch (operation)
            {
                case "sqrt":
                    if (currentInput >= 0)
                        result = Math.Sqrt(currentInput);
                    else
                    {
                        MessageBox.Show("Square root of a negative number is not allowed.");
                        ClearAll();
                    }
                    break;

                case "1/x":
                    if (currentInput != 0)
                        result = 1 / currentInput;
                    else
                    {
                        MessageBox.Show("Division by 0 is not allowed.");
                        ClearAll();
                    }
                    break;

                case "%":
                    result = currentInput / 100;
                    break;
            }
            return result;
        }


        //print decimal
        private void DecimalButtonClick(object sender, EventArgs e)
        {
            if (!displayBox.Text.Contains("."))
            {
                displayBox.Text += ".";
            }
        }

        //deletes the last index in display
        private void BackspaceButtonClick(object sender, EventArgs e)
        {
            if (displayBox.Text.Length > 0)
            {
                displayBox.Text = displayBox.Text.Remove(displayBox.Text.Length - 1, 1);

                double number;
                if (double.TryParse(displayBox.Text, out number))
                    currentInput = number;
                else
                    MessageBox.Show("Invalid input.");
            }
        }

        //Memory/history functions 
        private void MemoryOperationButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string operation = button.Text;

            switch (operation)
            {
                case "MC":
                    if (lastOperationIndex > 0 && lastOperationIndex <= memory.Length)
                    {
                        memory[lastOperationIndex - 1] = 0;
                        // Reset the history position
                        historyPosition = 0;

                        // Update the display to show 0
                        displayBox.Text = "0";
                    }
                    else
                    {
                        MessageBox.Show("No more history.");
                    }
                    break;

                case "MR":
                    if (lastOperationIndex > 0 && lastOperationIndex <= memory.Length)
                    {
                        // Update the display to show the memory value
                        displayBox.Text += " (" + memory[lastOperationIndex - 1] + ")";
                    }
                    else
                    {
                        MessageBox.Show("No more history.");
                    }
                    break;


                case "M+":
                    if (lastOperationIndex < memory.Length)
                    {
                        if (double.TryParse(displayBox.Text, out displayedValue))
                        {
                            memory[lastOperationIndex - 1] += displayedValue;

                            // Update the display to show the added memory value
                            displayBox.Text = memory[lastOperationIndex - 1].ToString();
                            lastOperationIndex++;
                        }
                        else
                        {
                            MessageBox.Show("Invalid input.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Memory is full.");
                    }
                    break;

                case "History":
                    if (lastOperationIndex > 0)
                    {
                        if (historyPosition < lastOperationIndex)
                        {
                            int historyIndex = lastOperationIndex - historyPosition - 1;

                            if (historyIndex >= 0 && historyIndex < memory.Length)
                            {
                                displayBox.Text = memory[historyIndex].ToString();
                                historyPosition++;
                            }
                            else
                            {
                                MessageBox.Show("No more history.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No more history.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No history available.");
                    }
                    break;

                case "MS":
                    if (lastOperationIndex < memory.Length)
                    {
                        displayedValue = double.Parse(displayBox.Text);
                        memory[lastOperationIndex] = displayedValue;
                        lastOperationIndex++;
                    }
                    else
                    {
                        MessageBox.Show("Memory is full.");
                    }
                    break;
            }
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string buttonText = button.Text;

            if (buttonText == "CE")
                currentInput = 0;

            ClearDisplay();

        }

        //Display function
        private void DisplayResult()
        {
            displayBox.Text = result.ToString();  // Display the result
        }

        //CE and C buttons
        private void ClearDisplay()
        {
            displayBox.Text = " ";
        }
        private void ClearAll()
        {
            displayBox.Text = " ";
            currentInput = 0;
            lastOperation = " ";
            lastOperationIndex = 0;
            DisplayResult();
        }

        private void userGuideButtonClick(object sender, EventArgs e)
        {
            // Open the user guide form
            UserGuideForm userGuideForm = new UserGuideForm();
            userGuideForm.ShowDialog(); 
        }
        private void viewEmre(object sender, EventArgs e)
        {
            MessageBox.Show("Fatih Emre Baser", "Proggrammed by;", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
