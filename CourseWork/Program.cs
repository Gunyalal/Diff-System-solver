using System;
using DiffurSys;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Globalization;

namespace CourseWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Process plotProcess = new Process();
            int i,n=10; // n - количество "шагов"
            double t0=0, t1=1; // рассматриваемый отрезок
            double h=(t1-t0)/n; // размер "шага"
            double[] x = new double[n+1];
            double[] y = new double[n+1];
            double[] t = new double[n+1];
            t[0] = t0;
            t[n - 1] = t1;
            x[0] = 0;
            y[0] = -1;  
            
            Console.WriteLine("i   ti   xi   yi\n0   " + t[0] + "    "  + x[0] +"    " + y[0]);
            for (i = 1; i < n+1; i++)
            {
                t[i] = t[0] + h * i;
                x[i] = x[i - 1] + h * Func.func1(x[i - 1], y[i - 1], t[i - 1]); 
                y[i] = y[i - 1] + h * Func.func2(x[i - 1], y[i - 1], t[i - 1]); 


                Console.WriteLine(i + "   " + t[i] + "  " + x[i] + "    " + y[i]);

            }
            double maxXY = Math.Abs(Math.Max(Math.Max(Math.Abs(x.Max()),Math.Abs(x.Min())), Math.Max(Math.Abs(y.Max()), Math.Abs(y.Min())))); //Вычисление необходимого масштаба графика
            //double minXY = Math.Min(x.Min(), y.Min());
            string StringMaxXY = maxXY.ToString().Replace(',', '.');
            

            //Gnuplot
            plotProcess.StartInfo.FileName = @"G:\Programs\gnuplot\bin\gnuplot.exe";
            plotProcess.StartInfo.UseShellExecute = false;
            plotProcess.StartInfo.RedirectStandardInput = true;
            plotProcess.Start();
            StreamWriter sw = plotProcess.StandardInput;
            sw.WriteLine("set yrange[" + "-" + StringMaxXY + "- 1" + ":" + StringMaxXY + "+1" + "]");
            sw.WriteLine("set xrange[" + t0 + ":" + t1 + "]");
            string[] stringT = new string[n + 1];
            string[] stringX = new string[n + 1];
            string[] stringY = new string[n + 1];
            string strInputText1;
            string strInputText2;

            //Замена запятой в Double на точку и конвертирование в string
            for (i=0;i<n+1; i++)
            {
                stringT[i] = Convert.ToString(t[i]);
                stringT[i] = stringT[i].Replace(',', '.');

                stringX[i] = Convert.ToString(x[i]);
                stringX[i] = stringX[i].Replace(',', '.');

                stringY[i] = Convert.ToString(y[i]);
                stringY[i] = stringY[i].Replace(',', '.');
            }
            
            for (i = 0; i < n ; i++)
            {


                //int ii = i - 1;
                strInputText1 = "set arrow from " + stringT[i] + " ," + stringX[i] + " to " + stringT[i + 1] + " ," + stringX[i+1] + " nohead lc rgb 'red'";
                strInputText2 = "set arrow from " + stringT[i] + " ," + stringY[i] + " to " + stringT[i + 1] + " ," + stringY[i + 1] + " nohead lc rgb 'blue'";

                sw.WriteLine(strInputText1);
                sw.WriteLine(strInputText2);
            }
            sw.WriteLine("set key left bottom Left title 'Legend' box 3");
            sw.WriteLine("plot 1/0 lc rgb 'red' t " + '\u0022' + "x(t)" + '\u0022' + ", 1 / 0 lc rgb 'blue' t " + '\u0022' + "y(t)" + '\u0022' );
            //sw.WriteLine("plot 1/0 lc rgb 'blue' t " + '\u0022' + "y(t)" + '\u0022');

            Console.ReadKey();
                

        }
    }
}
