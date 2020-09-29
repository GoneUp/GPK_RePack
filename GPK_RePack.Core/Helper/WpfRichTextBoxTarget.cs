// 
// Copyright (c) 2004-2011 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System.Diagnostics;
using System.Security.AccessControl;
using System.Windows.Controls;
using NLog;
using NLog.Targets;

#if !NET_CF && !MONO && !SILVERLIGHT

namespace GPK_RePack.Core.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using NLog.Config;
    using NLog.Internal;
    using System.Windows.Documents;
    using System.Windows.Media;
	using System.Linq;
    [Target("RichTextBox")]
    public sealed class WpfRichTextBoxTarget : TargetWithLayout
    {
        private int lineCount;
	    private int _width = 500;
	    private int _height = 500;
	    private static readonly TypeConverter colorConverter = new ColorConverter();

        static WpfRichTextBoxTarget()
        {
            var rules = new List<WpfRichTextBoxRowColoringRule>()
            {
                new WpfRichTextBoxRowColoringRule("level == LogLevel.Fatal", "White", "Red", FontStyles.Normal, FontWeights.Bold),
                new WpfRichTextBoxRowColoringRule("level == LogLevel.Error", "Red", "Empty", FontStyles.Italic, FontWeights.Bold),
                new WpfRichTextBoxRowColoringRule("level == LogLevel.Warn", "Orange", "Empty"),
                new WpfRichTextBoxRowColoringRule("level == LogLevel.Info", "Black", "Empty"),
                new WpfRichTextBoxRowColoringRule("level == LogLevel.Debug", "Gray", "Empty"),
                new WpfRichTextBoxRowColoringRule("level == LogLevel.Trace", "DarkGray", "Empty", FontStyles.Italic, FontWeights.Normal),
            };

            DefaultRowColoringRules = rules.AsReadOnly();
        }

        public WpfRichTextBoxTarget()
        {
            this.WordColoringRules = new List<WpfRichTextBoxWordColoringRule>();
            this.RowColoringRules = new List<WpfRichTextBoxRowColoringRule>();
            this.ToolWindow = true;
        }

        private delegate void DelSendTheMessageToRichTextBox(string logMessage, WpfRichTextBoxRowColoringRule rule);

        private delegate void FormCloseDelegate();

        public static ReadOnlyCollection<WpfRichTextBoxRowColoringRule> DefaultRowColoringRules { get; private set; }

        public string ControlName { get; set; }

        public string FormName { get; set; }

        [DefaultValue(false)]
        public bool UseDefaultRowColoringRules { get; set; }

        [ArrayParameter(typeof(WpfRichTextBoxRowColoringRule), "row-coloring")]
        public IList<WpfRichTextBoxRowColoringRule> RowColoringRules { get; private set; }

        [ArrayParameter(typeof(WpfRichTextBoxWordColoringRule), "word-coloring")]
        public IList<WpfRichTextBoxWordColoringRule> WordColoringRules { get; private set; }

        [DefaultValue(true)]
        public bool ToolWindow { get; set; }

        public bool ShowMinimized { get; set; }

	    public int Width
	    {
		    get { return _width; }
		    set { _width = value; }
	    }

	    public int Height
	    {
		    get { return _height; }
		    set { _height = value; }
	    }

	    public bool AutoScroll { get; set; }

        public int MaxLines { get; set; }

        internal Window TargetForm { get; set; }

        internal System.Windows.Controls.RichTextBox TargetRichTextBox { get; set; }

        internal bool CreatedForm { get; set; }

        protected override void InitializeTarget()
        {
			TargetRichTextBox = System.Windows.Application.Current.MainWindow.FindName(ControlName) as System.Windows.Controls.RichTextBox;

	        if (TargetRichTextBox != null) return;
            //this.TargetForm = FormHelper.CreateForm(this.FormName, this.Width, this.Height, false, this.ShowMinimized, this.ToolWindow);
            //this.CreatedForm = true;

            var openFormByName = System.Windows.Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.GetType().Name == FormName);
            if (openFormByName != null)
            {
                TargetForm = openFormByName;
                if (string.IsNullOrEmpty(this.ControlName))
                {
                   // throw new NLogConfigurationException("Rich text box control name must be specified for " + GetType().Name + ".");
					Trace.WriteLine("Rich text box control name must be specified for " + GetType().Name + ".");
                }

                CreatedForm = false;
				TargetRichTextBox = TargetForm.FindName(ControlName) as System.Windows.Controls.RichTextBox ;

                if (this.TargetRichTextBox == null)
                {
                   // throw new NLogConfigurationException("Rich text box control '" + ControlName + "' cannot be found on form '" + FormName + "'.");
					Trace.WriteLine("Rich text box control '" + ControlName + "' cannot be found on form '" + FormName + "'.");
                }
            }
            
			if(TargetRichTextBox == null)
            {
				TargetForm = new Window
				{
					Name = FormName,
					Width = Width,
					Height = Height,
					WindowStyle = ToolWindow ? WindowStyle.ToolWindow : WindowStyle.None,
					WindowState = ShowMinimized ? WindowState.Minimized : WindowState.Normal,
					Title = "NLog Messages"
				};
	            TargetForm.Show();

				TargetRichTextBox = new System.Windows.Controls.RichTextBox { Name = ControlName };
				var style = new Style(typeof(Paragraph));
				TargetRichTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
				style.Setters.Add(new Setter(Block.MarginProperty, new Thickness(0,0,0,0)));
				TargetRichTextBox.Resources.Add(typeof(Paragraph), style);
				TargetForm.Content = TargetRichTextBox;

                CreatedForm = true;
            }
        }

        protected override void CloseTarget()
        {
            if (CreatedForm)
            {
	            try
	            {
					TargetForm.Dispatcher.Invoke(() =>
					{
						TargetForm.Close();
						TargetForm = null;
					});
	            }
	            catch
	            {
	            }


                
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            WpfRichTextBoxRowColoringRule matchingRule = RowColoringRules.FirstOrDefault(rr => rr.CheckCondition(logEvent));

	        if (UseDefaultRowColoringRules && matchingRule == null)
            {
                foreach (var rr in DefaultRowColoringRules.Where(rr => rr.CheckCondition(logEvent)))
                {
	                matchingRule = rr;
	                break;
                }
            }

            if (matchingRule == null)
            {
                matchingRule = WpfRichTextBoxRowColoringRule.Default;
            }

            var logMessage = Layout.Render(logEvent);

	        if (System.Windows.Application.Current == null) return;

	        try
	        {
				if (System.Windows.Application.Current.Dispatcher.CheckAccess() == false)
				{
					System.Windows.Application.Current.Dispatcher.Invoke(() => SendTheMessageToRichTextBox(logMessage, matchingRule));
				}
				else
				{
					SendTheMessageToRichTextBox(logMessage, matchingRule);
				}
	        }
	        catch(Exception ex)
	        {
		        Debug.WriteLine(ex);
	        }

		}


		private static Color GetColorFromString(string color, Brush defaultColor)
		{
			
			if (color == "Empty")
			{
				color = "White";
			}

			return (Color)colorConverter.ConvertFromString(color);
		}


        private void SendTheMessageToRichTextBox(string logMessage, WpfRichTextBoxRowColoringRule rule)
        {
            System.Windows.Controls.RichTextBox rtbx = TargetRichTextBox;

            var tr = new TextRange(rtbx.Document.ContentEnd, rtbx.Document.ContentEnd);
            tr.Text = logMessage + "\n";
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, 
                new SolidColorBrush(GetColorFromString(rule.FontColor, (Brush)tr.GetPropertyValue(TextElement.ForegroundProperty)))
            );
            tr.ApplyPropertyValue(TextElement.BackgroundProperty,
                new SolidColorBrush(GetColorFromString(rule.BackgroundColor, (Brush)tr.GetPropertyValue(TextElement.BackgroundProperty)))
            );
            tr.ApplyPropertyValue(TextElement.FontStyleProperty, rule.Style);
            tr.ApplyPropertyValue(TextElement.FontWeightProperty, rule.Weight);


            if (this.MaxLines > 0)
            {
                this.lineCount++;
                if (this.lineCount > MaxLines)
                {
                    tr = new TextRange(rtbx.Document.ContentStart, rtbx.Document.ContentEnd);
                    tr.Text.Remove(0, tr.Text.IndexOf('\n'));
                    this.lineCount--;
                }
            }

            if (this.AutoScroll)
            {
                rtbx.ScrollToEnd();
            }
        }
    }
}
#endif