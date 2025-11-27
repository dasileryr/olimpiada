using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class CountryStatisticsForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool byOlympics;
        private Label lblResult;
        private ComboBox cmbCountry;
        private ComboBox cmbOlympics;
        private Label lblCountry;
        private Label lblOlympics;
        private Button btnShow;
        private Button btnClose;

        public CountryStatisticsForm(DatabaseHelper dbHelper, bool byOlympics)
        {
            this.dbHelper = dbHelper;
            this.byOlympics = byOlympics;
            InitializeComponent();
            LoadCountries();
            if (byOlympics)
                LoadOlympics();
        }

        private void InitializeComponent()
        {
            lblResult = new Label();
            cmbCountry = new ComboBox();
            cmbOlympics = new ComboBox();
            lblCountry = new Label();
            lblOlympics = new Label();
            btnShow = new Button();
            btnClose = new Button();
            SuspendLayout();

            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(12, 15);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(50, 15);
            lblCountry.Text = "Страна:";

            lblOlympics.AutoSize = true;
            lblOlympics.Location = new Point(12, byOlympics ? 45 : 15);
            lblOlympics.Name = "lblOlympics";
            lblOlympics.Size = new Size(70, 15);
            lblOlympics.Text = "Олимпиада:";
            lblOlympics.Visible = byOlympics;

            cmbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCountry.FormattingEnabled = true;
            cmbCountry.Location = new Point(80, 12);
            cmbCountry.Name = "cmbCountry";
            cmbCountry.Size = new Size(300, 23);
            cmbCountry.TabIndex = 0;

            cmbOlympics.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOlympics.FormattingEnabled = true;
            cmbOlympics.Location = new Point(100, byOlympics ? 42 : 12);
            cmbOlympics.Name = "cmbOlympics";
            cmbOlympics.Size = new Size(300, 23);
            cmbOlympics.TabIndex = 1;
            cmbOlympics.Visible = byOlympics;

            btnShow.Location = new Point(386, byOlympics ? 42 : 12);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(100, 30);
            btnShow.TabIndex = 2;
            btnShow.Text = "Показать";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += BtnShow_Click;

            lblResult.AutoSize = false;
            lblResult.Location = new Point(12, byOlympics ? 80 : 50);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(560, 200);
            lblResult.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblResult.TextAlign = ContentAlignment.MiddleCenter;

            btnClose.Location = new Point(472, byOlympics ? 290 : 260);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 3;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, byOlympics ? 332 : 302);
            Controls.Add(btnClose);
            Controls.Add(lblResult);
            Controls.Add(btnShow);
            Controls.Add(cmbOlympics);
            Controls.Add(cmbCountry);
            Controls.Add(lblOlympics);
            Controls.Add(lblCountry);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CountryStatisticsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = byOlympics ? "Статистика страны по олимпиаде" : "Статистика страны за всю историю";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadCountries()
        {
            var countries = dbHelper.GetAllCountries();
            cmbCountry.DataSource = countries;
            cmbCountry.DisplayMember = "CountryName";
            cmbCountry.ValueMember = "CountryId";
        }

        private void LoadOlympics()
        {
            var olympics = dbHelper.GetAllOlympics();
            cmbOlympics.DataSource = olympics.Select(o => new
            {
                o.OlympicsId,
                Display = $"{o.Year} - {o.City} ({o.HostCountryName})"
            }).ToList();
            cmbOlympics.DisplayMember = "Display";
            cmbOlympics.ValueMember = "OlympicsId";
        }

        private void LoadData()
        {
            if (cmbCountry.SelectedValue == null)
            {
                MessageBox.Show("Выберите страну", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (byOlympics && cmbOlympics.SelectedValue == null)
            {
                MessageBox.Show("Выберите олимпиаду", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var countryId = (int)cmbCountry.SelectedValue;
            int? olympicsId = byOlympics && cmbOlympics.SelectedValue != null 
                ? (int)cmbOlympics.SelectedValue 
                : null;

            var statistics = dbHelper.GetCountryStatistics(countryId, olympicsId);
            lblResult.Text = $"Страна: {statistics.CountryName}\n\n" +
                           $"Золото: {statistics.Gold}\n" +
                           $"Серебро: {statistics.Silver}\n" +
                           $"Бронза: {statistics.Bronze}\n" +
                           $"Всего: {statistics.Total}";
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

