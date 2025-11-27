using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class MedalistsForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool byOlympics;
        private DataGridView dataGridView;
        private ComboBox cmbOlympics;
        private ComboBox cmbSport;
        private Label lblOlympics;
        private Label lblSport;
        private Button btnShow;
        private Button btnClose;

        public MedalistsForm(DatabaseHelper dbHelper, bool byOlympics)
        {
            this.dbHelper = dbHelper;
            this.byOlympics = byOlympics;
            InitializeComponent();
            LoadSports();
            if (byOlympics)
                LoadOlympics();
            else
                LoadData();
        }

        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            cmbOlympics = new ComboBox();
            cmbSport = new ComboBox();
            lblOlympics = new Label();
            lblSport = new Label();
            btnShow = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();

            lblOlympics.AutoSize = true;
            lblOlympics.Location = new Point(12, 15);
            lblOlympics.Name = "lblOlympics";
            lblOlympics.Size = new Size(70, 15);
            lblOlympics.Text = "Олимпиада:";
            lblOlympics.Visible = byOlympics;

            lblSport.AutoSize = true;
            lblSport.Location = new Point(12, byOlympics ? 45 : 15);
            lblSport.Name = "lblSport";
            lblSport.Size = new Size(75, 15);
            lblSport.Text = "Вид спорта:";

            cmbOlympics.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOlympics.FormattingEnabled = true;
            cmbOlympics.Location = new Point(100, 12);
            cmbOlympics.Name = "cmbOlympics";
            cmbOlympics.Size = new Size(300, 23);
            cmbOlympics.TabIndex = 0;
            cmbOlympics.Visible = byOlympics;

            cmbSport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSport.FormattingEnabled = true;
            cmbSport.Location = new Point(100, byOlympics ? 42 : 12);
            cmbSport.Name = "cmbSport";
            cmbSport.Size = new Size(300, 23);
            cmbSport.TabIndex = 1;

            btnShow.Location = new Point(406, byOlympics ? 42 : 12);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(100, 30);
            btnShow.TabIndex = 2;
            btnShow.Text = "Показать";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += BtnShow_Click;

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, byOlympics ? 80 : 50);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(960, 400);
            dataGridView.TabIndex = 3;

            btnClose.Location = new Point(872, byOlympics ? 490 : 460);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 4;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, byOlympics ? 532 : 502);
            Controls.Add(btnClose);
            Controls.Add(dataGridView);
            Controls.Add(btnShow);
            Controls.Add(cmbSport);
            Controls.Add(cmbOlympics);
            Controls.Add(lblSport);
            Controls.Add(lblOlympics);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MedalistsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = byOlympics ? "Медалисты по олимпиаде" : "Медалисты за всю историю";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

        private void LoadSports()
        {
            var sports = dbHelper.GetAllSports();
            var sportsList = new List<Sport> { new Sport { SportId = 0, SportName = "Все виды спорта" } };
            sportsList.AddRange(sports);
            cmbSport.DataSource = sportsList;
            cmbSport.DisplayMember = "SportName";
            cmbSport.ValueMember = "SportId";
        }

        private void LoadData()
        {
            int? olympicsId = byOlympics && cmbOlympics.SelectedValue != null 
                ? (int)cmbOlympics.SelectedValue 
                : null;

            int? sportId = cmbSport.SelectedValue != null && (int)cmbSport.SelectedValue != 0
                ? (int)cmbSport.SelectedValue
                : null;

            var results = dbHelper.GetMedalistsBySport(olympicsId, sportId);
            var olympics = dbHelper.GetAllOlympics();
            dataGridView.DataSource = results.Select(r => new
            {
                Олимпиада = $"{olympics.FirstOrDefault(o => o.OlympicsId == r.OlympicsId)?.Year} - {olympics.FirstOrDefault(o => o.OlympicsId == r.OlympicsId)?.City}",
                Вид_спорта = r.SportName,
                Спортсмен = r.AthleteName,
                Медаль = r.MedalName
            }).ToList();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (byOlympics && cmbOlympics.SelectedValue == null)
            {
                MessageBox.Show("Выберите олимпиаду", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadData();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

