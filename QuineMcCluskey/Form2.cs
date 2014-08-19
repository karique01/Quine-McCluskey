using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuineMcCluskey
{
    public partial class Form2 : Form
    {
        int NoOfVariables;
        string result = "";
        int NoOfStage = 1;
        int MaxStage = 1;
        string[, ,] cubes;
        string[, ,] AllminTerms;
        string[,] minTermBinaries = new string[11, 30];
        int MaximumOnes = 0; //Number of maximum 1s for every Minterm
        string[, ,] minTermIsImplicant = new string[11, 30, 30];
     
         public Form2(int noOfStage,int maxStage, int maximumOnes, string[, ,] Kubes, string[,] MinTermBinaries, string[,,] MinTermIsImplicant,string[,,] allminterms,int noOfVars,string res)
        {
            InitializeComponent();
            this.NoOfStage = noOfStage;
            this.MaxStage = maxStage;
            this.MaximumOnes = maximumOnes;
            this.cubes = Kubes;
            this.minTermBinaries = MinTermBinaries;
            this.minTermIsImplicant = MinTermIsImplicant;
            this.AllminTerms = allminterms;
            this.NoOfVariables = noOfVars;
            this.result = res;
            doit();
        }
        void doit()
        {
            string expression = "";
            int StageTemp;
            string resultTemp="";
            StageTemp = NoOfStage+1;
            if (NoOfStage == MaxStage)
            {
                btnNextStage.Enabled = false;
                lblStage.Text = "Final Result :";
                txtResult.Text = result;
            }
            else
            {
                NoOfStage++;
                try
                {
                    if (minTermBinaries[NoOfStage, 0] != null)
                    {
                        txtResult.Text = "";
                        lblStage.Text = "Stage " + NoOfStage.ToString();
                        for (int NoOfOnes = 1; NoOfOnes <= MaximumOnes && cubes[NoOfStage, NoOfOnes, 0] != null; NoOfOnes++)
                        {
                            for (int p = 0; cubes[NoOfStage, NoOfOnes, p] != null; p++)
                            {
                                txtResult.Text += "m(" + AllminTerms[NoOfStage, NoOfOnes, p] + ")=" + cubes[NoOfStage, NoOfOnes, p] + minTermIsImplicant[NoOfStage, NoOfOnes, p] + "\r\n";
                            }
                            txtResult.Text += "------------\r\n";
                        }
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
                        txtResult.Text += resultTemp;
                    }
                    else
                    {
                        btnNextStage.Enabled = false;
                    }
                }
                catch (Exception)
                {

                }
            }
            //f.btnPreviousStage.Enabled = true;
        }

        private void btnNextStage_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(NoOfStage, MaxStage, MaximumOnes, cubes, minTermBinaries, minTermIsImplicant, AllminTerms,NoOfVariables,result);
            f.Show();
        }

       
        
    }
}
