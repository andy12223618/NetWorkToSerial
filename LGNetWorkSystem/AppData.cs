using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace Salary
{
    /// <summary>
    /// 用于数据流转
    /// </summary>
    class AppData
    {

        private static AppData appdata;
        public static AppData appDataSington()
        {
            if (appdata == null)
            {
                appdata = new AppData();
            }
            return appdata;

        }

        //鱼的状态标识 用于标识鱼是否正在使用
        public enum FishStatus : int
        {
            FishIsUse = 1,
            FishNotUse
        }

        public enum ChangeFunction : int
        {
            NoPriceChange = 1,
            PriceChange
        }
        //测试可以使用中文
        //有关工人的使用
        public enum WorkerStatus : int
        {
            //账号状态,
            //账号可用
            WorkerStatusIsUse = 1,
            WorkerStatusNotUSe
        }
        //系统用户状态
        public enum UserStatus : int
        {
            UserIS,
            UserISNot
        }

        //系统制定的部门的状态
        //其中如果用户已经被使用 则用户不能够被删除
        public enum DepartMentStatus : int
        {
            DepartMentISUse = 1,
            DepartMentISNot

        }
        //工序（包括零工的状态）
        public enum JobStatus : int
        {
            JobIsUse = 1,
            JobIsNot

        }
        //是否是零工
        public enum JobSpec : int
        {
            NoPartTime = 1,
            PartTime//这个是零工标识

        }
        //零工属性
        //1是计时
        //2是计重
        //3是计件
        //4是固定工资
        public enum PartJobAttribute : int
        {
            byTime = 1,
            byWeight,
            byCount,
            byEqual

        }
        public string[] UserRights = new string[] { "统计工资", "修改流水", "报表查询", "管理员", "出入库权限" };
        private string _UserSpecialRight = "";

        public string UserSpecialRight
        {
            get { return _UserSpecialRight; }
            set { _UserSpecialRight = value; }
        }

        string xmlConfigPath = Application.StartupPath + "\\AppConfig.config";
        private string _DBAddress = "";//返回数据库连接的相关的信息
        private string _DBName = "";
        private string _UserID = "";
        private string _Password = "";
        private string _DBNameKIS = "";
        private string _UserIDKIS = "";
        private string _PasswordKIS = "";

        public string PasswordKIS
        {
            get
            {

                _PasswordKIS = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLZTSettings", "PasswordZT");
                return _PasswordKIS;

            }

        }


        public string UserIDKIS
        {
            get
            {

                _UserIDKIS = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLZTSettings", "UserNameZT");

                return _UserIDKIS;
            }

        }



        public string DBNameKIS
        {
            get
            {

                _DBNameKIS = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLZTSettings", "CatalogZT");

                return _DBNameKIS;
            }

        }

        public string XMLConfigPath
        {
            get
            {
                return xmlConfigPath;
            }
        }

        private string _SystemPicPath;

        public string SystemPicPath
        {
            get { return XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SystemSetting", "SystemPicPath"); }
            private set { _SystemPicPath = value; }
        }


        public string DBAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_DBAddress))
                {
                    _DBAddress = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLSettings", "DataBase");
                }
                return _DBAddress;
            }

        }

        public string DBName
        {
            get
            {
                if (string.IsNullOrEmpty(_DBName))
                {
                    _DBName = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLSettings", "Catalog");
                }
                return _DBName;
            }

        }

        public string UserID
        {
            get
            {
                if (string.IsNullOrEmpty(_UserID))
                {
                    _UserID = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLSettings", "UserName");
                }
                return _UserID;
            }

        }
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_Password))
                {
                    _Password = XMLConfig.ConfigFileOperate.GetConfigValue(xmlConfigPath, "SQLSettings", "Password");
                }
                return _Password;
            }

        }
        /// <summary>
        /// 用以返回连接的字符串
        /// </summary>
        // private string _ConnectionString = "";
        public string ConnecionString
        {
            get
            {
                return "Data Source='" + DBAddress + "';Initial Catalog='"+DBName+"';User ID='" + UserID + "';Password='" + Password + "'";
            }
        }
        /// <summary>
        /// 获取全部的账套
        /// </summary>
        public string ConnectionStringZT
        {
            get
            {
                return "Data Source='" + DBAddress + "';Initial Catalog='Master';User ID='" + UserID + "';Password='" + Password + "'";
            }
        }
        /// <summary>
        ///  获取金蝶的账套
        /// </summary>
        public string ConnectionStringLKIS
        {
            get
            {
                return "Data Source='" + DBAddress + "';Initial Catalog='" + DBNameKIS + "';User ID='" + UserIDKIS + "';Password='" + PasswordKIS + "'";
            }
        }

        private DevComponents.DotNetBar.SuperTabControl _TabControl;

        public DevComponents.DotNetBar.SuperTabControl TabControl
        {
            get { return _TabControl; }
            set { _TabControl = value; }
        }
        //方法 删除tabcontrol数据
        public void deleteTabItem(Form frm)
        {
            for (int i = 0; i < AppData.appDataSington()._TabControl.Tabs.Count; i++)
            {
                if (_TabControl.Tabs[i].Tag == frm)
                {
                    _TabControl.Tabs.Remove(_TabControl.Tabs[i]);
                    break;
                }

            }

            if (_TabControl.Tabs.Count == 0)
            {
                _TabControl.Visible = false;
            }
        }


        public string BarcodeRule
        {
            get
            {
                return XMLConfig.ConfigFileOperate.GetConfigValue(Application.StartupPath + "\\AppConfig.config", "BarcodeRule","rule").ToString();
            }
        }
        //条码位数确认
        public string BarcodeNO
        {
            get
            {
                return XMLConfig.ConfigFileOperate.GetConfigValue(Application.StartupPath + "\\AppConfig.config", "BarcodeRule", "barcodeNo").ToString();
            }
        }




    }
}
