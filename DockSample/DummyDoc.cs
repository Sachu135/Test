using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using EasyScintilla.Stylers;
using ScintillaNET;
using System.Threading.Tasks;
using DockSample.lib;
using SFTPEntities;

namespace DockSample
{
    public partial class DummyDoc :  DockContent
    {
        public EasyScintilla.SimpleEditor TxtEditor { get; set; }
        public DummyDoc()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            
        }

        public DummyDoc(eFileType fileType)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            FileType = fileType;
            txtCodeEditor.Text = string.Empty;
            TxtEditor = txtCodeEditor;
        }
        public MainForm mainFrm { get; set; }
        
        public string FileName { get; set; }

        public DummyOutputWindow outputWindow { get; set; }
        public SFTPEntities.eFileType FileType { get; set; }
        public string FullPath { get; set; }
        
        // workaround of RichTextbox control's bug:
        // If load file before the control showed, all the text format will be lost
        // re-load the file after it get showed.
        private bool m_resetText = true;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_resetText)
            {
                m_resetText = false;
                FileName = FileName;
            }
        }

        //protected override string GetPersistString()
        //{
        //    // Add extra information into the persist string for this document
        //    // so that it is available when deserialized.
        //    return GetType().ToString() + "," + FileName + "," + Text;
        //}

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This is to demostrate menu item has been successfully merged into the main form. Form Text=" + Text);
        }

        private void menuItemCheckTest_Click(object sender, System.EventArgs e)
        {
            menuItemCheckTest.Checked = !menuItemCheckTest.Checked;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.txtCodeEditor.Text = Text;
            //if (FileName == string.Empty)
                
        }

        private void DummyDoc_Load(object sender, EventArgs e)
        {
            switch (FileType)
            {
                case SFTPEntities.eFileType.Python:
                    this.txtCodeEditor.Styler = new PythonStyler();
                    txtCodeEditor.Lexer = Lexer.Python;
                    break;
                case SFTPEntities.eFileType.Text:
                    break;
                case SFTPEntities.eFileType.Xml:
                    this.txtCodeEditor.Styler = new EasyScintilla.Stylers.HtmlStyler();
                    txtCodeEditor.Lexer = Lexer.Xml;
                    break;
                case SFTPEntities.eFileType.Json:
                    this.txtCodeEditor.Styler = new EasyScintilla.Stylers.JsonStyler();
                    txtCodeEditor.Lexer = Lexer.Json;
                    break;
                case SFTPEntities.eFileType.Csv:
                    
                    break;
                case SFTPEntities.eFileType.Excel:
                    
                    break;
                default:
                    
                    break;
            }
        }

        private async void txtCodeEditor_CharAdded(object sender, CharAddedEventArgs e)
        {
            Task t = new Task(() =>
            {
                int currentPos = 0, wordStartPos = 0;
                // Find the word start
                txtCodeEditor.PerformSafely(() =>
                {
                    if (txtCodeEditor.Lexer != Lexer.Python)
                    {
                        return;
                    }
                    currentPos = txtCodeEditor.CurrentPosition;
                    wordStartPos = txtCodeEditor.WordStartPosition(currentPos, true);
                });


                // Display the autocompletion list
                var lenEntered = currentPos - wordStartPos;
                if (lenEntered > 0)
                {
                    txtCodeEditor.PerformSafely(() =>
                    {
                        if (!txtCodeEditor.AutoCActive)
                            txtCodeEditor.AutoCShow(lenEntered, "and as assert break class continue def del elif else except finally for from global if import in is lambda nonlocal not or pass print raise return try while with yield False None True");
                    });
                }
            });
            t.Start();
            await t;
        }
    }
}