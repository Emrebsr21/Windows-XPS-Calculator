using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C__Calculator
{
    public partial class UserGuideForm : Form
    {
        public UserGuideForm()
        {
            InitializeComponent();

            // Set the user manual text when the form loads
            textBox1.Text = GetUserManualText();
        }

        private string GetUserManualText()
        {
            return "Hello, This is a User manual for my calculator. " +
                    "You can print any number, operand just by pressing the corresponding button.\r\n" +
                    "You can store the displayed number with MS, recall that with MR, " +
                    "add your stored number to displayed number with M+, and bring back your results with history.\r\n" +
                    "CE clears the display, C clears everything.\r\n" +
                    "Change the sign of a displayed number with (+/-) buttons.\r\n" +
                    "For a short amount of time, (1/x , %, and sqrt) functions are not available." +

                    "Prepared by Emre Baser.";
        }
    }
}