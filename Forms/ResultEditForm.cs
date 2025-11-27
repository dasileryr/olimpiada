using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class ResultEditForm : Form
    {
        private DatabaseHelper dbHelper;
        private Result? result;
        private ComboBox cmbOlympics;
        private ComboBox cmbSport;
        private ComboBox cmbAthlete;
        private ComboBox cmbMedal;
        private Button btnSave;
        private Button btnCancel;

        public ResultEditForm(DatabaseHelper dbHelper, Result? result)
        {
            this.dbHelper = dbHelper;
            this.result = result;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            Label lblOlympics = new Label();
            Label lblSport = new Label();
            Label lblAthlete = new Label();
            Label lblMedal = new Label();
            cmbOlympics = new ComboBox();
            cmbSport = new ComboBox();
            cmbAthlete = new ComboBox();
            cmbMedal = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();

            lblOlympics.AutoSize = true;
            lblOlympics.Location = new Point(12, 15);
            lblOlympics.Name = "lblOlympics";
            lblOlympics.Size = new Size(70, 15);
            lblOlympics.Text = "Олимпиада:";

            lblSport.AutoSize = true;
            lblSport.Location = new Point(12, 45);
            lblSport.Name = "lblSport";
            lblSport.Size = new Size(75, 15);
            lblSport.Text = "Вид спорта:";

            lblAthlete.AutoSize = true;
            lblAthlete.Location = new Point(12, 75);
            lblAthlete.Name = "lblAthlete";
            lblAthlete.Size = new Size(70, 15);
            lblAthlete.Text = "Спортсмен:";

            lblMedal.AutoSize = true;
            lblMedal.Location = new Point(12, 105);
            lblMedal.Name = "lblMedal";
            lblMedal.Size = new Size(45, 15);
            lblMedal.Text = "Медаль:";

            cmbOlympics.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOlympics.FormattingEnabled = true;
            cmbOlympics.Location = new Point(100, 12);
            cmbOlympics.Name = "cmbOlympics";
            cmbOlympics.Size = new Size(200, 23);
            cmbOlympics.TabIndex = 0;

            cmbSport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSport.FormattingEnabled = true;
            cmbSport.Location = new Point(100, 42);
            cmbSport.Name = "cmbSport";
            cmbSport.Size = new Size(200, 23);
            cmbSport.TabIndex = 1;

            cmbAthlete.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAthlete.FormattingEnabled = true;
            cmbAthlete.Location = new Point(100, 72);
            cmbAthlete.Name = "cmbAthlete";
            cmbAthlete.Size = new Size(200, 23);
            cmbAthlete.TabIndex = 2;

            cmbMedal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedal.FormattingEnabled = true;
            cmbMedal.Items.AddRange(new object[] { "Золото", "Серебро", "Бронза" });
            cmbMedal.Location = new Point(100, 102);
            cmbMedal.Name = "cmbMedal";
            cmbMedal.Size = new Size(200, 23);
            cmbMedal.TabIndex = 3;

            btnSave.Location = new Point(124, 140);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            btnCancel.Location = new Point(205, 140);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(312, 182);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cmbMedal);
            Controls.Add(cmbAthlete);
            Controls.Add(cmbSport);
            Controls.Add(cmbOlympics);
            Controls.Add(lblMedal);
            Controls.Add(lblAthlete);
            Controls.Add(lblSport);
            Controls.Add(lblOlympics);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ResultEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = result == null ? "Добавить результат" : "Редактировать результат";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadData()
        {
            var olympics = dbHelper.GetAllOlympics();
            cmbOlympics.DataSource = olympics.Select(o => new
            {
                o.OlympicsId,
                Display = $"{o.Year} - {o.City} ({o.HostCountryName})"
            }).ToList();
            cmbOlympics.DisplayMember = "Display";
            cmbOlympics.ValueMember = "OlympicsId";

            var sports = dbHelper.GetAllSports();
            cmbSport.DataSource = sports;
            cmbSport.DisplayMember = "SportName";
            cmbSport.ValueMember = "SportId";

            var athletes = dbHelper.GetAllAthletes();
            cmbAthlete.DataSource = athletes;
            cmbAthlete.DisplayMember = "FullName";
            cmbAthlete.ValueMember = "AthleteId";

            if (result != null)
            {
                cmbOlympics.SelectedValue = result.OlympicsId;
                cmbSport.SelectedValue = result.SportId;
                cmbAthlete.SelectedValue = result.AthleteId;
                cmbMedal.SelectedIndex = result.MedalType - 1;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbOlympics.SelectedValue == null)
            {
                MessageBox.Show("Выберите олимпиаду", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbSport.SelectedValue == null)
            {
                MessageBox.Show("Выберите вид спорта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbAthlete.SelectedValue == null)
            {
                MessageBox.Show("Выберите спортсмена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbMedal.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите медаль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (result == null)
                result = new Result();

            result.OlympicsId = (int)cmbOlympics.SelectedValue;
            result.SportId = (int)cmbSport.SelectedValue;
            result.AthleteId = (int)cmbAthlete.SelectedValue;
            result.MedalType = cmbMedal.SelectedIndex + 1;

            if (result.ResultId == 0)
                dbHelper.AddResult(result);
            else
                dbHelper.UpdateResult(result);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

