﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ProkingNet.NET;
using System.IO.Ports;
using Salary;

namespace LGNetWorkSystem
{
    public partial class FrmMain : Office2007Form
    {
        ProkingNet.NET.Client client = new ProkingNet.NET.Client();
        public event EventHandler clientReturnValue;
        NetData netdata = new NetData();
        int icount = 0;
        SerialPort sendPort;
        SerialPort serialPortGet;
        SerialPort serialReadPort;
       // public event EventHandler SerialPortScannerHandler;
        public event EventHandler SerialPortSend;
        public event EventHandler SerialReadPortHandler;
        public FrmMain()
        {
            InitializeComponent();
            clientReturnValue += new EventHandler(Form1_clientReturnValue);
            //SerialPortScannerHandler += new EventHandler(FrmMain_SerialPortScannerHandler);
            SerialPortSend += new EventHandler(FrmMain_SerialPortSend);
            SerialReadPortHandler += new EventHandler(FrmMain_SerialReadPortHandler);
        }
        //获取数据
        void FrmMain_SerialReadPortHandler(object sender, EventArgs e)
        {
            if (!checkBoxX1.Checked)
            {
                string value = Encoding.Default.GetString((byte[])sender);

                if (value.Substring(0, AppData.appDataSington().BarcodeRule.Length) == AppData.appDataSington().BarcodeRule)
                {
                    SendKeys.Send(Encoding.Default.GetString((byte[])sender) + "\n");
                    icount = icount + 1;
                    labelX10.Text = icount.ToString();
                    if (icount == 50)
                    {
                        // icount = 0;
                        // labelX10.Text = icount.ToString();
                        //发送命令到逄工的控制盒
                        string newCommand = command.Replace(" ", "");
                        byte[] commands = commandBytes(newCommand);
                        sendPort.Write(commands, 0, commands.Length);
                        //WavPlayer.Play(Application.StartupPath + "\\9.wav");
                        //System.Threading.Thread.Sleep(10);
                        //WavPlayer.Play(Application.StartupPath + "\\9.wav");
                    }
                }
                else
                {
                    if (value == "no read")
                    {
                        //WavPlayer.Play(Application.StartupPath + "\\7.wav");
                        //System.Threading.Thread.Sleep(10);
                        //WavPlayer.Play(Application.StartupPath + "\\7.wav");
                        string newCommand = command.Replace(" ", "");
                        byte[] commands = commandBytes(newCommand);
                        sendPort.Write(commands, 0, commands.Length);
                    }

                }


            }
        }

        /// <summary>
        /// 连接到扫描读头操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(textBoxX4.Text.Trim()))
            {
                MessageBoxEx.Show("端口必须填写");
            }
            serialReadPort = serialPortGet = new SerialPort(textBoxX4.Text.ToUpper(), 9600, Parity.None, 8, StopBits.One);
            serialReadPort.DataReceived += new SerialDataReceivedEventHandler(serialReadPort_DataReceived);
            try
            {
                serialReadPort.Open();
                lblSerialReadStatus.Text = "连接成功";
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("连接失败" + ex.Message);
                lblSerialReadStatus.Text = "连接失败";
                return;

            }
            
        }

