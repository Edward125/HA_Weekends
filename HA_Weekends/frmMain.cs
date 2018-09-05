using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace HA_Weekends
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        string url = @"http://api.goseek.cn/Tools/holiday?date=";

        private void frmMain_Load(object sender, EventArgs e)
        {

        }


        



        /// <summary>
        /// 获得今年有几周
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetWeekOfYear(int year)
        {
            DateTime the_Date = new DateTime(year, 1, 1);//本年的第一天
            TimeSpan tt = the_Date.AddYears(1) - the_Date;//求出本年有几天
            return tt.Days / 7 + 1; //因为年只有366天和365天除以7有余数所以始终需要加一周
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            StreamReader reader = null;
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
               // WriteLog.CreateExceptionLog(ex);
                return reader.ReadToEnd();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public class HolidayDate
        {
            public int Code { set; get; }
            public int Date { set; get; }
        }


        /// <summary>
        /// 
        /// </summary>
        public enum DateType
        {
            WORKDATE =0,//工作日
            WEEKEND = 1,//周末休息日
            HOLIDAY = 2//法定假日(国假类)

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdate"> 日期的格式,20180901</param>
        /// <returns></returns>
        private DateType  GetDateType(string sdate)
        {
            DateType dt = new DateType();
            string response = HttpGet(url + sdate);
            HolidayDate holidaydate = JsonConvert.DeserializeObject<HolidayDate>(response);
            dt = (DateType)holidaydate.Date;
            return dt;
        }





    


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string _sdate = dateTimePicker1.Value.ToString("yyyyMMdd");

            //MessageBox.Show(GetDateType(_sdate).ToString());

            
        }
    }
}
