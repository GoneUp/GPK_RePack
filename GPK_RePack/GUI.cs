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
using GPK_RePack.Parser;
using NLog;

namespace GPK_RePack
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        #region def
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Reader reader;
        #endregion

        #region Main

        private void GUI_Load(object sender, EventArgs e)
        {
            reader = new Reader();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.ValidateNames = true;
            open.InitialDirectory = Directory.GetCurrentDirectory();

            open.ShowDialog();

            foreach (var path in open.FileNames)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        reader.ReadGpk(path);
                    }
                    catch (Exception ex)
                    {
                        logger.FatalException("Parse failure! ", ex);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

    }
}
