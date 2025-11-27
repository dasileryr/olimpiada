using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class TeamForm : Form
    {
        private DatabaseHelper dbHelper;
        private DataGridView dataGridView;
        private ComboBox cmbCountry;
        private Label lblCountry;
        private Button btnShow;
        private Button btnClose;

        public TeamForm(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
            InitializeComponent();
            LoadCountries();
        }

        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            cmbCountry = new ComboBox();
            lblCountry = new Label();
            btnShow = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();

            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(12, 15);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(50, 15);
            lblCountry.Text = "Страна:";

            cmbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCountry.FormattingEnabled = true;
            cmbCountry.Location = new Point(80, 12);
            cmbCountry.Name = "cmbCountry";
            cmbCountry.Size = new Size(300, 23);
            cmbCountry.TabIndex = 0;

            btnShow.Location = new Point(386, 12);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(100, 30);
            btnShow.TabIndex = 1;
            btnShow.Text = "Показать";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += BtnShow_Click;

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 50);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(760, 400);
            dataGridView.TabIndex = 2;

            btnClose.Location = new Point(672, 460);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 3;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 502);
            Controls.Add(btnClose);
            Controls.Add(dataGridView);
            Controls.Add(btnShow);
            Controls.Add(cmbCountry);
            Controls.Add(lblCountry);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TeamForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Состав олимпиадной команды";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
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

        private void LoadData()
        {
            if (cmbCountry.SelectedValue == null)
            {
                MessageBox.Show("Выберите страну", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var countryId = (int)cmbCountry.SelectedValue;
            var athletes = dbHelper.GetCountryTeam(countryId);
            dataGridView.DataSource = athletes.Select(a => new
            {
                Фамилия = a.LastName,
                Имя = a.FirstName,
                Отчество = a.MiddleName ?? "",
                Дата_рождения = a.DateOfBirth.ToShortDateString()
            }).ToList();
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

