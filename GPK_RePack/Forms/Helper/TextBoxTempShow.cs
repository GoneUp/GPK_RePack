using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace GPK_RePack.Forms.Helper
{
    class TextBoxTempShow
    {
        private readonly ToolStripItem box;
        private readonly Form mainForm;
        private readonly Timer changeTimer;
        private string defaultString;

        public TextBoxTempShow(ToolStripItem box, Form mainForm)
        {
            this.box = box;
            this.mainForm = mainForm;

            changeTimer = new Timer();
            changeTimer.Tick += Timer_Tick;
        }

        public void StartTimer(String defaultString, String changeString, int timeToShowInMs)
        {
            changeTimer.Stop();

            this.defaultString = defaultString;
            mainForm.Invoke(new Action(() =>
              box.Text = changeString
            ));

            changeTimer.Interval = timeToShowInMs;
            changeTimer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            changeTimer.Stop();
            mainForm.Invoke(new Action(() =>
                box.Text = defaultString
            ));
        }
    }
}
