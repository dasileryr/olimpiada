using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class SportEditForm : Form
    {
        private DatabaseHelper dbHelper;
        private Sport? sport;
        private TextBox txtName;
        private Button btnSave;
        private Button btnCancel;

        public SportEditForm(DatabaseHelper dbHelper, Sport? sport)
        {
            this.dbHelper = dbHelper;
            this.sport = sport;
            InitializeComponent();
            if (sport != null)
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
            lblName.Size = new Size(75, 15);
            lblName.Text = "Вид спорта:";

            txtName.Location = new Point(100, 12);
            txtName.Name = "txtName";
            txtName.Size = new Size(200, 23);
            txtName.TabIndex = 0;

            btnSave.Location = new Point(144, 50);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 1;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            btnCancel.Location = new Point(225, 50);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(312, 92);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtName);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SportEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = sport == null ? "Добавить вид спорта" : "Редактировать вид спорта";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadData()
        {
            if (sport != null)
                txtName.Text = sport.SportName;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название вида спорта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sport == null)
                sport = new Sport();

            sport.SportName = txtName.Text;

            if (sport.SportId == 0)
                dbHelper.AddSport(sport);
            else
                dbHelper.UpdateSport(sport);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

