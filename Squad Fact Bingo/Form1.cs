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

namespace Squad_Fact_Bingo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Guess guess = new Guess();

        private void BTNimport_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            using StreamReader reader = new StreamReader(openFileDialog1.FileName);
            List<string[]> rows = new List<string[]>();
            while (true)
            {
                string[] row = reader.ReadLine()?.Split('\t');
                if (row == null)
                {
                    break;
                }
                rows.Add(row);
            }

            int guesses = rows.Max(row => row.Length) - 1;

            string[] countsRow = rows.Last();
            rows.RemoveAt(rows.Count - 1);
            if (countsRow[0] != "count")
            {
                MessageBox.Show("The bottom-left cell should say \"count\"");
                return;
            }

            guess.table.counts = new int[guesses];
            for (int i = 0; i < guesses; i++)
            {
                guess.table.counts[i] = i + 1 < countsRow.Length && int.TryParse(countsRow[i + 1], out int result) ? result : Constants.NoNum;
            }

            guess.table.facts = new string[rows.Count];
            for (int i = 0; i < rows.Count; i++)
            {
                guess.table.facts[i] = rows[i][0];
            }

            guess.table.names = new string[guesses][];
            for (int i = 0; i < guesses; i++)
            {
                guess.table.names[i] = new string[rows.Count];
                for (int j = 0; j < rows.Count; j++)
                {
                    guess.table.names[i][j] = i + 1 < rows[j].Length ? rows[j][i + 1] : string.Empty;
                }
            }

            guess.feedback = new Feedback[guesses][];
            for (int i = 0; i < guesses; i++)
            {
                guess.feedback[i] = new Feedback[rows.Count];
            }

            RewriteTable();

            guess.Clear();
        }

        private void RewriteTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Fact", "Fact");
            for (int i = 0; i < guess.table.names.Length; i++)
            {
                string name = "Guess" + (i + 1).ToString();
                dataGridView1.Columns.Add(name, name);
            }
            for (int i = 0; i < guess.table.facts.Length; i++)
            {
                string[] rowData = new string[guess.table.names.Length + 1];
                rowData[0] = guess.table.facts[i];
                for (int j = 0; j < guess.table.names.Length; j++)
                {
                    rowData[j + 1] = guess.table.names[j][i];
                }
                dataGridView1.Rows.Add(rowData);
            }
            string[] finalRowData = new string[guess.table.names.Length + 1];
            finalRowData[0] = string.Empty;
            for (int i = 0; i < guess.table.names.Length; i++)
            {
                finalRowData[i + 1] = guess.table.counts[i].ToString();
            }
            dataGridView1.Rows.Add(finalRowData);

            RecolourTable();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            guess.GiveFeedback(e.ColumnIndex - 1, e.RowIndex, e.Button == MouseButtons.Left ? Feedback.No : Feedback.Yes);
            RecolourTable();
        }

        private void RecolourTable()
        {
            for (int i = 0; i < guess.feedback.Length; i++)
            {
                for (int j = 0; j < guess.feedback[i].Length; j++)
                {
                    dataGridView1.Rows[j].Cells[i + 1].Style.BackColor = guess.feedback[i][j] switch
                    {
                        Feedback.Yes => Color.Green,
                        Feedback.No => Color.Red,
                        Feedback.Unsure => Color.White,
                        _ => throw new InvalidEnumArgumentException()
                    };
                }
            }
            dataGridView1.ClearSelection();
        }

        private void BTNclear_Click(object sender, EventArgs e)
        {
            guess.Clear();
            RecolourTable();
        }

        private void BTNguess_Click(object sender, EventArgs e)
        {
            var result = guess.MakeGuess();
            MessageBox.Show(string.Join("\n", result.Select(pair => $"{pair.Item2} - \t{pair.Item1}")));
        }
    }
}
