using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace QuineMcCluskey
{
                 
    public partial class Form1 : Form
    {
        string result = "";
        int NoOfVariables;
        int NoOfStage = 1;
        int MaxStage = 1;
        string[, ,] cubes;
        string[, ,] AllminTerms;
        string[,] minTermBinaries = new string[11, 30];
        int MaximumOnes = 0; //Number of maximum 1s for every Minterm
        string[, ,] minTermIsImplicant = new string[11, 30, 30];
        //public Form1(int NoOfStage, int MaximumOnes, string[, ,] cubes, string[,] minTermBinaries, string[,,] minTermIsImplicant)
        //{
        //    this.NoOfStage = NoOfStage;
        //    this.MaximumOnes = MaximumOnes;
        //    this.cubes = cubes;
        //    this.minTermBinaries = minTermBinaries;
        //    this.minTermIsImplicant = minTermIsImplicant;
        //}
        public Form1()
        {
            InitializeComponent();
        }
        private string ConvertToBinary(int x)
        {
            string s = "";
            char[] res,cl;
            while (x>0)
            {
                s = s + (x % 2).ToString();
                x = x / 2;
            }
            res=s.ToCharArray();
            cl=(char[])res.Clone();
            s = "";
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = cl[res.Length-i-1];
                s = s + res[i].ToString();
            }
            return s;
        }
        private string ConvertToDecimal(string x)
        {
            int result=0,m=1,countDash=0;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i]=='-')
                {
                    countDash++;
                }
            }


            for (int i = 0; i < x.Length; i++)
            {
                result += m * Convert.ToInt32(Convert.ToString(x[x.Length -1 - i]));
                m=m*2;
            }
            return result.ToString();
        
        }

        private object diffOne(string a, string b)
        {
            int t = 0;
            StringBuilder r=new StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                if (a.Length<b.Length)
                {
                    a = "0" + a;
                }
                if (a[i]!=b[i])
                {
                    t++;
                    r.Append('-');
                }else
            	{
                     r.Append(a[i]);
	            }
            }
            if (t==1)
        	{
                return r.ToString();
            }
            else
            {
                return null;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            string temp="";
            string extraZeros = "";
            NoOfVariables = (int)numVariable.Value;
            //Getting Minterms
            string[] minTerms=txtMinterms.Text.Split(',');
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    for (int k = 0; k < 30; k++)
                    {
                        minTermIsImplicant[i, j,k] = " *"; 
                    }
                   
                }
            }
            //Categorizing Minterms
            for (int i = 0; i < minTerms.Length; i++)
            {
                if (ConvertToBinary( Convert.ToInt32(minTerms[i])).Length <NoOfVariables)
                {
                    extraZeros = "";
                    for (int f = 1; f <= NoOfVariables-ConvertToBinary(Convert.ToInt32(minTerms[i])).Length; f++)
                    {
                        extraZeros += "0";
                    }
                    minTermBinaries[1, i] = extraZeros + ConvertToBinary(Convert.ToInt32(minTerms[i]));
                }else
                minTermBinaries[1,i]= ConvertToBinary( Convert.ToInt32(minTerms[i]));
                if (minTermBinaries[1,i].Split('1').Length-1>MaximumOnes)
	            {
                    MaximumOnes = minTermBinaries[1,i].Split('1').Length-1;
	            }
                
            }
           
            cubes =new string[10,MaximumOnes+1,20];
            AllminTerms = new string[10, MaximumOnes + 1, 20];
            int index;
            for (int NoOfOnes = 0; NoOfOnes <= MaximumOnes; NoOfOnes++)
            {
                index = 0;
                for (int minTermIndex = 0; minTermIndex < minTerms.Length; minTermIndex++)
			        {
                    if (minTermBinaries[1,minTermIndex].Split('1').Length-1==NoOfOnes)
            	        {
                            cubes[1,NoOfOnes,index] = minTermBinaries[1,minTermIndex];
                            AllminTerms[1, NoOfOnes, index] = ConvertToDecimal(minTermBinaries[1, minTermIndex]);
                            index++;
            	        }
			 
			        }
                
            }

            int l = 0;
            for (int Stage = 1; minTermBinaries[Stage,0]!=null; Stage++)
            {
                int indexOfMinterm = 0;
                //
                for (int NoOfOnes = 1; NoOfOnes < MaximumOnes; NoOfOnes++)
                {
                    l = 0;
                    for (int j = 0; cubes[Stage, NoOfOnes, j] != null; j++)
                    {
                        for (int x = 0; cubes[Stage, NoOfOnes + 1, x] != null; x++)
                        {
                            if (diffOne(cubes[Stage, NoOfOnes, j], cubes[Stage, NoOfOnes + 1, x]) != null)
                            {
                                minTermIsImplicant[Stage, NoOfOnes, j] = "";
                                minTermIsImplicant[Stage, NoOfOnes + 1, x] = "";
                            }
                            if (diffOne(cubes[Stage, NoOfOnes, j], cubes[Stage, NoOfOnes + 1, x]) != null && temp != (string)diffOne(cubes[Stage, NoOfOnes, j], cubes[Stage, NoOfOnes + 1, x]))
                            {
                                minTermBinaries[Stage + 1, indexOfMinterm] = (string)diffOne(cubes[Stage, NoOfOnes, j], cubes[Stage, NoOfOnes + 1, x]);
                                AllminTerms[Stage + 1, NoOfOnes, l] = AllminTerms[Stage, NoOfOnes, j] +","+ AllminTerms[Stage, NoOfOnes+1, x];
                                temp = (string)diffOne(cubes[Stage, NoOfOnes, j], cubes[Stage, NoOfOnes + 1, x]);
                                indexOfMinterm++;
                                l++;
                            }
                            //else
                            //{
                            //    minTermIsImplicant[Stage, NoOfOnes, j] = " *";
                            //    minTermIsImplicant[Stage, NoOfOnes + 1, x] = " *";
                            //}
                        }
                    }
                }

                //Removing extra repeated minterms
                
                
                //


                for (int NoOfOnes = 0; NoOfOnes <= MaximumOnes; NoOfOnes++)
                {
                    index = 0;
                    for (int minTermIndex = 0; minTermBinaries[Stage+1,minTermIndex]!=null; minTermIndex++)
                    {
                        if (minTermBinaries[Stage+1, minTermIndex].Split('1').Length - 1 == NoOfOnes)
                        {
                            cubes[Stage+1, NoOfOnes, index] = minTermBinaries[Stage+1, minTermIndex];
                            index++;
                        }

                    }

                }
                MaxStage = Stage;

            }
            //Showing the Results
            lblStage.Text = "Stage 1";
            NoOfStage = 1;
                for (int NoOfOnes = 1;NoOfOnes<=MaximumOnes; NoOfOnes++)
                {
                    for (int p = 0;cubes[NoOfStage, NoOfOnes, p]!=null; p++)
                    {
                       txtResult.Text+="m("+AllminTerms[NoOfStage,NoOfOnes,p]+")="+ cubes[NoOfStage, NoOfOnes, p] + minTermIsImplicant[NoOfStage, NoOfOnes, p] + "\r\n";
                       // txtResult.Text +="m("+ConvertToDecimal(cubes[NoOfStage, NoOfOnes, p])+")=" + cubes[NoOfStage, NoOfOnes, p] + minTermIsImplicant[NoOfStage, NoOfOnes, p] + "\r\n";
                    }
                    txtResult.Text += "------------\r\n";
                }
                string resultTemp = "";
                string expression = "";
                int StageTemp = 1;
                for (int NoOfOnes = 1; NoOfOnes <= MaximumOnes; NoOfOnes++)
                {
                    for (int p = 0; cubes[StageTemp, NoOfOnes, p] != null && minTermIsImplicant[StageTemp, NoOfOnes, p] == " *"; p++)
                    {
                        for (int NoOfVars = 0; NoOfVars < NoOfVariables; NoOfVars++)
                        {
                            if (cubes[StageTemp, NoOfOnes, p][NoOfVars] == '1')
                            {
                                expression = Convert.ToString(Convert.ToChar(65 + NoOfVars));
                            }
                            else if (cubes[StageTemp, NoOfOnes, p][NoOfVars] == '0')
                            {
                                expression = Convert.ToString(Convert.ToChar(65 + NoOfVars)) + "\'";
                            }
                            else
                            {
                                expression = "";
                            }
                            resultTemp += expression;
                        }

                        resultTemp += " + ";

                    }

                }
                try
                {
                    resultTemp = resultTemp.Remove(resultTemp.Length - 2);

                }
                catch (Exception)
                {
                    
                }
                txtResult.Text += "\r\n" + resultTemp;

             //The Final Optimized Result

            expression = "";
            for (int i = 0; i < NoOfVariables; i++)
            {
                result += Convert.ToString(Convert.ToChar(65 + i)) + ",";
            }
            result = result.Remove(result.Length - 1);
            result += ") = ";
            for (int Stage = 1;minTermBinaries[Stage,0]!=null ; Stage++)
            {
                for (int NoOfOnes = 1; NoOfOnes <= MaximumOnes; NoOfOnes++)
                {
                    for (int p = 0; cubes[Stage, NoOfOnes, p] != null && minTermIsImplicant[Stage, NoOfOnes, p] == " *"; p++)
                    {
                        for (int NoOfVars = 0; NoOfVars < NoOfVariables; NoOfVars++)
                        {
                            if (cubes[Stage, NoOfOnes, p][NoOfVars] == '1')
                            {
                                expression = Convert.ToString(Convert.ToChar(65 + NoOfVars));
                            }
                            else if (cubes[Stage, NoOfOnes, p][NoOfVars] == '0')
                            {
                                expression = Convert.ToString(Convert.ToChar(65 + NoOfVars)) + "\'";
                            }
                            else
                            {
                                expression = "";
                            }
                            result += expression;
                        }
                        
                        result += " + ";

                    }
                    
                }
            }
            result = result.Remove(result.Length - 2);
            result = result;
            btnNextStage.Enabled = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("this program is WriTTen bY : @minD (Amin Dorostanian)\r\nUniversity of Tabriz\r\nFaculty of Computer and Electrical Engineering\r\nE-mail:amin.dorost@gmail.com");
        }

        private void btnNextStage_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(NoOfStage,MaxStage,MaximumOnes,cubes,minTermBinaries,minTermIsImplicant,AllminTerms,NoOfVariables,result);
            f.Show();
            
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                txtResult.ForeColor = cd.Color;
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = cd.Color;
            }
        }

        private void lblFinalResult_MouseHover(object sender, EventArgs e)
        {
            //MessageBox.Show(lblFinalResult.Text,"Final Result");
        }
    }
}
