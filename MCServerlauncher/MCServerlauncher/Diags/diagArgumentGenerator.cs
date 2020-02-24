using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCServerLauncher.Diags
{
    public partial class diagArgumentGenerator : Form
    {

        List<string> ArgsOther = new List<string>();
        public diagArgumentGenerator()
        {
            InitializeComponent();
        }
        public diagArgumentGenerator(string args)
        {
            _args = args;
            InitializeComponent();
        }

        public string _args = "";
        public string _memory_min = "1024";
        public string _memory_max = "1024";
        public string _memory_min_mode = "MB";
        public string _memory_max_mode = "MB";
        
        private void load_args(string args)
        {
           string[] args_s = args.Split(' ');
            ArgsOther.Clear();
            string arg_min_mem = "1024";
            string arg_min_mem_type = "MB";
            string arg_max_mem = "1024";
            string arg_max_mem_type = "MB";
            bool useGUI = true;
            bool x64 = false;
            foreach(string arg in args_s)
            {

                bool bFoundArg = false;

                if (arg.Length > 4)
                    if (arg.Substring(0, 4) == "-Xmx")
                    {
                        arg_max_mem = arg.Substring(4, arg.Length - 5);
                        arg_max_mem_type = (arg[arg.Length - 1] == 'm' ? "MB" : "GB");
                        bFoundArg = true;
                    }
                if (arg.Length > 4)
                    if (arg.Substring(0,4) == "-Xms")
                {
                        arg_min_mem = arg.Substring(4, arg.Length - 5);
                        arg_min_mem_type = (arg[arg.Length - 1] == 'm' ? "MB" : "GB");
                        bFoundArg = true;


                }
                if (arg == "nogui")
                {
                    useGUI = false;
                    bFoundArg = true;
                }
                if(arg == "-d64")
                {
                    x64 = true;
                    bFoundArg = true;
                }

                if(!bFoundArg)
                    ArgsOther.Add(arg);
            }
            chkUseGUI.Checked = useGUI;
            tbMaxMemory.Text = arg_max_mem;
            tbMinMemory.Text = arg_min_mem;

            cmbMaxTYPE.Text = arg_max_mem_type;
            cmbMinTYPE.Text = arg_min_mem_type;
            chkX64.Checked = x64;

        }

        string generate_output()
        {

            string outp = "-Xmx" + tbMaxMemory.Text + (cmbMaxTYPE.Text.ToLower() == "mb" ? "m" : cmbMaxTYPE.Text.ToLower() == "gb" ? "g" : "m")
     + " -Xms" + tbMinMemory.Text + (cmbMinTYPE.Text.ToLower() == "mb" ? "m" : cmbMinTYPE.Text.ToLower() == "gb" ? "g" : "m")
     + (chkX64.Checked ? " -d64" : " ");
    

            foreach (string str in ArgsOther)
            {
                outp += " " + str;
            }
            outp += (chkUseGUI.Checked ? "" : " nogui");
            return outp;
        }
        private void btnDone_Click(object sender, EventArgs e)
        {

            _args = generate_output();
            Close();
        }

        private void diagArgumentGenerator_Load(object sender, EventArgs e)
        {
            chkX64.Enabled = Program.iJavaType == 64 ? true : false;
            load_args(_args);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _args = null;
            this.Close();
        }

        private void btnAdv_Click(object sender, EventArgs e)
        {
            diagArgs da = new diagArgs(generate_output());
            da.ShowDialog();
            if (da._args != null)
                load_args(da._args);
            da.Dispose();
        }
    }
}
