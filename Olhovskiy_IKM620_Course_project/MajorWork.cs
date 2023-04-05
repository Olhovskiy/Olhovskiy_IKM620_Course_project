using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Olhovskiy_IKM620_Course_project
{
    class MajorWork
    {
        private string SaveFileName;
        private string OpenFileName;
        public void WriteSaveFileName(string S)
        {
            this.SaveFileName = S;
        }
        public void WriteOpenFileName(string S)
        {
            this.OpenFileName = S;
        }
        private System.DateTime TimeBegin;
        private string Data;
        private string Result;
        public bool Modify;
        private int Key;
        public void SetTime()
        {
            this.TimeBegin = System.DateTime.Now;
        }
        public System.DateTime GetTime()
        {
            return this.TimeBegin;
        }
        public void Write(string D)
        {
            this.Data = D;
        }
        public string Read()
        {
            return this.Result;
        }
        public void Task()
        {
            if (this.Data.Length > 5)
            {
                this.Result = Convert.ToString(true);

            }
            else
            {
                this.Result = Convert.ToString(false);
            }
            this.Modify = true;
        }
        public void SaveToFile()
        {
            if (!this.Modify)
                return;
            try
            {
                Stream S;
                if (File.Exists(this.SaveFileName))
                    S = File.Open(this.SaveFileName, FileMode.Append);
                else
                    S = File.Open(this.SaveFileName, FileMode.Create);
                Buffer D = new Buffer();
                D.Data = this.Data;
                D.Result = Convert.ToString(this.Result);
                D.Key = Key;
                Key++;
                BinaryFormatter BF = new BinaryFormatter();
                S.Flush();
                S.Close();
                this.Modify = false;
            }
            catch
            {
                MessageBox.Show("Помилка роботи з файлом");
            }

        }
        public void ReadFromFile(System.Windows.Forms.DataGridView DG)
        {
            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("Файлу немає");
                    return;
                }
                Stream S;
                S = File.Open(this.OpenFileName, FileMode.Open);
                Buffer D;
                object O;
                BinaryFormatter BF = new BinaryFormatter();
                System.Data.DataTable MT = new System.Data.DataTable();
                System.Data.DataColumn cKey = new System.Data.DataColumn("Ключ");
                System.Data.DataColumn cInput = new System.Data.DataColumn("Вхідні дані");
                System.Data.DataColumn cResult = new System.Data.DataColumn("Результат");
                MT.Columns.Add(cKey);
                MT.Columns.Add(cInput);
                MT.Columns.Add(cResult);
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    System.Data.DataRow MR;
                    MR = MT.NewRow();
                    MR["Ключ"] = D.Key;
                    MR["Вхідні дані"] = D.Data;
                    MR["Результат"] = D.Result;
                    MT.Rows.Add(MR);

                }
                DG.DataSource = MT;
                S.Close();
            }
            catch
            {
                MessageBox.Show("Помилка файлу");
            }
        }
        public void Generator()
        {
            try
            {
                if (!File.Exists(this.SaveFileName))
                {
                    Key = 1;
                    return;
                }
                Stream S;
                S = File.Open(this.SaveFileName, FileMode.Open);
                Buffer D;
                object O;
                BinaryFormatter BF = new BinaryFormatter();
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    Key = D.Key;
                }
                Key++;
                S.Close();
            }
            catch
            {
                MessageBox.Show("Помилка файлу");
            }
        }
        public bool SaveFileNameExists()
        {
            if (this.SaveFileName == null)
                return false;
            else return true;
        }
        public void NewRec()
        {
            this.Data = "";
            this.Result = null;
        }
        public void Find(string Num)
        {
            int N;
            try
            {
                N = Convert.ToInt16(Num);
            }
            catch
            {
                MessageBox.Show("помилка пошукового запиту");
                return;
            }
            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файлу немає");
                    return;
                }

                Stream S;
                S = File.Open(this.OpenFileName, FileMode.Open);
                Buffer D;
                object O;
                BinaryFormatter BF = new BinaryFormatter();
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    if (D.Key == N)
                    {
                        string ST;
                        ST = "Запис містить:" + (char)13 + "No" + Num + "Вхідні дані:" + D.Data + "Результат:" + D.Result;

                        MessageBox.Show(ST, "Запис знайдена");
                        S.Close();
                        return;
                    }
                }
                S.Close();
                MessageBox.Show("Запис не знайдена");
            }
            catch
            {
                MessageBox.Show("Помилка файлу");
            }
        }
    }
}


    


