using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2
{
    public partial class Read : Form
    {
        public Read()
        {
            InitializeComponent();
        }
        private String get_read = null;
        private int[,] get_martix = new int[11,11];
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Documents|*.txt", Multiselect = false, ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(950), true))
                        {
                            get_read = await sr.ReadToEndAsync();
                            test.Text = get_read;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("讀檔失敗!!!!");
            }

            //可以啟動過濾
            button2.Enabled = true;
        }

        private String de = "";
        private bool warn_check = false;
        private char[]  double_check = new char[2];
        private int d_count = 0;
        private int c_count = 0;
        private int top_vertext = 0;
        
        private void do_str()
        {
            String reg = "";
            for(int i = 0; i <get_read.Length; i++)
            {
                if((get_read[i] >= '0' && get_read[i] <= '9') && (get_read[i+1] >= '0' && get_read[i+1] <= '9'))
                {
                    warn_check = true;
                    double_check[0] = get_read[i+1];
                    double_check[1] = get_read[i];
                    reg = reg + get_read[i];
                    c_count = d_count + 1;
                }
                else if (get_read[i] >= '0' && get_read[i] <= '9')
                {
                    reg = reg + get_read[i];
                    d_count++;
                }
     
            }
            test.Text = reg;
            int count = 0 , reg_x = 0 , reg_y = 0;
            
            for(int j = 0; j < reg.Length; j++)
            {
                count++;
                char c = reg[j];
                int r = c - '0';
                if(c_count-1 == j)
                {
                    char d = reg[j + 1];
                    int r2 = d - '0';
                    r = r * 10 + r2;
                    j++;
                }
                
                de = de + " " + r;
                if (count == 1)
                {
                    if(r > top_vertext)
                    {
                        top_vertext = r;
                    }
                    reg_x = r;
                }
                else if (count == 2)
                {
                    if (r > top_vertext)
                    {
                        top_vertext = r;
                    }
                    reg_y = r;
                }
                else if (count == 3)
                {
                    get_martix[reg_x, reg_y] = r;
                    get_martix[reg_y, reg_x] = r;
                    count = 0;
                }
            }
           
        }

        private void button2_Click(object sender, EventArgs ex)
        {
            try {
                do_str();
            } catch(Exception e)
            {
                MessageBox.Show(""+e.Message );
            }

            //可以啟動演算
            button3.Enabled = true;
            //把自己關閉
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                algorithm_Prim();
            }
            catch (Exception e1)
            {
                MessageBox.Show("" + e1.Message);
            }
        }

        private int route_total = 0;
        private string result = "";
        private void algorithm_Prim()
        {
            int[] X = new int[11];
            int[] Y = new int[11];

            //先做預設 X->拜訪過的 Y沒拜訪過的
            for (int i = 0; i < 11; i++)
            {
                X[i] = 0;
            }
            for (int j = 0; j < 11; j++)
            {
                Y[j] = 1;
            }
            //不管幾個點都從Vertex3開始做 by Prim's algorithm
            int ini_index = 3;
            X[ini_index] = 1;
            Y[ini_index] = 0;
            bool check_out = false;
            while (!check_out)
            {
                //尋找對Y裡面所有X有連到的點的weight看哪個最小
                int min_route = 99;
                for(int i = 0; i <= top_vertext; i++)
                {
                    //外層迴圈繞X 如果是1才動作
                    if (X[i] == 1)
                    {
                        //這邊先找到哪個weight是最小的
                        for(int j = 0; j <= top_vertext; j++)
                        {
                            //這邊先檢查是不是Y裡面的元素
                            if (Y[j] == 1)
                            {
                                //這邊等於0是表示沒有weight表示不能連上去的
                                if (get_martix[i, j] < min_route && get_martix[i, j] != 0)
                                {
                                    min_route = get_martix[i, j];
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        continue;
                    }      
                }

               
                //比對weight最小的是哪個點
                for (int q = 0; q <= top_vertext; q++)
                {
                    if (X[q] == 1)
                    {
                        for (int k = 0; k <= top_vertext; k++)
                        {
                            //這邊先確認過他是不是Y裡面的元素
                            if (Y[k] == 1)
                            {
                                if (min_route == get_martix[q, k])
                                {
                                    route_total += min_route;
                                    //把拜訪到的點加入X 同時把Y的點去掉
                                    X[k] = 1;
                                    Y[k] = 0;

                                    result = result +" " + k + "-" + q + ":" + get_martix[q, k] + "\n";
                                }
                            }
                            
                        }
                    }      
                }



                
                //判斷是不是要結束如果不是就繼續算
                int sum_break = 0;
                for (int k = 0; k <= top_vertext; k++)
                {
                    sum_break += X[k];
                }
                
                if (sum_break == top_vertext)
                {
                    check_out = true;
                    
                }
            }

            MessageBox.Show(""+result);

            
            result = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
