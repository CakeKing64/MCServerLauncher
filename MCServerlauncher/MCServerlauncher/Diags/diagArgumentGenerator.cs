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

            string arg_min_mem = "1024";
            string arg_min_mem_type = "MB";
            string arg_max_mem = "1024";
            string arg_max_mem_type = "MB";
            bool useGUI = true;
            foreach(string arg in args_s)
            {
                if (arg.Length > 4)
                    if (arg.Substring(0, 4) == "-Xmx")
                    {
                        arg_max_mem = arg.Substring(4, arg.Length - 5);
                        arg_max_mem_type = (arg[arg.Length - 1] == 'm' ? "MB" : "GB");
                    }
                if (arg.Length > 4)
                    if (arg.Substring(0,4) == "-Xms")
                {
                        arg_min_mem = arg.Substring(4, arg.Length - 5);
                        arg_min_mem_type = (arg[arg.Length - 1] == 'm' ? "MB" : "GB");
                    
                }
                if (arg == "nogui")
                    useGUI = false;
            }
            chkUseGUI.Checked = useGUI;
            tbMaxMemory.Text = arg_max_mem;
            tbMinMemory.Text = arg_min_mem;

            cmbMaxTYPE.Text = arg_max_mem_type;
            cmbMinTYPE.Text = arg_min_mem_type;

        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            _args = "-Xmx" + tbMaxMemory.Text + (cmbMaxTYPE.Text.ToLower() == "mb" ? "m" : cmbMaxTYPE.Text.ToLower() == "gb" ? "g" : "m")
                + " -Xms" + tbMinMemory.Text + (cmbMinTYPE.Text.ToLower() == "mb" ? "m" : cmbMinTYPE.Text.ToLower() == "gb" ? "g" : "m")
                + " -jar server.jar "
                + (chkUseGUI.Checked ? "" : "nogui");
            /*
                + " -o "
                + (chkOnlineMode.Checked ? "true" : "false");
                */
            Close();
        }

        private void diagArgumentGenerator_Load(object sender, EventArgs e)
        {
            load_args(_args);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _args = "CLOSED";
            this.Close();
        }
    }
}
