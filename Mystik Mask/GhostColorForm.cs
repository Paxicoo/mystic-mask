using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MysticMask
{
    public partial class GhostColorForm : Form
    {
        public string SelectedColorName { get; private set; }
        public string GhostName
        {
            get { return textBox1.Text; }
        }
        public GhostColorForm()
        {
            InitializeComponent();

            // Initialize colors
            Color yellow = Color.FromArgb(190, 216, 28);
            Color green = Color.FromArgb(48, 143, 68);
            Color orange = Color.FromArgb(223, 132, 27);
            Color pink = Color.FromArgb(247, 17, 244);
            Color grey = Color.FromArgb(90, 90, 90);
            Color blue = Color.FromArgb(86, 156, 211);
            Color red = Color.FromArgb(227, 17, 42);

            // Set colors to 'chooseX' pictureBox
            choose1.BackColor = yellow;
            choose2.BackColor = green;
            choose3.BackColor = orange;
            choose4.BackColor = pink;
            choose5.BackColor = red;
            choose6.BackColor = blue;
            

        }

        private void GhostColorForm_Load(object sender, EventArgs e)
        {

        }


        private void choose1_Click(object sender, EventArgs e)
        {
            ChangeGhostColor("yellow");
        }

        private void choose2_Click(object sender, EventArgs e)
        {
            ChangeGhostColor("green");
        }

        private void choose3_Click(object sender, EventArgs e)
        {
            ChangeGhostColor("orange");
        }

        private void choose4_Click(object sender, EventArgs e)
        {
            ChangeGhostColor("pink");
        }

        private void choose5_Click(object sender, EventArgs e)
        {
            ChangeGhostColor("red");
        }

        private void choose6_Click(object sender, EventArgs e)
        {
            ChangeGhostColor("blue");
        }
        private void ChangeGhostColor(string colorName)
        {
            SelectedColorName = colorName;
            // Construct the resource name based on the color
            string resourceName = $"{colorName}Ghost";
            mainGhostPictureBox.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourceName);
            continueButton.Enabled = true;
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a name for your Rosa.", "Name Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceed with closing the form and returning DialogResult.OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
