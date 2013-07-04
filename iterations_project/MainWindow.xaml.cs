using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iterations_project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //считываем А
            string[] sarrn;
            sarrn = boxA.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[][] sarr = new string[sarrn.Length][];
            for (int i = 0; i < sarrn.Length; i++)
            {
                sarrn[i] = sarrn[i].Trim(new char[] { '\r' });
                sarr[i] = sarrn[i].Split(new char[] { ' ', '\t' });

            }

            double[][] A = new double[sarr.Length][];
            for (int i = 0; i < sarr.Length; i++)
            {
                A[i] = new double[sarr[0].Length];
                for (int j = 0; j < sarr[0].Length; j++)
                {

                    A[i][j] = Convert.ToDouble(sarr[i][j]);
                }

            }
            //считываем B
            string sbstr = boxB.Text;
            string[][] bstr = new string[1][];
            bstr[0] = sbstr.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            double[][] B = MatrixCreate(1, bstr[0].Length); ;

            for (int i = 0; i < B[0].Length; i++)
            {

                B[0][i] = Convert.ToDouble(bstr[0][i]);
            }
            double E = Convert.ToDouble(boxE.Text);
            double[][] X = iterations(A, B, E);

            boxX.Text="";
            for (int i = 0; i < X[0].Length; i++)
            {
                boxX.Text += Convert.ToString(X[0][i]);
                boxX.Text += "\n";
            }

        }


        static double[][] copy(double[][] source) // копирование массива в массив
        {
            double[][] a = MatrixCreate(source.Length, source[0].Length);

            for (int i = 0; i < source.Length; i++)
            {
                for (int j = 0; j < source[0].Length; j++)
                {
                    a[i][j] = source[i][j];
                }

            }

            return a;
        }
        static double[] copy(double[] source)// копирование строки в строку
        {
            double[] a = new double[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                a[i] = source[i];

            }

            return a;
        }

        static double[][] MatrixCreate(int rows, int cols)// создание пустой матрицы заданного размера
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        

        static double[][] iterations(double[][] a, double[][] b, double e)//основная функция
        //iterations(A,B,E) 
        // А - заданная матрица
        // В - столбец свободных членов
        // Е - требуемая точность
        // double[3] x = iterations(a,b,e);
        {

            double[][] alpha = MatrixCreate(a.Length, a[0].Length);
            double[][] betha = MatrixCreate(b.Length, b[0].Length);

            alpha = copy(a);
            betha = copy(b);

            for (int i = 0; i < a.Length; i++)
            {
                double elem = alpha[i][i];
                for (int j = 0; j < a[0].Length; j++)
                {
                    alpha[i][j] = alpha[i][j] / elem;

                }
                betha[0][i] = betha[0][i] / elem;
                alpha[i][i] = 0;


            }


            double[][] xold = new double[1][];
            double[][] xnew = new double[1][];
            xold = copy(betha);


            bool flagend = false;

            while (!flagend)
            {

                double[][] xv = new double[1][];
                xv[0] = absminus(xold[0], iterate(alpha, xold[0], betha));
                //DEBUGOUT(xold);
                flagend = true;
                foreach (double value in xv[0])
                {
                    if (value > e)
                    {
                        flagend = false;
                    }
                }
                xold[0] = iterate(alpha, xold[0], betha);
            }

            return xold;
        }
        static double[] iterate(double[][] a, double[] x, double[][] b)//вспомогательная функция. вычисляет значение Х для следующей итерации
        {
            double[] nx = copy(x);

            for (int i = 0; i < nx.Length; i++)
            {
                double s = 0;
                for (int j = 0; j < a[0].Length; j++)
                {
                    s += a[i][j] * nx[j];
                }
                nx[i] = -s + b[0][i];
            }


            return nx;
        }
        
        static double[] absminus(double[] a, double[] b)// высчитывает разницу - для проверки соответсятвия точности
        {
            double[] r = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = Math.Abs(a[i] - b[i]);

            }
            return r;
        }


    }
}
