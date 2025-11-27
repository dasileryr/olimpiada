using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class MedalStandingForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool byOlympics;
        private DataGridView dataGridView;
        private ComboBox cmbOlympics;
        private Label lblOlympics;
        private Button btnShow;
        private Button btnClose;

        public MedalStandingForm(DatabaseHelper dbHelper, bool byOlympics)
        {
            this.dbHelper = dbHelper;
            this.byOlympics = byOlympics;
            InitializeComponent();
            if (byOlympics)
                LoadOlympics();
            else
                LoadData();
        }

        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            cmbOlympics = new ComboBox();
            lblOlympics = new Label();
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

            cmbOlympics.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOlympics.FormattingEnabled = true;
            cmbOlympics.Location = new Point(100, 12);
            cmbOlympics.Name = "cmbOlympics";
            cmbOlympics.Size = new Size(300, 23);
            cmbOlympics.TabIndex = 0;
            cmbOlympics.Visible = byOlympics;

            btnShow.Location = new Point(406, 12);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(100, 30);
            btnShow.TabIndex = 1;
            btnShow.Text = "Показать";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Visible = byOlympics;
            btnShow.Click += BtnShow_Click;

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, byOlympics ? 50 : 12);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(760, 400);
            dataGridView.TabIndex = 2;

            btnClose.Location = new Point(672, byOlympics ? 460 : 422);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 3;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, byOlympics ? 502 : 464);
            Controls.Add(btnClose);
            Controls.Add(dataGridView);
            Controls.Add(btnShow);
            Controls.Add(cmbOlympics);
            Controls.Add(lblOlympics);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MedalStandingForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = byOlympics ? "Медальный зачет по олимпиаде" : "Медальный зачет за всю историю";
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

        private void LoadData()
        {
            int? olympicsId = byOlympics && cmbOlympics.SelectedValue != null 
                ? (int)cmbOlympics.SelectedValue 
                : null;

            var standings = dbHelper.GetMedalStanding(olympicsId);
            dataGridView.DataSource = standings.Select(s => new
            {
                Страна = s.CountryName,
                Золото = s.Gold,
                Серебро = s.Silver,
                Бронза = s.Bronze,
                Всего = s.Total
            }).ToList();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (cmbOlympics.SelectedValue == null)
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

