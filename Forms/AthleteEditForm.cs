using Olimpiada.DataAccess;
using Olimpiada.Models;
using System.Drawing;
using System.IO;

namespace Olimpiada.Forms
{
    public partial class AthleteEditForm : Form
    {
        private DatabaseHelper dbHelper;
        private Athlete? athlete;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtMiddleName;
        private ComboBox cmbCountry;
        private DateTimePicker dtpDateOfBirth;
        private Button btnLoadPhoto;
        private Button btnSave;
        private Button btnCancel;
        private PictureBox pictureBox;
        private byte[]? photoData;

        public AthleteEditForm(DatabaseHelper dbHelper, Athlete? athlete)
        {
            this.dbHelper = dbHelper;
            this.athlete = athlete;
            InitializeComponent();
            LoadCountries();
            if (athlete != null)
                LoadData();
        }

        private void InitializeComponent()
        {
            Label lblFirstName = new Label();
            Label lblLastName = new Label();
            Label lblMiddleName = new Label();
            Label lblCountry = new Label();
            Label lblDateOfBirth = new Label();
            Label lblPhoto = new Label();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtMiddleName = new TextBox();
            cmbCountry = new ComboBox();
            dtpDateOfBirth = new DateTimePicker();
            btnLoadPhoto = new Button();
            pictureBox = new PictureBox();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();

            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(12, 15);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Size = new Size(34, 15);
            lblFirstName.Text = "Имя:";

            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(12, 45);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(58, 15);
            lblLastName.Text = "Фамилия:";

            lblMiddleName.AutoSize = true;
            lblMiddleName.Location = new Point(12, 75);
            lblMiddleName.Name = "lblMiddleName";
            lblMiddleName.Size = new Size(58, 15);
            lblMiddleName.Text = "Отчество:";

            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(12, 105);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(50, 15);
            lblCountry.Text = "Страна:";

            lblDateOfBirth.AutoSize = true;
            lblDateOfBirth.Location = new Point(12, 135);
            lblDateOfBirth.Name = "lblDateOfBirth";
            lblDateOfBirth.Size = new Size(90, 15);
            lblDateOfBirth.Text = "Дата рождения:";

            lblPhoto.AutoSize = true;
            lblPhoto.Location = new Point(12, 165);
            lblPhoto.Name = "lblPhoto";
            lblPhoto.Size = new Size(40, 15);
            lblPhoto.Text = "Фото:";

            txtFirstName.Location = new Point(120, 12);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(200, 23);
            txtFirstName.TabIndex = 0;

            txtLastName.Location = new Point(120, 42);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(200, 23);
            txtLastName.TabIndex = 1;

            txtMiddleName.Location = new Point(120, 72);
            txtMiddleName.Name = "txtMiddleName";
            txtMiddleName.Size = new Size(200, 23);
            txtMiddleName.TabIndex = 2;

            cmbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCountry.FormattingEnabled = true;
            cmbCountry.Location = new Point(120, 102);
            cmbCountry.Name = "cmbCountry";
            cmbCountry.Size = new Size(200, 23);
            cmbCountry.TabIndex = 3;

            dtpDateOfBirth.Location = new Point(120, 132);
            dtpDateOfBirth.Name = "dtpDateOfBirth";
            dtpDateOfBirth.Size = new Size(200, 23);
            dtpDateOfBirth.TabIndex = 4;

            btnLoadPhoto.Location = new Point(120, 162);
            btnLoadPhoto.Name = "btnLoadPhoto";
            btnLoadPhoto.Size = new Size(100, 30);
            btnLoadPhoto.TabIndex = 5;
            btnLoadPhoto.Text = "Загрузить фото";
            btnLoadPhoto.UseVisualStyleBackColor = true;
            btnLoadPhoto.Click += BtnLoadPhoto_Click;

            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(340, 12);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(150, 180);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 6;
            pictureBox.TabStop = false;

            btnSave.Location = new Point(164, 210);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 7;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            btnCancel.Location = new Point(245, 210);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(502, 252);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(pictureBox);
            Controls.Add(btnLoadPhoto);
            Controls.Add(dtpDateOfBirth);
            Controls.Add(cmbCountry);
            Controls.Add(txtMiddleName);
            Controls.Add(txtLastName);
            Controls.Add(txtFirstName);
            Controls.Add(lblPhoto);
            Controls.Add(lblDateOfBirth);
            Controls.Add(lblCountry);
            Controls.Add(lblMiddleName);
            Controls.Add(lblLastName);
            Controls.Add(lblFirstName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AthleteEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = athlete == null ? "Добавить спортсмена" : "Редактировать спортсмена";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
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
            if (athlete != null)
            {
                txtFirstName.Text = athlete.FirstName;
                txtLastName.Text = athlete.LastName;
                txtMiddleName.Text = athlete.MiddleName ?? "";
                cmbCountry.SelectedValue = athlete.CountryId;
                dtpDateOfBirth.Value = athlete.DateOfBirth;
                photoData = athlete.Photo;
                if (athlete.Photo != null)
                {
                    using (var ms = new MemoryStream(athlete.Photo))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        private void BtnLoadPhoto_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    photoData = File.ReadAllBytes(openFileDialog.FileName);
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbCountry.SelectedValue == null)
            {
                MessageBox.Show("Выберите страну", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (athlete == null)
                athlete = new Athlete();

            athlete.FirstName = txtFirstName.Text;
            athlete.LastName = txtLastName.Text;
            athlete.MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text;
            athlete.CountryId = (int)cmbCountry.SelectedValue;
            athlete.DateOfBirth = dtpDateOfBirth.Value;
            athlete.Photo = photoData;

            if (athlete.AthleteId == 0)
                dbHelper.AddAthlete(athlete);
            else
                dbHelper.UpdateAthlete(athlete);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

