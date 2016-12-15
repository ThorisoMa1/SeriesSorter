using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nameless
{
    public partial class seasonSpec : Form
    {
        private int[] seasons;
        public static  int SeasonNumber;
        public seasonSpec()
        {
            InitializeComponent();
        }

        private void seasonSpec_Load(object sender, EventArgs e)
        {
            SeasonPop();
            txtBoxSeason.DataSource = seasons;
        }
        private void seasonSpec_Close(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void SeasonPop() //populate seasons array
        {
            seasons = new int[100];

            for (int i = 0; i < seasons.Length; i++)
            {
                seasons[i] = i;
            }
        }
        private void txtBoxSeason_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SeasonNumber = int.Parse(txtBoxSeason.SelectedItem.ToString());
            this.Close();
        }
    }
}
