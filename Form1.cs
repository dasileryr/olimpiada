using Olimpiada.DataAccess;
using Olimpiada.Forms;

namespace Olimpiada
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper;

        public Form1()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void олимпиадыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new OlympicsForm(dbHelper);
            form.ShowDialog();
        }

        private void страныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CountriesForm(dbHelper);
            form.ShowDialog();
        }

        private void видыСпортаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SportsForm(dbHelper);
            form.ShowDialog();
        }

        private void спортсменыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AthletesForm(dbHelper);
            form.ShowDialog();
        }

        private void результатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ResultsForm(dbHelper);
            form.ShowDialog();
        }

        private void медальныйЗачетПоОлимпиадеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MedalStandingForm(dbHelper, true);
            form.ShowDialog();
        }

        private void медальныйЗачетЗаВсюИсториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MedalStandingForm(dbHelper, false);
            form.ShowDialog();
        }

        private void медалистыПоОлимпиадеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MedalistsForm(dbHelper, true);
            form.ShowDialog();
        }

        private void медалистыЗаВсюИсториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MedalistsForm(dbHelper, false);
            form.ShowDialog();
        }

        private void странаСБольшеВсегоЗолотыхПоОлимпиадеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TopGoldCountryForm(dbHelper, true);
            form.ShowDialog();
        }

        private void странаСБольшеВсегоЗолотыхЗаВсюИсториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TopGoldCountryForm(dbHelper, false);
            form.ShowDialog();
        }

        private void спортсменСБольшеВсегоЗолотыхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TopGoldAthleteForm(dbHelper);
            form.ShowDialog();
        }

        private void странаХозяйкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var country = dbHelper.GetMostFrequentHostCountry();
            MessageBox.Show(
                country != null 
                    ? $"Страна, которая чаще всех была хозяйкой олимпиады: {country}"
                    : "Данные не найдены",
                "Страна-хозяйка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void составКомандыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TeamForm(dbHelper);
            form.ShowDialog();
        }

        private void статистикаПоОлимпиадеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CountryStatisticsForm(dbHelper, true);
            form.ShowDialog();
        }

        private void статистикаЗаВсюИсториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CountryStatisticsForm(dbHelper, false);
            form.ShowDialog();
        }
    }
}
