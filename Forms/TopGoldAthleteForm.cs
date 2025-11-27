using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class TopGoldAthleteForm : Form
    {
        private DatabaseHelper dbHelper;
        private Label lblResult;
        private ComboBox cmbSport;
        private Label lblSport;
        private Button btnShow;
        private Button btnClose;

        public TopGoldAthleteForm(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
            InitializeComponent();
            LoadSports();
        }

        private void InitializeComponent()
        {
            lblResult = new Label();
            cmbSport = new ComboBox();
            lblSport = new Label();
            btnShow = new Button();
            btnClose = new Button();
            SuspendLayout();

            lblSport.AutoSize = true;
            lblSport.Location = new Point(12, 15);
            lblSport.Name = "lblSport";
            lblSport.Size = new Size(75, 15);
            lblSport.Text = "Вид спорта:";

            cmbSport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSport.FormattingEnabled = true;
            cmbSport.Location = new Point(100, 12);
            cmbSport.Name = "cmbSport";
            cmbSport.Size = new Size(300, 23);
            cmbSport.TabIndex = 0;

            btnShow.Location = new Point(406, 12);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(100, 30);
            btnShow.TabIndex = 1;
            btnShow.Text = "Показать";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += BtnShow_Click;

            lblResult.AutoSize = false;
            lblResult.Location = new Point(12, 50);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(560, 200);
            lblResult.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblResult.TextAlign = ContentAlignment.MiddleCenter;

            btnClose.Location = new Point(472, 260);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 2;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 302);
            Controls.Add(btnClose);
            Controls.Add(lblResult);
            Controls.Add(btnShow);
            Controls.Add(cmbSport);
            Controls.Add(lblSport);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TopGoldAthleteForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Спортсмен с больше всего золотых медалей";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadSports()
        {
            var sports = dbHelper.GetAllSports();
            cmbSport.DataSource = sports;
            cmbSport.DisplayMember = "SportName";
            cmbSport.ValueMember = "SportId";
        }

        private void LoadData()
        {
            if (cmbSport.SelectedValue == null)
            {
                MessageBox.Show("Выберите вид спорта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sportId = (int)cmbSport.SelectedValue;
            var athletes = dbHelper.GetAthleteWithMostGoldMedalsInSport(sportId);
            if (athletes.Count > 0)
            {
                var athlete = athletes[0];
                lblResult.Text = $"Спортсмен: {athlete.FullName}\n\n" +
                               $"Страна: {athlete.CountryName}\n" +
                               $"Дата рождения: {athlete.DateOfBirth.ToShortDateString()}";
            }
            else
            {
                lblResult.Text = "Данные не найдены";
            }
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

