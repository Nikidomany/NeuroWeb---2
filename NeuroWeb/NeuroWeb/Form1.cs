using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuroWeb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public double[][] w = new double[2][];      // weights
        public double[][] a = new double[3][];      // neurons's activations
        public double[][] delta = new double[3][];   // neurons's delta
        public double[][] GRAD = new double[2][];     // weights gradient
        public double[][] deltaW = new double[2][];    // weights delta
        public double y;                            // desired result
        public double error;                         // error value
        public double[] errorArray;                  // array of every error value
        public int[][] inputCombinations = new int[4][];           // array of input combinations
        public int inputCombinationsCounter = 1;       // iteration of chahging input combinations
        public int iterationCounter = 0;                // iteration of back error propagation
        public double[][][] wLog = new double[2][][];    //weight change array (logs)

        public int xScale;         // graphic scale along the X axis
        public int yScale;         // graphic scale along the Y axis

        public double epsilon;            // learning rate
        public double alpha;              // moment

        public double Sigmoid (double param)
        {
          double result = 1 / ( Math.Exp(-param) +1);
            return result;
        }  
        
        public void InputCombination ()
        {

            if (checkBox00.Checked)
            {
                a[0][0] = 0;
                a[0][1] = 0;
                y = 0;

                textBox.Text += $"\r\n\r\n" +
                         $"Input neurons activations:\r\n" +
                         $"a(00)={Convert.ToString(a[0][0])}  " +
                         $"a(01)={Convert.ToString(a[0][1])}\r\n" +
                         $"Desire output activation: \r\n" +
                         $"y = {y} \r\n";
                ActivationsComputing();
            }
            if (checkBox01.Checked)
            {
                a[0][0] = 0;
                a[0][1] = 1;
                y = 1;

                textBox.Text += $"\r\n\r\n" +
                         $"Input neurons activations:\r\n" +
                         $"a(00)={Convert.ToString(a[0][0])}  " +
                         $"a(01)={Convert.ToString(a[0][1])}\r\n" +
                         $"Desire output activation: \r\n" +
                         $"y = {y} \r\n";
                ActivationsComputing();
            }
            if (checkBox10.Checked)
            {
                a[0][0] = 1;
                a[0][1] = 0;
                y = 1;

                textBox.Text += $"\r\n\r\n" +
                         $"Input neurons activations:\r\n" +
                         $"a(00)={Convert.ToString(a[0][0])}  " +
                         $"a(01)={Convert.ToString(a[0][1])}\r\n" +
                         $"Desire output activation: \r\n" +
                         $"y = {y} \r\n";
                ActivationsComputing();
            }
            if (checkBox11.Checked)
            {
                a[0][0] = 1;
                a[0][1] = 1;
                y = 0;

                textBox.Text += $"\r\n\r\n" +
                         $"Input neurons activations:\r\n" +
                         $"a(00)={Convert.ToString(a[0][0])}  " +
                         $"a(01)={Convert.ToString(a[0][1])}\r\n" +
                         $"Desire output activation: \r\n" +
                         $"y = {y} \r\n";
                ActivationsComputing();
            }
            //a[0][0] = inputCombinations[inputCombinationsCounter - 1][ 0];
            //a[0][1] = inputCombinations[inputCombinationsCounter - 1][ 1];  // assigning input neuron activations

            //switch (inputCombinationsCounter)
            //{
            //    case 1:
            //        y = 0;
            //        inputCombinationsCounter++;
            //        break;
            //    case 2:
            //        y = 1;
            //        inputCombinationsCounter++;
            //        break;
            //    case 3:
            //        y = 1;
            //        inputCombinationsCounter++;
            //        break;
            //    case 4:
            //        y = 0;
            //        inputCombinationsCounter = 1;
            //        break;
            //}

            //textBox.Text += $"\r\n\r\n" +
            //              $"Input neurons activations:\r\n" +
            //              $"a(00)={Convert.ToString(a[0][0])}  " +
            //              $"a(01)={Convert.ToString(a[0][1])}\r\n" +
            //              $"Desire output activation: \r\n" +
            //              $"y = {y} \r\n" ;

            //ActivationsComputing();
        }

        public void ArraysInitialization()
        {
            a[0] = new double[2];
            a[1] = new double[2];
            a[2] = new double[1];

            w[0] = new double[4];
            w[1] = new double[2];

            delta[0] = new double[0];
            delta[1] = new double[2];
            delta[2] = new double[1];

            GRAD[0] = new double[4];
            GRAD[1] = new double[2];

            deltaW[0] = new double[4];
            deltaW[1] = new double[2];

            wLog[0] = new double[4][];
            wLog[1] = new double[2][];

            errorArray = new double[1000];

            inputCombinations[0] = new int[2] { 0, 0 };
            inputCombinations[1] = new int[2] { 0, 1 };
            inputCombinations[2] = new int[2] { 1, 0 };
            inputCombinations[3] = new int[2] { 1, 1 };




            for (int i = 0; i < deltaW[0].Length; i++)
            {
                deltaW[0][i] = 0;
                wLog[0][i] = new double[1000];
            }
            for (int i = 0; i < deltaW[1].Length; i++)
            {
                deltaW[1][i] = 0;
                wLog[1][i] = new double[1000];
            }

            WeightRandomize();  
        }

        public void SchemeLabes()
        {
            w00Lable.Text = $"{Math.Round(w[0][0], 4)}";
            w01Lable.Text = $"{Math.Round(w[0][1], 4)}";
            w02Lable.Text = $"{Math.Round(w[0][2], 4)}";
            w03Lable.Text = $"{Math.Round(w[0][3], 4)}";
            w10Lable.Text = $"{Math.Round(w[1][0], 4)}";
            w11Lable.Text = $"{Math.Round(w[1][1], 4)}";

            a00Label.Text = $"{Math.Round(a[0][0], 2)}";
            a01Label.Text = $"{Math.Round(a[0][1], 2)}";
            a10Label.Text = $"{Math.Round(a[1][0], 2)}";
            a11Label.Text = $"{Math.Round(a[1][1], 2)}";
            label9.Text = $"{Math.Round(a[2][0], 2)}";
        }

        public void WeightRandomize()    
        {
            Random random = new Random();

            for (int i = 0; i <= 3; i++)
            {
                w[0][i] = Math.Round((random.NextDouble() + random.Next(-2, 2)), 3); //random value
            }

            for (int i = 0; i <= 1; i++)
            {
                w[1][i] = Math.Round((random.NextDouble() + random.Next(-2, 2)), 3);  //random value
            }

            //I1 = 1, I2 = 0, w1 = 0.45, w2 = 0.78 ,w3 = -0.12 ,w4 = 0.13 ,w5 = 1.5 ,w6 = -2.3.
            //w[0][0] = 0.45;
            //w[0][1] = 0.78;
            //w[0][2] = -0.12;
            //w[0][3] = 0.13;       // ВРЕМЕННЫЕ ЗНАЧЕНИЯ
            //w[1][0] = 1.5;
            //w[1][1] = -2.3;

            textBox.Text = $"NeuroWeb starting... \r\n \r\n" +
                          $"Аssigning random values ​​to weights:\r\n" +
                          $"w(00)={Convert.ToString(w[0][0])}  " +
                          $"w(01)={Convert.ToString(w[0][1])}  " +
                          $"w(02)={Convert.ToString(w[0][2])}  " +
                          $"w(03)={Convert.ToString(w[0][3])}\r\n" +
                          $"w(10)={Convert.ToString(w[1][0])}  " +
                          $"w(11)={Convert.ToString(w[1][1])}  ";

            InputCombination();
        }

        public void ActivationsComputing()   
        {
            double a10IN = a[0][0]*w[0][0] + a[0][1]*w[0][2];  // input sum
            double a10OUT = Sigmoid(a10IN);     // sigmoid input sum 
            a[1][0] = Math.Round(a10OUT, 2);    // output neuron activation

            double a11IN = a[0][1] * w[0][3] + a[0][0] * w[0][1];  
            double a11OUT = Sigmoid(a11IN);     
            a[1][1] = Math.Round(a11OUT, 2);

            double a20IN = a[1][0] * w[1][0] + a[1][1] * w[1][1];
            double a20OUT = Sigmoid(a20IN);
            a[2][0] = Math.Round(a20OUT, 2);

            if (checkBox.Checked)
                textBox.Text += 
                          $"     Activations:\r\n" +
                          $"a(10)={a[1][0]}\r\n" +
                          $"a(11)={a[1][1]}\r\n" +
                          $"OUT: a(20)={a[2][0]}";

            ErrorComputing();
        }

        public void ErrorComputing()
        {
            error = (y - a[2][0]) * (y - a[2][0]);     // (не использовал ^ т.к. не применим для double)
            errorArray[iterationCounter] = error;

            if (checkBox.Checked)
                textBox.Text += $"\r\n ERROR: {Math.Round(error,4)}";

            buttonCOMPUTE.Enabled = true;
            errorValueLabel.Text = $"Error: {Math.Round(error,4)}";

            if (Math.Round(error,4) == 0)
                errorValueLabel.ForeColor = Color.Red;
           
            SchemeLabes();
        }

        public void BackPropagationMethod()
        {
            delta[2][0] = (y - a[2][0]) * (1 - a[2][0]) * a[2][0];
            delta[1][0] = ((1 - a[1][0]) * a[1][0]) * (w[1][0] * delta[2][0]);
            delta[1][1] = ((1 - a[1][1]) * a[1][1]) * (w[1][1] * delta[2][0]);

            GRAD[1][0] = delta[2][0] * a[1][0];
            deltaW[1][0] = epsilon * GRAD[1][0] + alpha * deltaW[1][0];
            w[1][0] += deltaW[1][0];
            wLog[1][0][iterationCounter] = w[1][0];

            GRAD[1][1] = delta[2][0] * a[1][1];
            deltaW[1][1] = epsilon * GRAD[1][1] + alpha * deltaW[1][1];
            w[1][1] += deltaW[1][1];
            wLog[1][1][iterationCounter] = w[1][1];


            GRAD[0][0] = delta[1][0] * a[0][0];
            deltaW[0][0] = epsilon * GRAD[0][0] + alpha * deltaW[0][0];
            w[0][0] += deltaW[0][0];
            wLog[0][0][iterationCounter] = w[0][0];

            GRAD[0][2] = delta[1][0] * a[0][1];
            deltaW[0][2] = epsilon * GRAD[0][2] + alpha * deltaW[0][2];
            w[0][2] += deltaW[0][2];
            wLog[0][2][iterationCounter] = w[0][2];

            GRAD[0][1] = delta[1][1] * a[0][0];
            deltaW[0][1] = epsilon * GRAD[0][1] + alpha * deltaW[0][1];
            w[0][1] += deltaW[0][1];
            wLog[0][1][iterationCounter] = w[0][1];

            GRAD[0][3] = delta[1][1] * a[0][1];
            deltaW[0][3] = epsilon * GRAD[0][3] + alpha * deltaW[0][3];
            w[0][3] += deltaW[0][3];
            wLog[0][3][iterationCounter] = w[0][3];

            if (checkBox.Checked)
            textBox.Text += 
                $"\r\n\r\n\r\n" +
                $"{iterationCounter} ITERATION \r\n" +

                $"delta(20)={Math.Round(delta[2][0],6)}\r\n" +
                $"delta(10)={Math.Round(delta[1][0], 6)}\r\n" +
                $"delta(11)={Math.Round(delta[1][1], 6)}\r\n" +

                $"GRAD(00)={Math.Round(GRAD[0][0],6)}\r\n" +
                $"GRAD(01)={Math.Round(GRAD[0][1],6)}\r\n" +
                $"GRAD(02)={Math.Round(GRAD[0][2],6)}\r\n" +
                $"GRAD(03)={Math.Round(GRAD[0][3],6)}\r\n" +
                $"GRAD(10)={Math.Round(GRAD[1][0],6)}\r\n" +
                $"GRAD(11)={Math.Round(GRAD[1][1],6)}\r\n" +

                $"w(00)={Math.Round(w[0][0],6)}\r\n" +
                $"w(01)={Math.Round(w[0][1],6)}\r\n" +
                $"w(02)={Math.Round(w[0][2],6)}\r\n" +
                $"w(03)={Math.Round(w[0][3],6)}\r\n" +
                $"w(10)={Math.Round(w[1][0],6)}\r\n" +
                $"w(11)={Math.Round(w[1][1],6)}\r\n";

            ActivationsComputing();
        }

        public void Graphic()
        {
            Graphics graphic = pictureBoxGraphic.CreateGraphics();

            Pen blackPen = new Pen(Color.Black, 2f);
            Pen redPen = new Pen(Color.Red, 1f);
            Pen boldRedPen = new Pen(Color.Red, 2f);
            Pen greenPen = new Pen(Color.Green, 1f);
            Pen brownPen = new Pen(Color.Brown, 1f);
            Pen bluePen = new Pen(Color.Blue, 1f);
            Pen coralPen = new Pen(Color.Coral, 1f);
            Pen purplePen = new Pen(Color.Purple, 1f);

            graphic.DrawLine(blackPen, 0, 0, 0, 240);         //graph axe Y
            graphic.DrawLine(blackPen, 0, 120, 500, 120);     //graph axe X

            Point[] errorPoints = new Point[iterationCounter];
            for (int i = 0; i < iterationCounter; i++)
            {
                errorArray[i] = Math.Round(errorArray[i] * 100);
            }
            for (int i = 0; i < errorPoints.Length; i++)
            {
                errorPoints[i] = new Point(i * xScale, 120 - (int)errorArray[i]);
            }

                

            graphic.DrawLines(boldRedPen, errorPoints);     //error graphic





            //  The following are graphics of weights changes

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < iterationCounter; j++)
                {
                    wLog[0][i][j] = Math.Round(wLog[0][i][j] * yScale);
                }
            }     //weight[0] rounding and scale multiplication
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < iterationCounter; j++)
                {
                    wLog[1][i][j] = Math.Round(wLog[1][i][j] * yScale);
                }
            }     //weight[1] rounding and scale multiplication

            Point[] w00points = new Point[iterationCounter];        // w00 point array 
            for (int i = 0; i < w00points.Length; i++)
            {
                w00points[i] = new Point(i * xScale, 120 - (int)wLog[0][0][i]);  //w00 point array with weights of each iteration
            }
            graphic.DrawLines(redPen, w00points);      // w00 graphic

            Point[] w01points = new Point[iterationCounter];
            for (int i = 0; i < w01points.Length; i++)
            {
                w01points[i] = new Point(i * xScale, 120 - (int)wLog[0][1][i]);
            }
            graphic.DrawLines(brownPen, w01points);

            Point[] w02points = new Point[iterationCounter];
            for (int i = 0; i < w02points.Length; i++)
            {
                w02points[i] = new Point(i * xScale, 120 - (int)wLog[0][2][i]);
            }
            graphic.DrawLines(bluePen, w02points);

            Point[] w03points = new Point[iterationCounter];
            for (int i = 0; i < w03points.Length; i++)
            {
                w03points[i] = new Point(i * xScale, 120 - (int)wLog[0][3][i]);
            }
            graphic.DrawLines(greenPen, w03points);

            Point[] w10points = new Point[iterationCounter];
            for (int i = 0; i < w10points.Length; i++)
            {
                w10points[i] = new Point(i * xScale, 120 - (int)wLog[1][0][i]);
            }
            graphic.DrawLines(coralPen, w10points);

            Point[] w11points = new Point[iterationCounter];
            for (int i = 0; i < w11points.Length; i++)
            {
                w11points[i] = new Point(i * xScale, 120 - (int)wLog[1][1][i]);
            }
            graphic.DrawLines(purplePen, w11points);
        }

        public void Scheme()
        {
            Graphics pictureScheme = pictureBoxScheme.CreateGraphics();

            Pen blackPen = new Pen(Color.Black, 2f);
            Pen redPen = new Pen(Color.Red, 2f);
            Pen greenPen = new Pen(Color.Green, 2f);
            Pen brownPen = new Pen(Color.Brown, 2f);
            Pen bluePen = new Pen(Color.Blue, 2f);
            Pen coralPen = new Pen(Color.Coral, 2f);
            Pen purplePen = new Pen(Color.Purple, 2f);

            if (true)
            {
                pictureScheme.DrawEllipse(blackPen, 100, 50, 50, 50);
                pictureScheme.DrawEllipse(blackPen, 100, 150, 50, 50);
                pictureScheme.DrawEllipse(blackPen, 300, 50, 50, 50);
                pictureScheme.DrawEllipse(blackPen, 300, 150, 50, 50);
                pictureScheme.DrawEllipse(blackPen, 500, 100, 50, 50);

                pictureScheme.DrawLine(redPen, 150, 75, 300, 75);
                pictureScheme.DrawLine(greenPen, 150, 175, 300, 175);
                pictureScheme.DrawLine(brownPen, 150, 75, 300, 175);
                pictureScheme.DrawLine(bluePen, 150, 175, 300, 75);

                pictureScheme.DrawLine(coralPen,350,75,500,125);
                pictureScheme.DrawLine(purplePen,350,175,500,125);
            }   // scheme building

        }

        private void buttonSTART_Click(object sender, EventArgs e)
        {
            ArraysInitialization();

            buttonStart.Enabled = false;
            learningRateLabel.Enabled = false;
            momentLabel.Enabled = false;
            learningRateNUD.Enabled = false;
            momentNUD.Enabled = false;
            buttonRESET.Enabled = true;
            iterationCountNUD.Enabled = true;
            iterationCountLabel.Enabled = true;
            errorValueLabel.Enabled = true;
            checkBox.Enabled = true;
            xScaleNUD.Enabled = true;
            xScaleLabel.Enabled = true;
            yScaleNUD.Enabled = true;
            yScaleLabel.Enabled = true;

            epsilon = Convert.ToDouble(learningRateNUD.Value);
            alpha = Convert.ToDouble(momentNUD.Value);

            Scheme();
        }
        private void buttonRESET_Click(object sender, EventArgs e)
        {
            buttonRESET.Enabled = false;
            buttonCOMPUTE.Enabled = false;
            buttonStart.Enabled = true;
            learningRateLabel.Enabled = true;
            momentLabel.Enabled = true;
            learningRateNUD.Enabled = true;
            momentNUD.Enabled = true;
            iterationCountNUD.Enabled = false;
            iterationCountLabel.Enabled = false;
            errorValueLabel.Enabled = false;
            errorValueLabel.Text = "Error:";
            textBox.Text = "";
            iterationCounter = 0;
            iterationCountLabel.Text = "Iteration: 0";
            //iterationCountNUD.Value = 1;
            pictureBoxGraphic.Image = null;
            checkBox.Enabled = false;
            errorValueLabel.ForeColor = Color.Black;

            xScaleNUD.Enabled = false;
            xScaleLabel.Enabled = false;
            yScaleNUD.Enabled = false;
            yScaleLabel.Enabled = false;
        }
        private void buttonCOMPUTE_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < iterationCountNUD.Value; i++)
            {
                //if (error != 0)
                //{
                //    iterationCounter++;
                //    BackPropagationMethod();
                //}
                //else if (error == Math.Round(error, 4))
                //{
                //    iterationCounter++;
                //    InputCombination();
                //}

                InputCombination();
                iterationCounter++;
                BackPropagationMethod();

            }

            iterationCountLabel.Text = $"Iteration: {iterationCounter}";

            xScale = (int)xScaleNUD.Value;
            yScale = (int)yScaleNUD.Value;
            Graphic();
        }
    }



}
