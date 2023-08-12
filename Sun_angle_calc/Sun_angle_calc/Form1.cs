using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sun_angle_calc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int day = int.Parse(textBox1.Text);
            double hour = Double.Parse(textBox2.Text);



            //Calc(2023, 240, hour, 36.0428856334391, 139.67763891246423, out h, out a);
            Calc2(day,hour,35,135);


            //label2.Text = ("Height=" + h.ToString() + "  Angle=" + a.ToString());


        }


        public void Calc2(int day,double hour,double lat ,double lon)
        {

            double theta = 2 * Math.PI * (day+0.5) / 365;

            //太陽赤緯(°)
            double delta = 0.33281 - 22.984 * Math.Cos(theta) - 0.34990 * Math.Cos(2 * theta) - 0.13980 * Math.Cos(3 * theta) + 3.7872 * Math.Sin(theta) + 0.03250 * Math.Sin(2 * theta) + 0.07187 * Math.Sin(3 * theta);

            //均時差(hour)
            double e = 0.0072 * Math.Cos(theta) - 0.0528 * Math.Cos(2 * theta) - 0.0012 * Math.Cos(3 * theta) - 0.1229 * Math.Sin(theta) - 0.1565 * Math.Sin(2 * theta) - 0.0041 * Math.Sin(3 * theta);

            //e = 0;

            //時角(°)
            double t = hour + (lon - 135) / 15 + e;
            t = 15 * t - 180;

            double latRad = lat * Math.PI / 180;
            double deltaRad= delta * Math.PI / 180;
            double tRad= t * Math.PI / 180;

            double h = Math.Asin(Sin(latRad) * Sin(deltaRad) + Cos(latRad) * Cos(deltaRad) * Cos(tRad));
            h = h * 180 / Math.PI;


            label2.Text = h.ToString();


        }

            public void Calc(int year, int day, double hour, double lat, double lon, out double h, out double a)
        {
            double latRad = lat * Math.PI / 180;

            // 赤緯と均時差の計算
            int n = year - 1968;
            double d0 = 3.71f + 0.2596f * n - (int)((n + 3) / 4);
            double M_deg = 0.9856f * (day - d0);
            double M = M_deg * Math.PI / 180;
            double eps_deg = 12.3901f + 0.0172f * (n + M_deg / 360f);
            double eps = eps_deg * Math.PI / 180;
            double v_deg = M_deg + 1.914f * Sin(M) + 0.02f * Sin(2f * M);
            double v = v_deg * Math.PI / 180;
            double Et1 = M - v;
            double Et2 = Math.Atan2((0.043f * Sin(2 * (v + eps))), (1f - 0.043f * Cos(2 * (v + eps))));

            // 均時差
            double Et = Et1 - Et2;

            const double delta0 = -23.4393f * Math.PI / 180;

            // 太陽の視赤緯
            double delta = Math.Asin(Cos(v + eps) * Sin(delta0));

            // 時角
            double t = (15 * (hour - 12) + (lon - 135)) * Math.PI / 180 + Et;

            // 太陽高度角
            h = Math.Asin(Sin(latRad) * Sin(delta) + Cos(latRad) * Cos(delta) * Cos(t));
            h= h*180/Math.PI;

            double sinA = (Cos(delta) * Sin(t)) / Cos(h);
            double cosA = (Sin(h) * Sin(latRad) - Sin(delta)) / (Cos(h) * Cos(latRad));

            // 太陽方位角 南が0度になるようにPIを足す
            a = Math.Atan2(sinA, cosA) + Math.PI;
            a = a * 180 / Math.PI;

        }

        private double Cos(double theta)
        {
            return Math.Cos(theta);
        }

        private double Sin(double theta)
        {
            return Math.Sin(theta);
        }

    }
}
