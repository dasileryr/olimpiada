using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class OlympicsEditForm : Form
    {
        private DatabaseHelper dbHelper;
        private Olympics? olympics;
        private TextBox txtYear;
        private ComboBox cmbType;
        private ComboBox cmbCountry;
        private TextBox txtCity;
        private Button btnSave;
        private Button btnCancel;

        public OlympicsEditForm(DatabaseHelper dbHelper, Olympics? olympics)
        {
            this.dbHelper = dbHelper;
            this.olympics = olympics;
            InitializeComponent();
            LoadCountries();
            if (olympics != null)
                LoadData();
        }

        private void InitializeComponent()
        {
            Label lblYear = new Label();
            Label lblType = new Label();
            Label lblCountry = new Label();
            Label lblCity = new Label();
            txtYear = new TextBox();
            cmbType = new ComboBox();
            cmbCountry = new ComboBox();
            txtCity = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();

            // lblYear
            lblYear.AutoSize = true;
            lblYear.Location = new Point(12, 15);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(28, 15);
            lblYear.Text = "Год:";

            // lblType
            lblType.AutoSize = true;
            lblType.Location = new Point(12, 45);
            lblType.Name = "lblType";
            lblType.Size = new Size(30, 15);
            lblType.Text = "Тип:";

            // lblCountry
            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(12, 75);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(50, 15);
            lblCountry.Text = "Страна:";

            // lblCity
            lblCity.AutoSize = true;
            lblCity.Location = new Point(12, 105);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(40, 15);
            lblCity.Text = "Город:";

            // txtYear
            txtYear.Location = new Point(80, 12);
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(200, 23);
            txtYear.TabIndex = 0;

            // cmbType
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "Летняя", "Зимняя" });
            cmbType.Location = new Point(80, 42);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(200, 23);
            cmbType.TabIndex = 1;

            // cmbCountry
            cmbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCountry.FormattingEnabled = true;
            cmbCountry.Location = new Point(80, 72);
            cmbCountry.Name = "cmbCountry";
            cmbCountry.Size = new Size(200, 23);
            cmbCountry.TabIndex = 2;

            // txtCity
            txtCity.Location = new Point(80, 102);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(200, 23);
            txtCity.TabIndex = 3;

            // btnSave
            btnSave.Location = new Point(124, 140);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            // btnCancel
            btnCancel.Location = new Point(205, 140);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            // OlympicsEditForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 182);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtCity);
            Controls.Add(cmbCountry);
            Controls.Add(cmbType);
            Controls.Add(txtYear);
            Controls.Add(lblCity);
            Controls.Add(lblCountry);
            Controls.Add(lblType);
            Controls.Add(lblYear);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OlympicsEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = olympics == null ? "Добавить олимпиаду" : "Редактировать олимпиаду";
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
            if (olympics != null)
            {
                txtYear.Text = olympics.Year.ToString();
                cmbType.SelectedIndex = olympics.IsSummer ? 0 : 1;
                cmbCountry.SelectedValue = olympics.HostCountryId;
                txtCity.Text = olympics.City;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtYear.Text, out int year) || year < 1896 || year > 2100)
            {
                MessageBox.Show("Введите корректный год", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип олимпиады", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbCountry.SelectedValue == null)
            {
                MessageBox.Show("Выберите страну", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Введите город", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (olympics == null)
            {
                olympics = new Olympics();
            }

            olympics.Year = year;
            olympics.IsSummer = cmbType.SelectedIndex == 0;
            olympics.HostCountryId = (int)cmbCountry.SelectedValue;
            olympics.City = txtCity.Text;

            if (olympics.OlympicsId == 0)
                dbHelper.AddOlympics(olympics);
            else
                dbHelper.UpdateOlympics(olympics);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