        void serialReadPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(300);
            if (serialReadPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[serialReadPort.BytesToRead];
                serialReadPort.Read(buffer, 0, buffer.Length);
                this.Invoke(SerialReadPortHandler, buffer);
            }

        }

      



        void Form1_clientReturnValue(object sender, EventArgs e)
        {

            if (!checkBoxX1.Checked)
            {
                string value = Encoding.Default.GetString((byte[])sender);

                if (value.Substring(0, AppData.appDataSington().BarcodeRule.Length) == AppData.appDataSington().BarcodeRule)
                {
                    SendKeys.Send(Encoding.Default.GetString((byte[])sender) + "\n");
                    icount = icount + 1;
                    labelX10.Text = icount.ToString();
                    if (icount == 50)
                    {
                       // icount = 0;
                       // labelX10.Text = icount.ToString();
                        //发送命令到逄工的控制盒
                        string newCommand = command.Replace(" ", "");
                        byte[] commands = commandBytes(newCommand);
                        sendPort.Write(commands, 0, commands.Length);
                        //WavPlayer.Play(Application.StartupPath + "\\9.wav");
                        //System.Threading.Thread.Sleep(10);
                        //WavPlayer.Play(Application.StartupPath + "\\9.wav");
                    }
                }
                else
                {
                    if (value == "no read")
                    {
                        //WavPlayer.Play(Application.StartupPath + "\\7.wav");
                        //System.Threading.Thread.Sleep(10);
                        //WavPlayer.Play(Application.StartupPath + "\\7.wav");
                        string newCommand = command.Replace(" ", "");
                        byte[] commands = commandBytes(newCommand);
                        sendPort.Write(commands, 0, commands.Length);
                    }

                }


            }
        }
        /// <summary>
        /// 用于显示是否连接到扫描器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        string command = "5a 03 01 a5";
        private byte[] commandBytes(string command)
        {

            string sj = command;
            byte[] returnBytes = new byte[sj.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(sj.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 监听端口打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {

            //load的时候 将数据显示出来
            //2015-09-22 修改 增加扫描枪端口和增加确认按钮接口
            serialPortGet = new SerialPort("COM10", 9600, Parity.None, 8, StopBits.One);
            try
            {
              
                textBoxX3.Text = XMLConfig.ConfigFileOperate.GetConfigValue(Application.StartupPath + "\\AppConfig.config", "LGConnect", "LGCOM");
                textBoxX4.Text = XMLConfig.ConfigFileOperate.GetConfigValue(Application.StartupPath + "\\AppConfig.config", "LGConnect", "LGSerialRead");
                serialPortGet.Open();
                serialPortGet.DataReceived += new SerialDataReceivedEventHandler(serialPortGet_DataReceived);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return;

            }
        }
        /// <summary>
        /// 获取抓屏数据 用来停止端口操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serialPortGet_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //label1.Text = "抓屏减1";
            serialPortGet.DiscardInBuffer();
            string newCommand = command.Replace(" ", "");
            byte[] commands = commandBytes(newCommand);
            sendPort.Write(commands, 0, commands.Length);
            WavPlayer.Play(Application.StartupPath + "\\7.wav");
            System.Threading.Thread.Sleep(10);
            WavPlayer.Play(Application.StartupPath + "\\7.wav");
            //同时要将数据删除掉
            if (icount >= 1)
            {
                icount = icount - 1;
                labelX10.Text = icount.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newCommand = command.Replace(" ", "");
            byte[] commands = commandBytes(newCommand);
            sendPort.Write(commands, 0, commands.Length);
        }
        /// <summary>
        /// 连接控制盒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxX3.Text.Trim()))
            {
                MessageBoxEx.Show("端口必须填写");
            }
            sendPort = new SerialPort(textBoxX3.Text.Trim(), 9600, Parity.None, 8, StopBits.One);
            sendPort.DataReceived += new SerialDataReceivedEventHandler(sendPort_DataReceived);
            try
            {
                sendPort.Open();
                lblControlPort.Text = "连接成功";
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("连接失败" + ex.Message);
                lblControlPort.Text = "连接失败";
                return;

            }

        }
        //接收数据
        void sendPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(300);
            if (sendPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[sendPort.BytesToRead];
                sendPort.Read(buffer, 0, buffer.Length);
                this.Invoke(SerialPortSend, buffer);
            }

        }
        //增加控制盒接收端口
        void FrmMain_SerialPortSend(object sender, EventArgs e)
        {
           
            string value = Encoding.Default.GetString(changeToString((byte[])sender));
            //MessageBox.Show(value);
            if (value == "A501FE5A")
            {
                icount = 0;
                labelX10.Text = icount.ToString();
                SendKeys.Send("{F8}");
            }

        }

        //十六进制数据转化
        List<byte> values = new List<byte>();
        private byte[] changeToString(byte[] valus)
        {
            values.Clear();
            for (int i = 0; i < valus.Length; i++)
            {
                byte tmp = Convert.ToByte(valus[i] / 0x10);
                if (tmp >= 0 && tmp <= 9)
                {
                    tmp += 0x30;
                }
                else
                {
                    tmp += 0x37;
                }
                values.Add(tmp);
                byte tmp2 = Convert.ToByte(valus[i] & 0x0F);
                if (tmp2 >= 0 && tmp2 <= 9)
                {
                    tmp2 += 0x30;
                }
                else
                {
                    tmp2 += 0x37;
                }
                values.Add(tmp2);
            }
            return values.ToArray();
        }


       
        void FrmMain_SerialPortScannerHandler(object sender, EventArgs e)
        {
            byte[] values = (byte[])sender;
            string data = Encoding.Default.GetString(values);
            //MessageBox.Show("扫描到的数据的位数是"+data.Length);
            //2015-10-12 修改 不等于14位和不等于15位的时侯
            if (data.Length == 16 || data.Length == 17)
            {
                SendKeys.Send(data + "\n");
                if (icount > 0)
                {
                    //label1.Text = "扫描枪减1"+DateTime.Now.ToString();
                    //icount = icount - 1;
                    labelX10.Text = icount.ToString();
                }
            }
            else
            {
                //SendKeys.Send(data+"\n");
            }

        }
  
     
        void FrmMain_SerialPortButtonHandler(object sender, EventArgs e)
        {
            byte[] values = (byte[])sender;
            string data = Encoding.Default.GetString(values);
            if (data == "A5 02 FD 5A")
            {
                SendKeys.Send("{F8}");
            }
            else
            {
                SendKeys.Send("{F8}");
            }
        }
        /// <summary>
        /// 确认条码规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX3_Click(object sender, EventArgs e)
        {
            FrmBarcodeRule rule = new FrmBarcodeRule();
            rule.ShowDialog();
            rule.Dispose();
        }
        /// <summary>
        /// 连接端口确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX4_Click(object sender, EventArgs e)
        {
            try
            {
               
                XMLConfig.ConfigFileOperate.SetConfigValue(Application.StartupPath + "\\AppConfig.config", "LGConnect", "LGCOM", textBoxX3.Text.Trim());
                XMLConfig.ConfigFileOperate.SetConfigValue(Application.StartupPath + "\\AppConfig.config", "LGConnect", "LGSerialRead", textBoxX4.Text.Trim());
                MessageBoxEx.Show("保存参数成功\r\n重新启动软件生效");

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("保存连接参数失败" + ex.Message);

            }

        }
        /// <summary>
        /// 清空扫描数量计数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX5_Click(object sender, EventArgs e)
        {
            icount = 0;
            labelX10.Text = icount.ToString();
        }
 
       
        void FrmMain_SerialPortLOTHandler(object sender, EventArgs e)
        {
            byte[] values = (byte[])sender;
            string data = Encoding.Default.GetString(values);
            //lot号+2个回车=10位
            if (data.Length == 10)
            {
                SendKeys.Send(data + "\n");
            }

        }
        

    }
}
