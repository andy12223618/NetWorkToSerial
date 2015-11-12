using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace LGNetWorkSystem
{
    public partial class FrmBarcodeRule : Office2007Form
    {
        public FrmBarcodeRule()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 保存条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxX3.Text))
            {
                return;
            }

            if (XMLConfig.ConfigFileOperate.SetConfigValue(Application.StartupPath + "\\AppConfig.config", "BarcodeRule", "rule", textBoxX3.Text.Trim()))
            {
                MessageBoxEx.Show("保存条码前缀成功");
                this.Close();
            }
        }

        private void FrmBarcodeRule_Load(object sender, EventArgs e)
        {
            textBoxX3.Text = XMLConfig.ConfigFileOperate.GetConfigValue(Application.StartupPath + "\\AppConfig.config", "BarcodeRule", "rule");
            
        }
        /// <summary>
        /// 此处确认的时候，用于什么时候 用于减1 什么时候用来将数据上传到LG原有的系统当中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
          
           
        }
    }
}
