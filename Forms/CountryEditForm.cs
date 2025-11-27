using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class CountryEditForm : Form
    {
        private DatabaseHelper dbHelper;
        private Country? country;
        private TextBox txtName;
        private Button btnSave;
        private Button btnCancel;

        public CountryEditForm(DatabaseHelper dbHelper, Country? country)
        {
            this.dbHelper = dbHelper;
            this.country = country;
            InitializeComponent();
            if (country != null)
                LoadData();
        }

        private void InitializeComponent()
        {
            Label lblName = new Label();
            txtName = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();

            lblName.AutoSize = true;
            lblName.Location = new Point(12, 15);
            lblName.Name = "lblName";
            lblName.Size = new Size(50, 15);
            lblName.Text = "Страна:";

            txtName.Location = new Point(80, 12);
            txtName.Name = "txtName";
            txtName.Size = new Size(200, 23);
            txtName.TabIndex = 0;

            btnSave.Location = new Point(124, 50);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 1;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            btnCancel.Location = new Point(205, 50);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 92);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtName);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CountryEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = country == null ? "Добавить страну" : "Редактировать страну";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadData()
        {
            if (country != null)
                txtName.Text = country.CountryName;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название страны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (country == null)
                country = new Country();

            country.CountryName = txtName.Text;

            if (country.CountryId == 0)
                dbHelper.AddCountry(country);
            else
                dbHelper.UpdateCountry(country);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

