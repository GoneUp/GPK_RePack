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

using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using NLog.Config;

#if !NET_CF && !MONO && !SILVERLIGHT

namespace GPK_RePack.Core.Helper
{
	[NLogConfigurationItem]
    public class WpfRichTextBoxWordColoringRule
    {
        private Regex compiledRegex;

        public WpfRichTextBoxWordColoringRule()
        {
            this.FontColor = "Empty";
            this.BackgroundColor = "Empty";
        }

        public WpfRichTextBoxWordColoringRule(string text, string fontColor, string backgroundColor)
        {
            this.Text = text;
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
            this.Style = FontStyles.Normal;
            this.Weight = FontWeights.Normal;
        }

        public WpfRichTextBoxWordColoringRule(string text, string textColor, string backgroundColor, FontStyle fontStyle, FontWeight fontWeight)
        {
            this.Text = text;
            this.FontColor = textColor;
            this.BackgroundColor = backgroundColor;
            this.Style = fontStyle;
            this.Weight = fontWeight;
        }

        public string Regex { get; set; }

        public string Text { get; set; }

        [DefaultValue(false)]
        public bool WholeWords { get; set; }

        [DefaultValue(false)]
        public bool IgnoreCase { get; set; }

        public FontStyle Style { get; set; }

        public FontWeight Weight { get; set; }

        public Regex CompiledRegex
        {
            get
            {
                if (this.compiledRegex == null)
                {
                    string regexpression = this.Regex;
                    if (regexpression == null && this.Text != null)
                    {
                        regexpression = System.Text.RegularExpressions.Regex.Escape(this.Text);
                        if (this.WholeWords)
                        {
                            regexpression = "\b" + regexpression + "\b";
                        }
                    }

                    RegexOptions regexOptions = RegexOptions.Compiled;
                    if (this.IgnoreCase)
                    {
                        regexOptions |= RegexOptions.IgnoreCase;
                    }

                    this.compiledRegex = new Regex(regexpression, regexOptions);
                }

                return this.compiledRegex;
            }
        }

        [DefaultValue("Empty")]
        public string FontColor { get; set; }

        [DefaultValue("Empty")]
        public string BackgroundColor { get; set; }
    }
}
#endif